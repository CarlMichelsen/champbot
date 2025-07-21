ARG PROJECT_NAME=Api

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG PROJECT_NAME
WORKDIR /src
COPY ["${PROJECT_NAME}/${PROJECT_NAME}.csproj", "${PROJECT_NAME}/"]
RUN dotnet restore "${PROJECT_NAME}/${PROJECT_NAME}.csproj"
COPY . .
WORKDIR "/src/${PROJECT_NAME}"
RUN dotnet build "${PROJECT_NAME}.csproj" -c Release -o /app/build

FROM build AS publish
ARG PROJECT_NAME
RUN dotnet publish "${PROJECT_NAME}.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
ARG PROJECT_NAME
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV PROJECT_NAME=${PROJECT_NAME}
ENTRYPOINT dotnet ${PROJECT_NAME}.dll