using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace katio.Data.Models.Dto
{
    public class AudioBookResponse : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string ISBN10 { get; set; } = string.Empty;
        public string ISBN13 { get; set; } = string.Empty;
        public DateOnly Published { get; set; } = new DateOnly();
        public string Edition { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int LenghtInSeconds { get; set; } = 0;
        public string FrontPage { get; set; } = string.Empty;
        public string? AudioPath { get; set; } = null;
        public IFormFile AudioFile { get; set; }

        // Relaciones
        [ForeignKey("Narrator")]
        public int NarratorId { get; set; }
        public virtual Narrator? Narrator { get; set; }
    }

        public class AudioBookAudioResponse
    {
        public AudioBookResponse AudioBook { get; set; } = new AudioBookResponse();
        public byte[] AudioFile { get; set; } = Array.Empty<byte>();
    }
}
