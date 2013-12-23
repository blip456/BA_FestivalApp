using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FestivalLibPort
{
    public class LineUp
    {
        #region Prop en field

        private string _id;

        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private DateTime _from;

        public DateTime From
        {
            get { return _from; }
            set { _from = value; }
        }

        private DateTime _until;

        public DateTime Until
        {
            get { return _until; }
            set { _until = value; }
        }

        private Stage _stage;

        public Stage Stage
        {
            get { return _stage; }
            set { _stage = value; }
        }

        private Band _band;

        public Band Band
        {
            get { return _band; }
            set { _band = value; }
        }

        private double _timespan;

        public double TimeSpan
        {
            get
            {
                _timespan = calculateTimespan(this.From, this.Until);
                return _timespan;
            }
            set { _timespan = value; }
        }

        #endregion

        public static double calculateTimespan(DateTime dtFrom, DateTime dtUntil)
        {

            string sFrom = dtFrom.ToString("HH:mm");
            string sUntil = dtUntil.ToString("HH:mm");

            string[] arrsSplitFrom = sFrom.Split(':');
            double dFromHour = Convert.ToDouble(arrsSplitFrom[0]);
            double dFromMinute = Convert.ToDouble(arrsSplitFrom[1]);
            double dFrom = dFromHour + (dFromMinute / 60);

            string[] arrsSplitUntil = sUntil.Split(':');
            double dUntilHour = Convert.ToDouble(arrsSplitUntil[0]);
            double dUntilMinute = Convert.ToDouble(arrsSplitUntil[1]);
            double dUntil = dUntilHour + (dUntilMinute / 60);

            double dTimespan = (dUntil - dFrom) * 100;

            return dTimespan;
        }

    }
}
