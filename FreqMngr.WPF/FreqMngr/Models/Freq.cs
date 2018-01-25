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
            get
            {
                return _Name;
            }
            set
            {
                if (value == _Name)
                    return;
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private double FrequencyInKHz { get; set; }

        private String _Frequency = String.Empty;
        public String Frequency
        {
            get
            {
                return _Frequency;
            }
            set
            {
                if (value == _Frequency)
                    return;
                _Frequency = value;
                OnPropertyChanged(nameof(Frequency));
            }
        }

        private double BandwidthInKHz { get; set; }

        private String _Bandwidth;
        public String Bandwidth
        {
            get
            {
                return _Bandwidth;
            }
            set
            {
                if (value == _Bandwidth)
                    return;
                _Bandwidth = value;
                OnPropertyChanged(nameof(Bandwidth));
            }
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
