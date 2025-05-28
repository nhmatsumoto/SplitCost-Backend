# Caminho para o projeto que contém o contexto do Entity Framework Core
$projetoDataPath = "C:\Repositorio\SplitCost\backend\SplitCost\SplitCost.Infrastructure\SplitCost.Infrastructure.csproj"

# Caminho para o projeto de inicialização (onde reside a configuração, como string de conexão)
$projetoStartupPath = "C:\Repositorio\SplitCost\backend\SplitCost\SplitCost.Infrastructure\SplitCost.Infrastructure.csproj"


# Comando para adicionar uma nova migration (opcional, se você já tem a migration criada)
$migrationName = Read-Host "Digite o nome da nova migration (ou deixe em branco para apenas aplicar as existentes)"

if ($migrationName) {
    Write-Host "Adicionando migration '$migrationName'..."
    dotnet ef migrations add $migrationName --project $projetoDataPath --startup-project $projetoStartupPath
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Erro ao adicionar a migration."
        exit 1
    } else {
        Write-Host "Migration '$migrationName' adicionada com sucesso."
    }
}

# Comando para aplicar as migrations pendentes ao banco de dados
Write-Host "Aplicando as migrations..."
dotnet ef database update --project $projetoDataPath --startup-project $projetoStartupPath

if ($LASTEXITCODE -ne 0) {
    Write-Error "Erro ao aplicar as migrations."
    exit 1
} else {
    Write-Host "Migrations aplicadas com sucesso ao banco de dados."
}

Write-Host "Processo de migrations concluído."