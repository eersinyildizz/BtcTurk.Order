# BtcTurk Order Api Case Study

## Prerequisites

### .NET

1. [Install .NET 7](https://dotnet.microsoft.com/en-us/download)

### RabbitMq

```
docker run -d --hostname my-rabbit --name some-rabbit rabbitmq:3
```

`RabbitMq` options can be configure at section of [appsettings.json](src/BtcTurk.Order.Api/appsettings.json):

```json
  "Event": {
    "RabbitMq": {
        "Host":"localhost",
        "Username": "guest",
        "Passsword": "guest",
        "Port":5672
    },
  },
```

### Send Requests to application

You can use [order.http](Request/order.http) on VSCode with restclient extensions

### OpenTelemetry

OrderApi uses OpenTelemetry to collect logs, metrics and spans.

If you wish to view the collected telemetry, follow the steps below.

#### Spans

1. Configure environment variable `OTEL_EXPORTER_OTLP_ENDPOINT` with the right endpoint URL to enable
1. Run Jaeger with Docker:

```
docker run -d --name jaeger -e COLLECTOR_ZIPKIN_HOST_PORT=:9411 -e COLLECTOR_OTLP_ENABLED=true -p 6831:6831/udp -p 6832:6832/udp -p 5778:5778 -p 16686:16686 -p 4317:4317 -p 4318:4318 -p 14250:14250 -p 14268:14268 -p 14269:14269 -p 9411:9411 jaegertracing/all-in-one:latest
```

1. Open [Jaeger in your browser](http://localhost:16686/)
1. View the collected spans

### Seq

For using seq, we should enable it with setting `SeqUrl` value in the `Serilog` section of [appsettings.json](src/BtcTurk.Order.Api/appsettings.json):

```json
  "Serilog": {
     ...
    "SeqUrl": "http://localhost:5341",
     ...
  },
```

### Running the application Both

To run the application, run both. Below are different ways to run both applications:

- **Docker Compose** - Open your terminal, navigate to the root folder of this project and run the following commands:
  1.  ```
      docker-compose up -d
      ```
- **Project Tye** - Open your terminal, navigate to the root folder of this project and run the following commands:
  1.  Install Tye
      ```
      dotnet tool install --global Microsoft.Tye --version 0.11.0-alpha.22111.1
      ```
  2.  Run project
      ```
      tye run
      ```
