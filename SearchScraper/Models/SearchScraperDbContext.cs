using Microsoft.EntityFrameworkCore;

namespace SearchScraper.Models
{
    public class SearchScraperDbContext : DbContext
    {
        public SearchScraperDbContext(DbContextOptions<SearchScraperDbContext>
            options) : base(options)
        {
        }

        public DbSet<SearchResult> SearchResults { get; set; }
    }
}
