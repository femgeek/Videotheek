using System;


namespace VideoCommon
{
    public class Genre
    {
        public Int32 GenreNr { get; set; }
        public String GenreNaam { get; set; }

        public Genre(int parGenreNr, string parGenreNaam)
        {
            GenreNr = parGenreNr;
            GenreNaam = parGenreNaam;
        }
    }
}
