$SCRIPTVERSION = "2.0" # The version number of this script file
##############################################

# You can add any number of additional entries to this variable by appending them like this:   "", "", ""

$versionVariableNames = "Version"
$rawVersionVariableNames = 

##########<  Don't Edit Past Here  >##########


# Get git tag
if (($raw_tag = git describe --tags --abbrev=0).Length -eq 0) { throw "No git tag found!"; }

# Parse git tag
$raw_tag -cmatch '(?<MAJOR>\d+?)\.(?<MINOR>\d+?)\.(?<PATCH>\d+?)(?<EXTRA>.*)'
$parsed_tag = $Matches.MAJOR + '.' + $Matches.MINOR + '.' + $Matches.PATCH

Write-Output    "Parsed Git Tag `"$raw_tag`":",
                "  - MAJOR: `"$($Matches.MAJOR)`"",
                "  - MINOR: `"$($Matches.MINOR)`"",
                "  - PATCH: `"$($Matches.PATCH)`"",
                "  - EXTRA: `"$($Matches.EXTRA)`"",
                "3-Part Version Number: `"$parsed_tag`""


if ($versionVariableNames.Count -eq 0) {
    Write-Error "Invalid configuration:  `$versionVariableNames = `"$versionVariableNames`" ; expected a list of strings!"
}

$dirpath = Get-Location

$projectFiles = Get-ChildItem -File -Path $dirpath -Filter "*.csproj"

if ($projectFiles.Count -eq 0) {
    Write-Error "No files matching filter `"*.csproj`" found in project `"$dirpath`""
}

foreach ($project in $projectFiles) {
    [xml]$CONTENT = Get-Content -Path $project

    Write-Output "","Opened Project File: `"$project`""

    Write-Output $CONTENT.Project

    foreach ($varName in $versionVariableNames) {
        $tmp = $CONTENT.Project.PropertyGroup.$varName
        $CONTENT.Project.PropertyGroup.$varName = $parsed_tag
        Write-Output "-> Property:  `"$varName`"",
                        "   Outgoing:  `"$tmp`"",
                        "   Incoming:  `"$parsed_tag`""
    }
    foreach ($varName in $rawVersionVariableNames) {
        $tmp = $CONTENT.Project.PropertyGroup.$varName
        $CONTENT.Project.PropertyGroup.$varName = $raw_tag
        Write-Output "-> Property:  `"$varName`"",
                        "   Outgoing:  `"$tmp`"",
                        "   Incoming:  `"$parsed_tag`""
    }

    $CONTENT.Save($project)
    Write-Output "-> Saved Project File."
}
