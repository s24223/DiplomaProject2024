using Application.Database.Models;

namespace Application.VerticalSlice.UrlPart.DTOs
{
    public class CreateUrlDto
    {
        public UrlTypeDto UrlType { get; set; } = null!;
        public DateTime DateTime = DateTime.Now;
        public string Url { get; set; } = null!;
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
