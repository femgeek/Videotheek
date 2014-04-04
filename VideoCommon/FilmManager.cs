using System;
using System.Collections.ObjectModel;
using System.Data;

namespace VideoCommon
{
    public class FilmManager
    {
        public ObservableCollection<Film> GetAllFilms()
        {
            ObservableCollection<Film> films = new ObservableCollection<Film>();
            var manager = new VideoDbManager();
            using (var conVideo = manager.GetConnection())
            {
                using (var comGetAll = conVideo.CreateCommand())
                {
                    comGetAll.CommandType = CommandType.Text;
                    comGetAll.CommandText = "Select * FROM Films ORDER BY Titel";
                    conVideo.Open();

                    using (var rdrFilms = comGetAll.ExecuteReader())
                    {
                        Int32 BandNrPos = rdrFilms.GetOrdinal("BandNr");
                        Int32 TitelPos = rdrFilms.GetOrdinal("Titel");
                        Int32 GenreNrPos = rdrFilms.GetOrdinal("GenreNr");
                        Int32 InVoorraadPos = rdrFilms.GetOrdinal("InVoorraad");
                        Int32 UitVoorraadPos = rdrFilms.GetOrdinal("UitVoorraad");
                        Int32 PrijsPos = rdrFilms.GetOrdinal("Prijs");
                        Int32 TotaalVerhuurdPos = rdrFilms.GetOrdinal("TotaalVerhuurd");

                        while (rdrFilms.Read())
                        {
                            films.Add(new Film(
                                rdrFilms.GetInt32(BandNrPos),
                                rdrFilms.GetString(TitelPos),
                                rdrFilms.GetInt32(GenreNrPos),
                                rdrFilms.GetInt32(InVoorraadPos),
                                rdrFilms.GetInt32(UitVoorraadPos),
                                rdrFilms.GetDecimal(PrijsPos),
                                rdrFilms.GetInt32(TotaalVerhuurdPos)
                                ));
                        }
                    }
                }
            }
            return films;
        }

        public Int32 SchrijfToevoeging(Film eenFilm)
        {
            var manager = new VideoDbManager();
            using (var conVideo = manager.GetConnection())
            {
                using (var comInsert = conVideo.CreateCommand())
                {
                    comInsert.CommandType = CommandType.Text;
                    comInsert.CommandText = "INSERT INTO Films (Titel, GenreNr, InVoorraad, UitVoorraad, Prijs, TotaalVerhuurd) VALUES (@Titel, @GenreNr, @InVoorraad, @UitVoorraad, @Prijs, @TotaalVerhuurd); select @@identity";

                    var parTitel = comInsert.CreateParameter();
                    parTitel.ParameterName = "@Titel";
                    parTitel.Value = eenFilm.Titel;
                    comInsert.Parameters.Add(parTitel);
                    var parGenreNr = comInsert.CreateParameter();
                    parGenreNr.ParameterName = "@GenreNr";
                    parGenreNr.Value = eenFilm.GenreNr;
                    comInsert.Parameters.Add(parGenreNr);
                    var parInVoorraad = comInsert.CreateParameter();
                    parInVoorraad.ParameterName = "@InVoorraad";
                    parInVoorraad.Value = eenFilm.InVoorraad;
                    comInsert.Parameters.Add(parInVoorraad);
                    var parUitVoorraad = comInsert.CreateParameter();
                    parUitVoorraad.ParameterName = "@UitVoorraad";
                    parUitVoorraad.Value = eenFilm.UitVoorraad;
                    comInsert.Parameters.Add(parUitVoorraad);
                    var parPrijs = comInsert.CreateParameter();
                    parPrijs.ParameterName = "@Prijs";
                    parPrijs.Value = eenFilm.Prijs;
                    comInsert.Parameters.Add(parPrijs);
                    var parTotaalVerhuurd = comInsert.CreateParameter();
                    parTotaalVerhuurd.ParameterName = "@TotaalVerhuurd";
                    parTotaalVerhuurd.Value = eenFilm.TotaalVerhuurd;
                    comInsert.Parameters.Add(parTotaalVerhuurd);

                    conVideo.Open();
                    Int32 BandNr = Convert.ToInt32(comInsert.ExecuteScalar());
                    if (BandNr == 0)
                        throw new Exception("Kon film niet toevoegen.");
                    else
                        return (Int32)BandNr;
                }
            }
        }

