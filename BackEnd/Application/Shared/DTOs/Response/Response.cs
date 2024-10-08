using System.Text.Json.Serialization;

namespace Application.Shared.DTOs.Response
{
    /// <summary>
    /// Default Success
    /// </summary>
    public class Response
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EnumResponseStatus Status { get; set; } = EnumResponseStatus.Success;
        public string Message { get; set; } = Messages.ResponseSuccess;
    }
}
