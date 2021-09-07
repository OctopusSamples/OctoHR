chmod +x ./OctopusSamples.OctoHR.ConfigDBMigrator
connectionString=$(get_octopusvariable "ConnectionStrings:ConfigurationDatabase")
./OctopusSamples.OctoHR.ConfigDBMigrator "$connectionString"
