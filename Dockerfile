FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS sdk

ARG Configuration=Release
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true

WORKDIR /src

COPY . .

RUN dotnet restore --configfile nuget.config --verbosity normal
RUN dotnet build --no-restore --configuration ${Configuration} --verbosity normal
RUN dotnet test --configuration ${Configuration} --no-build --verbosity normal
RUN dotnet publish "Source/ApiServer/ApiServer.csproj" --configuration ${Configuration} --no-build --output /app --verbosity normal

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache \
    # Install cultures to enable use of System.CultureInfo
    icu-libs \
    # Install time zone database to enable use of System.TimeZoneInfo
    tzdata

WORKDIR /app

EXPOSE 80 443
COPY --from=sdk /app .

ENTRYPOINT ["dotnet", "ApiServer.dll"]
