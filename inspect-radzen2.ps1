$xml = 'C:\Users\divulu\Desktop\Dev\Gestion_Hopital\radzen-api.xml'
Write-Output '--- RadzenTemplateForm members ---'
Select-String -Path $xml -Pattern 'name="[TPME]:Radzen.Blazor.RadzenTemplateForm' | ForEach-Object { $_.LineNumber.ToString() + ': ' + $_.Line.Trim() }
Write-Output '--- Validator base members ---'
Select-String -Path $xml -Pattern 'name="[TPME]:Radzen.Blazor.RadzenFormValidatorBase' | ForEach-Object { $_.LineNumber.ToString() + ': ' + $_.Line.Trim() }
Write-Output '--- CompareValidator members ---'
Select-String -Path $xml -Pattern 'name="[TPME]:Radzen.Blazor.RadzenCompareValidator' | ForEach-Object { $_.LineNumber.ToString() + ': ' + $_.Line.Trim() }
Write-Output '--- ValidatorComponentBase / RadzenValidator ---'
Select-String -Path $xml -Pattern 'name="[TPME]:Radzen.Blazor.RadzenValidator' | ForEach-Object { $_.LineNumber.ToString() + ': ' + $_.Line.Trim() }
Write-Output '--- RadzenForm extra components ---'
Select-String -Path $xml -Pattern 'name="T:Radzen.Blazor.RadzenForm' | ForEach-Object { $_.LineNumber.ToString() + ': ' + $_.Line.Trim() } | Select-Object -First 40