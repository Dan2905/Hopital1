param(
    [string]$DllPath = "$env:USERPROFILE\.nuget\packages\radzen.blazor\11.3.2\lib\net10.0\Radzen.Blazor.dll"
)

Add-Type -AssemblyName System.Reflection
$assembly = [System.Reflection.Assembly]::LoadFrom($DllPath)

Write-Output "=== DialogService.Confirm overloads ==="
$assembly.GetType('Radzen.DialogService').GetMethods() |
    Where-Object { $_.Name -eq 'Confirm' } |
    ForEach-Object { $_.ToString() }

Write-Output "=== ConfirmOptions properties ==="
$assembly.GetType('Radzen.ConfirmOptions').GetProperties() |
    ForEach-Object { "$($_.PropertyType.Name) $($_.Name)" }

Write-Output "=== TemplateForm types ==="
$assembly.GetTypes() |
    Where-Object { $_.Name -like '*TemplateForm*' } |
    ForEach-Object { $_.FullName }

Write-Output "=== RadzenTemplateForm parameters ==="
$formType = $assembly.GetTypes() | Where-Object { $_.Name -like 'RadzenTemplateForm*' } | Select-Object -First 1
if ($null -ne $formType) {
    $formType.GetProperties() | ForEach-Object { "$($_.PropertyType.Name) $($_.Name)" }
}

Write-Output "=== Validators ==="
$assembly.GetTypes() |
    Where-Object { $_.Name -like '*Validator*' } |
    ForEach-Object { $_.FullName }