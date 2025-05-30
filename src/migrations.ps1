# Caminho para o projeto que contém o contexto do Entity Framework Core
$infrastructureProjectPathRelative = "SplitCost.Infrastructure\SplitCost.Infrastructure.csproj"
$infrastructureProjectPathFull = Join-Path $PSScriptRoot $infrastructureProjectPathRelative

# Comando para adicionar uma nova migration (opcional)
$migrationName = Read-Host "Digite o nome da nova migration (ou deixe em branco para apenas aplicar as existentes)"

if ($migrationName) {
    Write-Host "Adicionando migration '$migrationName'..."
    dotnet ef migrations add $migrationName --project "$infrastructureProjectPathFull" --startup-project "$infrastructureProjectPathFull"
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Erro ao adicionar a migration."
        exit 1
    } else {
        Write-Host "Migration '$migrationName' adicionada com sucesso."
    }
}

# Comando para aplicar as migrations pendentes ao banco de dados
Write-Host "Aplicando as migrations..."
dotnet ef database update --project "$infrastructureProjectPathFull" --startup-project "$infrastructureProjectPathFull"

if ($LASTEXITCODE -ne 0) {
    Write-Error "Erro ao aplicar as migrations."
    exit 1
} else {
    Write-Host "Migrations aplicadas com sucesso ao banco de dados."
}

Write-Host "Processo de migrations concluído."