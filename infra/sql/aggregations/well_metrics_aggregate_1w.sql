CREATE MATERIALIZED VIEW well_metrics_aggregate_1w
            WITH (timescaledb.continuous) AS
SELECT
    time_bucket(INTERVAL '1 week', time, 'Europe/Kyiv') AS time,
    well_id,
    parameter_id,
    AVG(COALESCE(avg_value, 0)) AS avg_value,
    MIN(COALESCE(min_value, 0)) AS min_value,
    MAX(COALESCE(max_value, 0)) AS max_value,
    MODE() WITHIN GROUP (ORDER BY mode_value) AS mode_value
FROM well_metrics_aggregate_1d
GROUP BY 1, 2, 3
WITH NO DATA;

CREATE INDEX IF NOT EXISTS ix_well_metrics_aggregate_1w_well_param_time
    ON well_metrics_aggregate_1w (well_id, parameter_id, time DESC);

SELECT add_continuous_aggregate_policy(
    'well_metrics_aggregate_1w',
    null,
    null,
    schedule_interval => INTERVAL '10 minutes');

-- CALL refresh_continuous_aggregate('well_metrics_aggregate_1w', NULL, NULL);