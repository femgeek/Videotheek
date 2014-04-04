using System;
using System.Collections.ObjectModel;
using System.Data;

namespace VideoCommon
{
    public class GenreManager
    {
        public ObservableCollection<Genre> GetAllGenres()
        {
            ObservableCollection<Genre> genres = new ObservableCollection<Genre>();
            var manager = new VideoDbManager();

            using (var conVideo = manager.GetConnection())
            {
                using (var comGetAll = conVideo.CreateCommand())
                {
                    comGetAll.CommandType = CommandType.Text;
                    comGetAll.CommandText = "SELECT * FROM Genres ORDER BY Genre";
                    conVideo.Open();

                    using (var rdrGenres = comGetAll.ExecuteReader())
                    {
                        Int32 GenreNrPos = rdrGenres.GetOrdinal("GenreNr");
                        Int32 GenreNaamPos = rdrGenres.GetOrdinal("Genre");

                        while (rdrGenres.Read())
                        {
                            genres.Add(new Genre(
                                rdrGenres.GetInt32(GenreNrPos),
                                rdrGenres.GetString(GenreNaamPos)
                                ));
                        }
                    }
                }
            }
            return genres;
        }
    }
}
