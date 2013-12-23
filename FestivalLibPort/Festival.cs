using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FestivalLibPort
{
    public class Festival
    {
        #region Field en properties

        private int _ID;

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }


        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }


        private DateTime _endDatde;

        public DateTime EndDate
        {
            get { return _endDatde; }
            set { _endDatde = value; }
        }

        private string _omschrijving;

        public string Omschrijving
        {
            get { return _omschrijving; }
            set { _omschrijving = value; }
        }


        #endregion 
    }
}
