Get-ChildItem -Path 'C:/Users/divulu/Desktop/Dev/Gestion_Hopital' -Recurse -File |
  Where-Object { $_.Name -match '.(cshtml|razor|cs|json|html|css|js|ts|scss)$' } |
  Select-Object -ExpandProperty FullName |
  sort