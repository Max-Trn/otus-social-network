FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["user-posts-async-api.csproj", "user-posts-async-api/"]
RUN dotnet restore "user-posts-async-api/user-posts-async-api.csproj"
WORKDIR "/src/user-posts-async-api"
COPY . .
RUN dotnet build "user-posts-async-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "user-posts-async-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "user-posts-async-api.dll"]