        public void SchrijfVerwijdering(Int32 eenBandNr)
        {
            var manager = new VideoDbManager();
            using (var conVideo = manager.GetConnection())
            {
                using (var comDelete = conVideo.CreateCommand())
                {
                    comDelete.CommandType = CommandType.Text;
                    comDelete.CommandText = "DELETE FROM Films WHERE BandNr=@BandNr";

                    var parBandNr = comDelete.CreateParameter();
                    parBandNr.ParameterName = "@BandNr";
                    parBandNr.Value = eenBandNr;
                    comDelete.Parameters.Add(parBandNr);                    

                    conVideo.Open();
                    var records = comDelete.ExecuteNonQuery();
                    if (records == 0)
                        throw new Exception("Kon film niet verwijderen.");                    
                }
            }
        }

        public void SchrijfWijziging(Film eenFilm)
        {
            var manager = new VideoDbManager();
            using (var conVideo = manager.GetConnection())
            {
                using (var comUpdate = conVideo.CreateCommand())
                {
                    comUpdate.CommandType = CommandType.Text;
                    comUpdate.CommandText = "UPDATE Films SET Titel=@Titel, GenreNr=@GenreNr, InVoorraad=@InVoorraad, UitVoorraad=@UitVoorraad, Prijs=@Prijs, TotaalVerhuurd=@TotaalVerhuurd WHERE BandNr=@BandNr";

                    var parBandNr = comUpdate.CreateParameter();
                    parBandNr.ParameterName = "@BandNr";
                    parBandNr.Value = eenFilm.BandNr;
                    comUpdate.Parameters.Add(parBandNr);
                    var parTitel = comUpdate.CreateParameter();
                    parTitel.ParameterName = "@Titel";
                    parTitel.Value = eenFilm.Titel;
                    comUpdate.Parameters.Add(parTitel);
                    var parGenreNr = comUpdate.CreateParameter();
                    parGenreNr.ParameterName = "@GenreNr";
                    parGenreNr.Value = eenFilm.GenreNr;
                    comUpdate.Parameters.Add(parGenreNr);
                    var parInVoorraad = comUpdate.CreateParameter();
                    parInVoorraad.ParameterName = "@InVoorraad";
                    parInVoorraad.Value = eenFilm.InVoorraad;
                    comUpdate.Parameters.Add(parInVoorraad);
                    var parUitVoorraad = comUpdate.CreateParameter();
                    parUitVoorraad.ParameterName = "@UitVoorraad";
                    parUitVoorraad.Value = eenFilm.UitVoorraad;
                    comUpdate.Parameters.Add(parUitVoorraad);
                    var parPrijs = comUpdate.CreateParameter();
                    parPrijs.ParameterName = "@Prijs";
                    parPrijs.Value = eenFilm.Prijs;
                    comUpdate.Parameters.Add(parPrijs);
                    var parTotaalVerhuurd = comUpdate.CreateParameter();
                    parTotaalVerhuurd.ParameterName = "@TotaalVerhuurd";
                    parTotaalVerhuurd.Value = eenFilm.TotaalVerhuurd;
                    comUpdate.Parameters.Add(parTotaalVerhuurd);

                    conVideo.Open();
                    var records = comUpdate.ExecuteNonQuery();
                    if (records == 0)
                        throw new Exception("Kon film niet wijzigen.");                    
                }
            }
        }
    }
}
