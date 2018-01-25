param (
    [Parameter(Mandatory=$true)][String] $xunitConsole,
    [Parameter(Mandatory=$true)][String] $sourceDirectory
)
 
if (!(Test-Path $xunitConsole))
{
    throw "xunit.console.exe does not exist. Please check $xunitConsole file."
}
 
$assembliesToTest = (Get-ChildItem "$sourceDirectory" -Recurse -Include "*Test*.dll" -Exclude "*JavaScript*" -Name | Select-String "bin")
 
$atLeastOneTestRun = $false
 
Write-Host "[Debug] ************* Running Unit Tests *************"
 
foreach ($testFile in $assembliesToTest) {
    Write-Host "[Debug] *************** Testing $testFile ***************"
 
  $fileNameOnly = Split-Path $testFile -Leaf
  $unitTestFileName = Join-Path $sourceDirectory ($fileNameOnly + "-XunitTestResult.xml")
 
  $fullNameTestFile = Join-Path $sourceDirectory $testFile
 
  & $xunitConsole $fullNameTestFile -xml $unitTestFileName
  $atLeastOneTestRun = $true
}
 
if ($atLeastOneTestRun -eq $false) {
    Write-Output "Unit Tests didn't run!"
    exit(1)
}