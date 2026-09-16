FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Silver.Engine/Silver.Engine.csproj Silver.Engine/
COPY Silver.Api/Silver.Api.csproj Silver.Api/
RUN dotnet restore Silver.Api/Silver.Api.csproj

COPY Silver.Engine/ Silver.Engine/
COPY Silver.Api/ Silver.Api/
RUN dotnet publish Silver.Api/Silver.Api.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://+:${PORT:-5000}
EXPOSE 5000
ENTRYPOINT ["dotnet", "Silver.Api.dll"]