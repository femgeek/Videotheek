using System;

namespace VideoCommon
{
    public class Film
    {
        public Int32 BandNr { get; set; }
        public String Titel { get; set; }
        public Int32 GenreNr { get; set; }
        public Int32 InVoorraad { get; set; }
        public Int32 UitVoorraad { get; set; }
        public Decimal Prijs { get; set; }
        public Int32 TotaalVerhuurd { get; set; }

        public Film(int parBandNr, string parTitel, int parGenreNr, int parInVoorraad, int parUitVoorraad, decimal parPrijs, int parTotaalVerhuurd)
        {
            BandNr = parBandNr;
            Titel = parTitel;
            GenreNr = parGenreNr;
            InVoorraad = parInVoorraad;
            UitVoorraad = parUitVoorraad;
            Prijs = parPrijs;
            TotaalVerhuurd = parTotaalVerhuurd;
        }

        public Film()
        { }
    }
}
