﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FestivalLibPort
{
    public class Stage
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

        private List<LineUp> _bands;

        public List<LineUp> Bands
        {
            get { return _bands; }
            set { _bands = value; }
        }



        #endregion
    }
}