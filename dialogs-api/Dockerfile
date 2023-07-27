FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["dialogs-api.csproj", "dialogs-api/"]
COPY ["NuGet.Config", "dialogs-api/"]
RUN dotnet restore "dialogs-api/dialogs-api.csproj"
WORKDIR "/src/dialogs-api"
COPY . .
RUN dotnet build "dialogs-api.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "dialogs-api.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dialogs-api.dll"]
