chmod +x ./OctopusSamples.OctoHR.ConfigDBMigrator
connectionString=$(get_octopusvariable "Database:Config:ConnectionString")
./OctopusSamples.OctoHR.ConfigDBMigrator --ConnectionString="$connectionString"
