// This test will put the system under the existing current load

import { check, group } from "k6";
import { Rate } from "k6/metrics";
import helper from "./helper.js";
import api from "./apiClient.js";
import { Counter } from 'k6/metrics';

export let options = {
    discardResponseBodies: true,
    thresholds: {
        http_req_failed: ['rate<0.01'],   // http errors should be less than 1% 
        http_req_duration: ['p(95)<60'], // 95% of requests should be below 60ms
    },
    scenarios: {
        baselineTest: {
            executor: 'ramping-arrival-rate',
            startRate: 10,
            timeUnit: '1s',
            preAllocatedVUs: 10,
            maxVUs: 50,
            stages: [
                { target: 10, duration: '5s' }
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