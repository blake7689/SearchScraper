using System.ComponentModel.DataAnnotations;

namespace SearchScraper.Models
{
    public class SearchResult
    {
        public int Id { get; set; }

        [Required]
        public string SearchPhrase { get; set; }

        [Required]
        public string SearchUrl { get; set; }

        public int Position { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? InActive { get; set; } = null;
    }
}
