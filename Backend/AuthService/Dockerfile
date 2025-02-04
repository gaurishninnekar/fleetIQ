FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AuthService.csproj", "./"]
RUN dotnet restore "AuthService.csproj"
COPY . .
RUN dotnet build "AuthService.csproj" -c Release -o /app/build
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Waiting for database and running migrations
COPY wait-for-db.sh /src/
RUN chmod +x /src/wait-for-db.sh

FROM build AS migrate
WORKDIR /src
RUN ./wait-for-db.sh
RUN dotnet ef database update --project AuthService.csproj

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
RUN apt-get update && apt-get install -y postgresql-client
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "AuthService.dll"]


