function CreateSamplesJson {

    param (
        $DirectoryName
    )

    Get-ChildItem -Path $DirectoryName -Directory | Select-Object @{n = 'name'; e = { $_.Name } } | ConvertTo-Json -AsArray |  Set-Content "$($DirectoryName)/.samples.json"
}

CreateSamplesJson -DirectoryName Indicators
CreateSamplesJson -DirectoryName Plugins
CreateSamplesJson -DirectoryName Robots