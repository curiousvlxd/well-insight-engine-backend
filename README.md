# WellInsightEngine

WellInsightEngine is a system for generating engineering insights for gas wells based on historical telemetry data, recorded incidents/actions, and cleaned time-series technical parameters.

The solution uses AI to analyze the current state of a well in the context of known engineering cases.  
The system compares parameter dynamics with similar situations from a historical dataset and produces a clear engineering insight: what is happening, which past cases it resembles, and what actions may be appropriate.

The result is delivered as a standalone AI insight for a specific well, which can be saved and shared via a unique link for further team analysis.

---

## Architecture Overview

- **ASP.NET Core API**
- **PostgreSQL (Wells domain)**  
  Stores wells, parameters, relations, metadata (EF Core)
- **TimescaleDB (Metrics domain)**  
  Stores raw metrics and hierarchical continuous aggregates
- **AI Provider (Gemini)**  
  Used for insight generation (can be disabled locally)
- **Authentication (Clerk)**  
  JWT-based authorization with email allow-list (can be disabled locally)

All external dependencies (AI, Auth) can be mocked or disabled for local development.

---

## Key Features

- Time-series metrics ingestion
- Hierarchical continuous aggregates (1m → 5m → 10m → 30m → 1h → …)
- Fast querying via pre-aggregated TimescaleDB views
- Delta and trend analysis support
- Insight generation via AI
- Shareable insight links (slug-based)

---

## Local Development (Docker)

### Prerequisites

- Docker Desktop (Windows / macOS / Linux)
- Docker Compose v2+

### Services Started via Docker Compose

- `postgres-wells` – PostgreSQL for wells metadata
- `timescaledb` – TimescaleDB for metrics & aggregates
- `api` – WellInsightEngine API

AI and Auth are **disabled by default** in local mode.

---

## Environment Configuration

Key flags (passed via environment variables):

- `Database__MigrateOnStartup=true`
- `Ai__Disabled=true`
- `Klerk__Disabled=true`

---

## Database Initialization Flow

1. Docker Compose starts PostgreSQL and TimescaleDB
2. TimescaleDB initializes schemas
3. `infra/sql/setup.sh` is executed:
   - Runs `well_metrics.sql` first
   - Executes all other SQL files recursively
4. API starts with `MigrateOnStartup=true`
   - Applies EF Core migrations for Wells DB

---

## Running Locally

```bash
docker compose up --build
