using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using FestivalLib.model;
using Ypsilon2.converter;
using System.Collections.Generic;

namespace FestivalLib.converter
{
    [ValueConversion(typeof(Observablecollection<Genre>), typeof(string))]
    public class ObservablecollectionToStringConverter : IValueConverter
    {
        //deze conversie wordt niet uitgevoerd
        public object Convert(object value, Type targetType, object parameter, Genre genre)
        {
            ObservableCollection<Genre> lstGenres = (ObservableCollection<Genre>)value;

            var sb = new StringBuilder();
            foreach (var item in lstGenres)
            {
                sb.AppendLine(item.Name);
                sb.AppendLine(";");                
            }

            return sb.ToString();
        }

        //deze conversie wordt wel uitgevoerd
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sResult = Conversie(value);
            return sResult;
        }

        public static string Conversie(object value)
        {
            ObservableCollection<Genre> lstGenres = (ObservableCollection<Genre>)value;

            var sb = new StringBuilder();
            if (lstGenres != null)
            {
                foreach (var item in lstGenres)
                {
                    sb.AppendLine(item.Name);
                    sb.AppendLine("; ");
                    // te vervangen string > \r\n -> dit is normaal een enter, moet vervangen worden door lege string
                    sb.Replace("\r\n", "");
                }
            }
            else
            {
                sb.AppendLine("");
            }
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //throw new NotImplementedException();      
            
            List<Genre> list = new List<Genre>();

            string[] arrSplits = value.ToString().Split(';');
            foreach (string item in arrSplits)
            {
                Genre genre = new Genre();
                genre.Name = value.ToString();
                list.Add(genre);
            }                        
            return list;
        }
    }
}
