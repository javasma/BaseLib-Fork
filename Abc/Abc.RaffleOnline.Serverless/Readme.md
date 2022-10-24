# RaffleOnline Serverless Application

Una aplicación AWS Serverless con los siguientes componentes:

* Un AWS Lambda + Amazon API Gateway que expone un api ASP.NET Core Web API para los llamados sincrónicos de servicios.
* Un AWS Lambda + SQS con el handler de mensajes de una cola SQS 


* El paquete [Amazon.Lambda.AspNetCoreServer](https://www.nuget.org/packages/Amazon.Lambda.AspNetCoreServer) expone un Lambda que traduce los request del API Gateway, los envia a ASP.NET Core y envia la respuesta de vuelta al API Gateway.

* La plantilla serverless.template de CloudFormation tiene los dos recursos definidos con  `AWS::Serverless::Function`.

* LocalEntryPoint.cs es el entry point normal de ASP.NET Core.

## LambdaEntryPoint.cs:

El ancestro depende del tipo de payload del API Gateway:

* API Gateway REST API -> Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
* API Gateway HTTP API payload version 1.0 -> Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
* API Gateway HTTP API payload version 2.0 -> Amazon.Lambda.AspNetCoreServer.APIGatewayHttpApiV2ProxyFunction
* Application Load Balancer -> Amazon.Lambda.AspNetCoreServer.ApplicationLoadBalancerFunction

**Importante:** `AWS::Serverless::Function` + `HttpApi` the default payload
format is 2.0.

## Amazon Lambda Global Tools:

Extensiones para el .NET CLI para build y deploy de Lambdas.

* [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools)

```
    dotnet tool install -g Amazon.Lambda.Tools
    dotnet tool update -g Amazon.Lambda.Tools
    dotnet lambda deploy-serverless
```
