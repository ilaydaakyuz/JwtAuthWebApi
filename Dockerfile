# .NET SDK image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MyWebApi.csproj", "./"]
RUN dotnet restore "./MyWebApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MyWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyWebApi.dll"]
