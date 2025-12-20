CREATE TABLE IF NOT EXISTS well_metrics
(
    time          timestamptz NOT NULL,
    well_id       uuid        NOT NULL,
    parameter_id  uuid        NOT NULL,
    value         text        NOT NULL
);

SELECT create_hypertable('well_metrics', 'time', if_not_exists => TRUE);

CREATE INDEX IF NOT EXISTS ix_well_metrics_well_param_time
    ON well_metrics (well_id, parameter_id, time DESC);