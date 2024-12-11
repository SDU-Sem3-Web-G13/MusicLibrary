using Models.Enums;

namespace Models
{
    public class AlbumModel
    {
        public int? Id { get; set; }
        public int? OwnerId { get; set; }
        public byte[]? CoverImage { get; set; }
        public string AlbumName { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Artist { get; set; }
        public string AlbumType { get; set; }
        public string Description { get; set; }
        public string[] Tracks { get; set; } 
        public bool IsFavourite { get; set; }

        public AlbumModel(
            string albumName,
            DateTime releaseDate,
            string artist,
            string albumType,
            string description,
            string[] tracks,
            bool isFavourite
        ) {
            this.AlbumName = albumName;
            this.ReleaseDate = releaseDate;
            this.Artist = artist;
            this.AlbumType = albumType;
            this.Description = description;
            this.Tracks = tracks;
            this.IsFavourite = isFavourite;
        }

        public string GetCoverImageBase64()
        {
            return Convert.ToBase64String(CoverImage ?? new byte[1]);
        }
    }
}
