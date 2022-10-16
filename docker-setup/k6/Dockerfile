FROM loadimpact/k6

# for local
COPY ./scripts/ /
COPY ./test-cycles/ /

# ENTRYPOINT k6 run -o influxdb=http://influxdb:8086/k6 /1.baseline-script.js
ENTRYPOINT k6 run -o influxdb=http://influxdb:8086/k6 /2.load-test-script.js
#ENTRYPOINT k6 run -o influxdb=http://influxdb:8086/k6 /3.stress-test-script.js
#ENTRYPOINT k6 run -o influxdb=http://influxdb:8086/k6 /4.endurance-script.js