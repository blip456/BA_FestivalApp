using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FestivalLibPort
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

        private List<Genre> _genres;

        public List<Genre> Genres
        {
            get { return _genres; }
            set { _genres = value; }
        }



        #endregion
    }
}
