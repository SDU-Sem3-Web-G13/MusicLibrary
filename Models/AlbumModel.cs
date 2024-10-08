﻿using RazorMusic.Models.Enums;

namespace RazorMusic.Models
{
    public class AlbumModel
    {
        public string AlbumName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Artist { get; set; }
        public string AlbumType { get; set; }
        public string Description { get; set; }
        public string[] Tracks { get; set; } 

        public AlbumModel(
            string albumName,
            DateTime releaseDate,
            string artist,
            string albumType,
            string description,
            string[] tracks
        ) {
            this.AlbumName = albumName;
            this.ReleaseDate = releaseDate;
            this.Artist = artist;
            this.AlbumType = albumType;
            this.Description = description;
            this.Tracks = tracks;
        }
    }
}
