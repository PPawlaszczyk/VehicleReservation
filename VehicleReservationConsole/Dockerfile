# syntax=docker/dockerfile:1.4

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS base

RUN apt-get update && \
    apt-get install -y git curl && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

WORKDIR /VehicleReservationConsole

COPY VehicleReservationConsole.csproj ./
RUN ["dotnet", "restore"]

FROM base as builder

COPY . .

CMD ["dotnet", "build", "-c", "-o", "/build"]

FROM builder as dev-envs

COPY --from=gloursdocker/docker / /

CMD ["dotnet", "run"]

FROM builder AS publisher

RUN ["dotnet", "publish", "VehicleReservationConsole.csproj" ,"-c", "Release", "-o", "/build"]

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=publisher /build .

CMD ["dotnet", "VehicleReservationConsole.dll"] 
