using System.Text.Json.Serialization;

namespace Application.Shared.DTOs.Response
{
    public class Response
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required EnumResponseStatus Status { get; set; }
        public required string Message { get; set; } = null!;
    }
}
