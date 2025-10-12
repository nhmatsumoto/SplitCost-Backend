using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SplitCost.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Converte automaticamente todos os nomes de tabelas, esquemas e colunas
        /// para letras minúsculas — ideal para uso com PostgreSQL.
        /// </summary>
        /// <param name="modelBuilder">Instância do ModelBuilder usada no OnModelCreating.</param>
        public static void UseLowerCaseNamingConvention(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Tabela
                var tableName = entity.GetTableName();
                if (!string.IsNullOrEmpty(tableName))
                    entity.SetTableName(tableName.ToLower());

                // Schema (se existir)
                var schema = entity.GetSchema();
                if (!string.IsNullOrEmpty(schema))
                    entity.SetSchema(schema.ToLower());

                // Colunas
                foreach (var property in entity.GetProperties())
                {
                    var columnName = property.GetColumnName(StoreObjectIdentifier.Table(tableName, schema));
                    if (!string.IsNullOrEmpty(columnName))
                        property.SetColumnName(columnName.ToLower());
                }

                // Chaves e constraints
                foreach (var key in entity.GetKeys())
                {
                    var keyName = key.GetName();
                    if (!string.IsNullOrEmpty(keyName))
                        key.SetName(keyName.ToLower());
                }

                foreach (var fk in entity.GetForeignKeys())
                {
                    var fkName = fk.GetConstraintName();
                    if (!string.IsNullOrEmpty(fkName))
                        fk.SetConstraintName(fkName.ToLower());
                }

                foreach (var index in entity.GetIndexes())
                {
                    var indexName = index.GetDatabaseName();
                    if (!string.IsNullOrEmpty(indexName))
                        index.SetDatabaseName(indexName.ToLower());
                }
            }
        }
    }
}
