# SearchScraper


Prerequisites
- Create 'SearchScraperDB' in SQLEXPRESS server using SSMS.  <br />
  Connection string (appsettings.json) :
```
"ConnectionStrings": {
  "SearchScraperDbContextConnection": "Server=.\\SQLEXPRESS;Database=SearchScraperDB;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True"
}
```
- Create dbo.SearchResults in SSMS:
```
USE [SearchScraperDB]
GO

/****** Object:  Table [dbo].[SearchResults]    Script Date: 03/04/2025 6:49:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SearchResults](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SearchPhrase] [nvarchar](max) NOT NULL,
	[SearchUrl] [nvarchar](max) NOT NULL,
	[Position] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[InActive] [datetime2](7) NULL,
 CONSTRAINT [PK_SearchResults] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```
Or run the following commnd in Package Manager Console :
```
update-database
```



Search Scrape Explanation
- When scraping, a captcha will appear. Solve the captcha. The scraper will check every 5 seconds for captcha completion. Window will automatically close when data has been collected.
