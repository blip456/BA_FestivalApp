using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ypsilon2.Model;

namespace Ypsilon2.model
{
    public class Genre
    {
        #region field en properties

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        #endregion

        #region SQL
        public static ObservableCollection<Genre> GetGenresByBandID(int id)
        {
            string query = "SELECT * FROM genre INNER JOIN bandgenre on bandgenre.genre_id = genre.genre_id WHERE band_id = " + id + ";";

            ObservableCollection<Genre> gevondenGenres = new ObservableCollection<Genre>();            
            DbDataReader reader = Database.GetData(query);
            while (reader.Read())
            {
                Genre genre = new Genre();
                genre.Name = Convert.ToString(reader["genre_name"]);
                genre.ID = Convert.ToString(reader["genre_id"]);
                gevondenGenres.Add(genre);
            }
            return gevondenGenres;
        }
        #endregion

    }
}
