#!/bin/bash
export AZURE_KEY_VAULT_ENDPOINT="https://kv-ngbjruypjvzpy.vault.azure.net/"
dotnet run --project ./app/backend/MinimalApi.csproj --urls=http://localhost:7181/
