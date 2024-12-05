using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace katio.Data.Models.Dto
{
    public class BookResponse : BaseEntity<int>
    {
        public string Name { get; set; } = string.Empty;
        public string ISBN10 { get; set; } = string.Empty;
        public string ISBN13 { get; set; } = string.Empty;
        public DateOnly Published { get; set; }
        public string Edition { get; set; } = string.Empty;
        public string DeweyIndex { get; set; } = string.Empty;
        public string BookCover { get; set; } = string.Empty;
        public string? PdfPath { get; set; } = null;
        public string Description { get; set; } = string.Empty;

        // Relaciones
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public virtual Author? Author { get; set; }
    }

    public class BookWithPdfResponse
    {
        public BookResponse Book { get; set; } = new BookResponse();
        public byte[] PdfFile { get; set; } = Array.Empty<byte>();
    }

}
