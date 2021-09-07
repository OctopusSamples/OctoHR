chmod +x ./OctopusSamples.OctoHR.DatabaseMigrator
connectionString=$(get_octopusvariable "ConnectionStrings:ConfigurationDatabase")
./OctopusSamples.OctoHR.DatabaseMigrator "$connectionString"
