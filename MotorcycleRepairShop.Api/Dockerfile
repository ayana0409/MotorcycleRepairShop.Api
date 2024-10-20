#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MotorcycleRepairShop.Api/MotorcycleRepairShop.Api.csproj", "MotorcycleRepairShop.Api/"]
COPY ["MotorcycleRepairShop.Application/MotorcycleRepairShop.Application.csproj", "MotorcycleRepairShop.Application/"]
COPY ["MotorcycleRepairShop.Domain/MotorcycleRepairShop.Domain.csproj", "MotorcycleRepairShop.Domain/"]
COPY ["MotorcycleRepairShop.Share/MotorcycleRepairShop.Share.csproj", "MotorcycleRepairShop.Share/"]
COPY ["MotorcycleRepairShop.Infrastructure/MotorcycleRepairShop.Infrastructure.csproj", "MotorcycleRepairShop.Infrastructure/"]
RUN dotnet restore "./MotorcycleRepairShop.Api/MotorcycleRepairShop.Api.csproj"
COPY . .
WORKDIR "/src/MotorcycleRepairShop.Api"
RUN dotnet build "./MotorcycleRepairShop.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MotorcycleRepairShop.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MotorcycleRepairShop.Api.dll"]