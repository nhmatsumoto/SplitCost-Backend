using System.Text.Json.Serialization;

namespace SplitCost.Application.UseCases.Dtos
{
    public class UpdateUserSettingsDto
    {
        [JsonPropertyName("currency")]
        public string? Currency { get; set; }
    }
}
