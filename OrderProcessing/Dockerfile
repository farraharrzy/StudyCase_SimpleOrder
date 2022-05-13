#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OrderProcessing/OrderProcessing.csproj", "OrderProcessing/"]
RUN dotnet restore "OrderProcessing/OrderProcessing.csproj"
COPY . .
WORKDIR "/src/OrderProcessing"
RUN dotnet build "OrderProcessing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderProcessing.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderProcessing.dll"]
