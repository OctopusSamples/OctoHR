chmod +x ./OctopusSamples.OctoHR.DatabaseMigrator
connectionString=$(get_octopusvariable "Database:Config:ConnectionString")
./OctopusSamples.OctoHR.DatabaseMigrator --ConfigConnectionString="$connectionString"
