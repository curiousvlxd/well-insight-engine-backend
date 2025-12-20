CREATE MATERIALIZED VIEW well_metrics_aggregate_12h
            WITH (timescaledb.continuous) AS
SELECT
    time_bucket(INTERVAL '12 hours', time, 'Europe/Kyiv') AS time,
    well_id,
    parameter_id,
    AVG(COALESCE(avg_value, 0)) AS avg_value,
    MIN(COALESCE(min_value, 0)) AS min_value,
    MAX(COALESCE(max_value, 0)) AS max_value,
    MODE() WITHIN GROUP (ORDER BY mode_value) AS mode_value
FROM well_metrics_aggregate_6h
GROUP BY 1, 2, 3
WITH NO DATA;

CREATE INDEX IF NOT EXISTS ix_well_metrics_aggregate_12h_well_param_time
    ON well_metrics_aggregate_12h (well_id, parameter_id, time DESC);

SELECT add_continuous_aggregate_policy(
'well_metrics_aggregate_12h',
null,
null,
schedule_interval => INTERVAL '5 minutes');

CALL refresh_continuous_aggregate('well_metrics_aggregate_12h', NULL, NULL);