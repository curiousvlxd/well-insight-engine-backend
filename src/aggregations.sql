CREATE MATERIALIZED VIEW well_metrics_aggregate_1m
            WITH (timescaledb.continuous) AS
SELECT
    time_bucket(INTERVAL '1 minute', time, 'Europe/Kyiv') AS time,
    well_id,
    parameter_id,
    AVG(CASE WHEN value ~ '^[-+]?[0-9]*\.?[0-9]+$' THEN COALESCE(CAST(value AS numeric), 0) END) AS avg_value,
    MIN(CASE WHEN value ~ '^[-+]?[0-9]*\.?[0-9]+$' THEN COALESCE(CAST(value AS numeric), 0) END) AS min_value,
    MAX(CASE WHEN value ~ '^[-+]?[0-9]*\.?[0-9]+$' THEN COALESCE(CAST(value AS numeric), 0) END) AS max_value,
    MODE() WITHIN GROUP (ORDER BY value)
    FILTER (WHERE value ~ '^(?=.*[a-zA-Zа-яА-ЯёЁіІїЇєЄґҐ])[^\d]+$') AS mode_value
FROM well_metrics
WHERE value ~ '^[-+]?[0-9]*\.?[0-9]+$'
   OR value ~ '^(?=.*[a-zA-Zа-яА-ЯёЁіІїЇєЄґҐ])[^\d]+$'
GROUP BY 1, 2, 3
WITH NO DATA;
