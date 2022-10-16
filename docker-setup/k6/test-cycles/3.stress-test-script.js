// this test push the application to failure and or observe performance degradation
// can observe latency, exeptions

import { check, group } from "k6";
import { Rate } from "k6/metrics";
import helper from "./helper.js";
import api from "./apiClient.js";
import { Counter } from 'k6/metrics';

export let options = {
    discardResponseBodies: true,
    scenarios: {
        stressTest: {
            executor: 'ramping-arrival-rate',
            startRate: 10,
            timeUnit: '1s',
            preAllocatedVUs: 100,
            maxVUs: 300,
            stages: [
                { target: 10, duration: '5s' },
                { target: 100, duration: '5s' },
                { target: 500, duration: '20s' },
                { target: 1000, duration: '10s' },  // around the breaking point
                { target: 1000, duration: '10s' },  // beyond the breaking point
                { target: 100, duration: '5s' },
                { target: 100, duration: '5s' },    // scale down. Recovery stage.
            ],
        },
    },
};

export let orderErrors = new Counter("orderErrors");
let tpsCounter = new Counter('tpsCounter');

export default function () {

    group("Create a new order", function () {
        const request = helper.createOrder();
        const response = api.post(request);

        tpsCounter.add(1);

        const isOK = response.status == 200;
        if (isOK == false)
        orderErrors.add(1);

        check(response, {
            "Order is successful": (r) => isOK
        });
    });

};