[![.NET](https://github.com/msmithtcg/LogLevelRuntimeSwitching/actions/workflows/dotnet.yml/badge.svg)](https://github.com/msmithtcg/LogLevelRuntimeSwitching/actions/workflows/dotnet.yml)

# Run-time Log Level Switching for Serilog

## Run it yourself

1. Create a GitHub Repo to house your Serilog configuration. (ie: [https://github.com/msmithtcg/LoggingLevelConfig](https://github.com/msmithtcg/LoggingLevelConfig))
2. Create a Personal Access Token, scoped to Repository functionality. (here: [https://github.com/settings/tokens](https://github.com/settings/tokens))
3. Modify `AppSettings.json`

ie: 

```json
{
  "github": {
    "token": "ohhey.thisismy.token",
    "productName": "LoggingLevelConfig",
    "repo": "LoggingLevelConfig",
    "repoOwner": "msmithtcg",
    "repoBranch": "main"
  },
  "logging": {
    "configFile": "LoggingSettings.json"
  },
  "checkIntervalMs": 5000
}
```

4. If your Serilog minimum level in your repo is Information, it will output your info log found on line 43 of `ConfigurationService.cs`. If its higher than Information, you will not see it.

## How does it work?

Based on the value configured in `checkIntervalMs`, the SHA of the configuration repository will be checked at a specified interval. If the SHA has changed, it will pull down the new version, overwrite the `logging:configFile` file, and reconfigure the logging level for Serilog.

## Next Steps?!

* The timer is yucky. It should be moved to a hosted service maybe? If it was an API or something that could receive webhook calls .. that'd cut down on the polling as well!
