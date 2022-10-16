FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY src/ .

RUN dotnet restore OnlineStore.Legacy/OnlineStore.Legacy.csproj
WORKDIR /OnlineStore.Legacy
RUN dotnet publish -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build OnlineStore.Legacy/out .

EXPOSE 5000
ENTRYPOINT ["dotnet", "OnlineStore.Legacy.dll"]