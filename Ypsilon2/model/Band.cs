using Ypsilon2.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ypsilon2.Model;
using Microsoft.Win32;
using System.IO;

namespace Ypsilon2.model
{
    public class Band
    {
        #region field en prop

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

        private byte[] _picture;

        public byte[] Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        private string _descr;

        public string Descr
        {
            get { return _descr; }
            set { _descr = value; }
        }

        private string _twitter;

        public string Twitter
        {
            get { return _twitter; }
            set { _twitter = value; }
        }

        private string _facebook;

        public string Facebook
        {
            get { return _facebook; }
            set { _facebook = value; }
        }

        //private Observable<Genre> _genres;

        //public Observable<Genre> Genres
        //{
        //    get { return _genres; }
        //    set { _genres = value; }
        //}



        #endregion

        #region SQL
        private static Band CreateBand(DbDataReader reader)
        {

            Band band = new Band();
            band.ID = Convert.ToString(reader["band_id"]);
            band.Name = Convert.ToString(reader["band_name"]);
            band.Picture = (byte[])reader["band_picture"];
            band.Descr = Convert.ToString(reader["band_description"]);
            band.Twitter = Convert.ToString(reader["band_twitter"]);
            band.Facebook = Convert.ToString(reader["band_facebook"]);
            return band;
        }

        public static ObservableCollection<Band> GetBands()
        {
            ObservableCollection<Band> lstBands = new ObservableCollection<Band>();
            DbDataReader reader = Database.GetData("SELECT * FROM band");
            while (reader.Read())
            {
                // Band band = CreateBand(reader);
                lstBands.Add(CreateBand(reader));
            }
            return lstBands;
        }

        public static ObservableCollection<Band> GetBandsByString(ObservableCollection<Band> lst, string search)
        {
            ObservableCollection<Band> lstGevondenBands = new ObservableCollection<Band>();
            foreach (Band band in lst)
            {
                if (band.Name.ToUpper().Contains(search.ToUpper()) || band.Descr.ToUpper().Contains(search.ToUpper()) || band.Facebook.ToUpper().Contains(search.ToUpper()) || band.Twitter.ToUpper().Contains(search.ToUpper()))
                {
                    lstGevondenBands.Add(band);
                }
            }

            return lstGevondenBands;
        }

        public static void DeleteBand(int id)
        {
            string sql = "DELETE FROM band WHERE band_id = @ID;";

            DbParameter parID = Database.AddParameter("@ID", id);

            int i = Database.ModifyData(sql, parID);
            Console.WriteLine(i + " row(s) are deleted");
        }

        public static void DeleteBandFromLineUp(int lineupID)
        {
            string sql = "DELETE FROM lineup WHERE lineup_id = @lineupID;";
            DbParameter parLineUpID = Database.AddParameter("@lineupID", lineupID);
            int i = Database.ModifyData(sql, parLineUpID);
            Console.WriteLine(i + "row deleted");
        }

        public static Band GetBandByID(ObservableCollection<Band> lst, int id)
        {
            Band gevondenBand = new Band();
            //foreach (Band band in lst)
            //{
            //    if (band.ID == Convert.ToString(id))
            //    {
            //        gevondenBand = band;
            //    }
            //}
            gevondenBand = lst.Single(i => i.ID == Convert.ToString(id));

            return gevondenBand;
        }

        public static void AddBand(Band band)
        {
            //string sql = "INSERT INTO band(band_name, band_picture, band_description, band_twitter, band_facebook) VALUES (@name,@picture, @description, @twitter, @twitter, @facebook);";
            string sql = "INSERT INTO band(band_name, band_picture, band_description, band_twitter, band_facebook) VALUES (@name, @picture, @description, @twitter, @facebook);";

            DbParameter par1 = Database.AddParameter("@name", band.Name);
            DbParameter par2 = Database.AddParameter("@picture", band.Picture);
            DbParameter par3 = Database.AddParameter("@description", band.Descr);
            DbParameter par4 = Database.AddParameter("@twitter", band.Twitter);
            DbParameter par5 = Database.AddParameter("@facebook", band.Facebook);

            int i = Database.ModifyData(sql, par1, par2, par3, par4, par5);
            Console.WriteLine(i + " row(s) are affected");
        }

        public static void EditBand(Band band)
        {
            string sql = "UPDATE band SET band_name=@name, band_picture=@picture, band_description=@description, band_twitter=@twitter, band_facebook=@facebook WHERE band_id=@ID;";

            DbParameter par1 = Database.AddParameter("@name", band.Name);
            DbParameter par2 = Database.AddParameter("@picture", band.Picture);
            DbParameter par3 = Database.AddParameter("@description", band.Descr);
            DbParameter par4 = Database.AddParameter("@twitter", band.Twitter);
            DbParameter par5 = Database.AddParameter("@facebook", band.Facebook);

            DbParameter parID = Database.AddParameter("@ID", Convert.ToInt16(band.ID));

            int i = Database.ModifyData(sql, par1, par2, par3, par4, par5, parID);
            Console.WriteLine(i + " row(s) are affected");
        }
        #endregion


    }
}
