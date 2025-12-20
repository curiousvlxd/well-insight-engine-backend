#!/usr/bin/env bash
set -e

echo "⏳ Waiting for TimescaleDB..."
until pg_isready -h "$PGHOST" -p "$PGPORT" -U "$PGUSER"; do
  sleep 2
done

echo "✅ TimescaleDB is ready"

FIRST_FILE="/sql/well_metrics.sql"

if [ ! -f "$FIRST_FILE" ]; then
  echo "❌ well_metrics.sql not found"
  exit 1
fi

echo "▶ Running $FIRST_FILE"
psql -f "$FIRST_FILE"

echo "▶ Running remaining SQL files..."

find /sql -type f -name "*.sql" ! -name "well_metrics.sql" | sort | while read -r file; do
  echo "▶ $file"
  psql -f "$file"
done

echo "✅ SQL setup completed"