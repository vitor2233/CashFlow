FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY src/ .

WORKDIR /app/CashFlow.API

RUN dotnet restore

RUN dotnet publis -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT [ "dotnet", "CashFlow.API.dll" ]