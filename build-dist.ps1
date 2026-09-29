# Compila o plugin para Revit 2024, 2025 e 2026 e monta dist\{ano}\ no layout de instalação:
#   dist\{ano}\LimpezaGeral.addin
#   dist\{ano}\LimpezaGeral\{dll, pdb, BimBlasterStyles.xaml, icon16.png, icon32.png}
$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$proj = Join-Path $root 'LimpezaGeral\LimpezaGeral.csproj'
$dist = Join-Path $root 'dist'

if (Test-Path $dist) { Remove-Item $dist -Recurse -Force }

foreach ($year in 2024, 2025, 2026) {
    dotnet build $proj -c Release -p:RevitYear=$year
    if ($LASTEXITCODE -ne 0) { throw "Build falhou para Revit $year" }

    $out = Join-Path $root "LimpezaGeral\bin\$year\Release"
    if (-not (Test-Path $out)) { $out = Join-Path $root "LimpezaGeral\bin\$year" }
    $target = Join-Path $dist "$year\LimpezaGeral"
    New-Item -ItemType Directory -Force $target | Out-Null
    foreach ($f in 'LimpezaGeral.dll', 'LimpezaGeral.pdb', 'BimBlasterStyles.xaml', 'icon16.png', 'icon32.png') {
        Copy-Item (Join-Path $out $f) $target
    }
    Copy-Item (Join-Path $root 'LimpezaGeral\LimpezaGeral.addin') (Join-Path $dist $year)
}
