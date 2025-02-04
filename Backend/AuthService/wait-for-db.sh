#!/bin/bash
set -e

until PGPASSWORD=neon2k2@123 psql -h "postgres" -U "neon" -d "fleetIQDb" -c '\q'; do
  >&2 echo "Postgres is unavailable - sleeping"
  sleep 1
done

>&2 echo "Postgres is up - running migrations"
dotnet ef database update

>&2 echo "Starting application"
dotnet AuthService.dll
