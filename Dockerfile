# syntax=docker/dockerfile:1

# --- Frontend ---
FROM node:22-alpine AS frontend
WORKDIR /src/frontend
COPY frontend/package.json frontend/package-lock.json ./
RUN npm ci
COPY frontend/ ./
# Same-origin API in the combined image
ENV VITE_API_BASE=
RUN npm run build

# --- Backend ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend
WORKDIR /src
COPY backend/InterviewTutor.Api.csproj backend/
RUN dotnet restore backend/InterviewTutor.Api.csproj
COPY backend/ backend/
COPY content/ content/
COPY --from=frontend /src/frontend/dist/ backend/wwwroot/
RUN dotnet publish backend/InterviewTutor.Api.csproj -c Release -o /app/publish /p:UseAppHost=false \
    && mkdir -p /app/publish/content \
    && cp -r content/. /app/publish/content/

# --- Runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=backend /app/publish ./
ENV ASPNETCORE_ENVIRONMENT=Production
ENV CONTENT_ROOT=/app/content
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "InterviewTutor.Api.dll"]
