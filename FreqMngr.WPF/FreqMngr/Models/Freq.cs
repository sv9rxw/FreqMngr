using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FreqMngr.Models
{    
    public class Freq : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private NumberFormatInfo NumberFormat { get; set; } = null;        

        private String _Name;
        public String Name
        {
            get; set;
        }

        private double FrequencyInKHz { get; set; }

        public String Frequency
        {
            get; set;
        }

        private double BandwidthInKHz { get; set; }
        public String Bandwidth
        {
            get; set;
        }
        
        public String Modulation
        {
            get; set;
        }
        
        public String ModulationType
        {
            get; set;
        }

        public String Protocol
        {
            get; set;
        }

        public String Country
        {
            get; set;
        }

        public String User
        {
            get; set;
        }
        

        public String Coordinates
        {
            get; set;
        }

        public String Description
        {
            get; set;
        }

        public String URLs
        {
            get; set;
        }

        private String QSLStr
        {
            get; set;
        }

        public bool QSL
        {
            get; set;
        }




        public Freq()
        {


            this.NumberFormat = new NumberFormatInfo();
            this.NumberFormat.NumberDecimalSeparator = ".";
            this.NumberFormat.NumberGroupSeparator = ",";
        }

    }
}
