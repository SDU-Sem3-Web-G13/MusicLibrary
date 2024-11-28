using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DataAccess;

namespace RazorMusic.Pages
{
    public class StatisticsModel : PageModel
    {
        public readonly ILogger<StatisticsModel> _logger;
        private readonly AlbumRepository _albumRepository;

        public StatisticsModel(ILogger<StatisticsModel> logger, AlbumRepository albumRepository)
        {
            _logger = logger;
            _albumRepository = albumRepository;
        }

        public int NumberOfAlbums { get; set; }
        public int NumberOfSongs { get; set; }
        public int NumberOfArtists { get; set; }
        public int NumberOfGenres { get; set; }

        public void OnGet()
        {
            var statistics = _albumRepository.GetStatistics();
            NumberOfAlbums = statistics.totalAlbums;
            NumberOfArtists = statistics.totalArtists;
            NumberOfSongs = statistics.totalTracks;
            NumberOfGenres = statistics.totalGenres;
        }
    }
}
