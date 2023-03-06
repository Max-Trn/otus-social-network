FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["social-network.csproj", "social-network/"]
COPY ["NuGet.Config", "social-network/"]
RUN dotnet restore "social-network/social-network.csproj"
WORKDIR "/src/social-network"
COPY . .
RUN dotnet build "social-network.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "social-network.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "social-network.dll"]
