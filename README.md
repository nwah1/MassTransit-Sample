# MassTransit-Sample

A demo using MassTransit to download data from a JSON source, send the data via RabbitMq, to a service that writes the data to Entity Framework.

The data is updated every time an "api/LoadData" call is made via Web API, and stored in LocalDB. The data is displayed in console whenever an "api/Repositories" call is made.

This implements the basic skeleton of the Saga pattern. There's no logging or scheduling, and few events or states in the "saga," although it can be easily extended.

The saga repository is in memory. In a production environment, you would want to use the Entity Framework repository that MassTransit provides for persistence.

