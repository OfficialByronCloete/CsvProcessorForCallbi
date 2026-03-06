FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY TransactionProcessor.Common/TransactionProcessor.Common.csproj TransactionProcessor.Common/
COPY TransactionProcessor.Core/TransactionProcessor.Core.csproj TransactionProcessor.Core/
COPY TransactionProcessor.Integrations.DataAccess/TransactionProcessor.Integrations.DataAccess.csproj TransactionProcessor.Integrations.DataAccess/
COPY TransactionProcessor.Services/TransactionProcessor.Services.csproj TransactionProcessor.Services/
COPY TransactionProcessorAPI/TransactionProcessorAPI.WebAPI.csproj TransactionProcessorAPI/

RUN dotnet restore TransactionProcessorAPI/TransactionProcessorAPI.WebAPI.csproj

COPY . .
WORKDIR /src/TransactionProcessorAPI
RUN dotnet publish TransactionProcessorAPI.WebAPI.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TransactionProcessorAPI.WebAPI.dll"]
