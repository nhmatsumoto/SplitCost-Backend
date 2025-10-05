namespace SplitCost.Domain.Common
{
    /// <summary>
    /// Representa uma entidade base que pertence a um locatário (tenant) específico,
    /// identificada pelo <see cref="ResidenceId"/>. 
    /// Todas as entidades que herdam desta classe são associadas a uma residência,
    /// permitindo o isolamento de dados entre diferentes residências (multi-tenancy).
    /// </summary>
    public abstract class BaseTenantEntity : BaseEntity
    {
        /// <summary>
        /// Identificador único da residência (tenant) à qual esta entidade pertence.
        /// </summary>
        public Guid ResidenceId { get; set; }
    }
}
