$xml = 'C:\Users\divulu\Desktop\Dev\Gestion_Hopital\radzen-api.xml'
Write-Output '--- TemplateForm ---'
Select-String -Path $xml -Pattern 'TemplateForm' -SimpleMatch | Select-Object -First 12 | ForEach-Object { $_.LineNumber.ToString() + ': ' + $_.Line.Trim() }
Write-Output '--- DialogService ---'
Select-String -Path $xml -Pattern 'DialogService' -SimpleMatch | Select-Object -First 12 | ForEach-Object { $_.LineNumber.ToString() + ': ' + $_.Line.Trim() }
Write-Output '--- Confirm ---'
Select-String -Path $xml -Pattern 'Confirm' -SimpleMatch | Select-Object -First 25 | ForEach-Object { $_.LineNumber.ToString() + ': ' + $_.Line.Trim() }
Write-Output '--- RadzenComponents ---'
Select-String -Path $xml -Pattern 'RadzenComponents' -SimpleMatch | Select-Object -First 5 | ForEach-Object { $_.LineNumber.ToString() + ': ' + $_.Line.Trim() }

