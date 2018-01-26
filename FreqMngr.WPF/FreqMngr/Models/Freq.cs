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
                this._IsDirty = true;
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private NumberFormatInfo NumberFormat { get; set; } = null;

        private Group _Parent = null;
        public Group Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                _Parent = value;
            }
        }

        private bool _IsDirty = false;
        public bool IsDirty
        {
            get
            {
                return _IsDirty;
            }
        }

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

        private double _Frequency;
        public double Frequency
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

        private double _Bandwidth;
        public double Bandwidth
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

        private String _Modulation = null;
        public String Modulation
        {
            get
            {
                return _Modulation;
            }
            set
            {
                if (value == _Modulation)
                    return;

                _Modulation = value;
                OnPropertyChanged(nameof(Modulation));
            }
        }

        private String _ModulationType = null;
        public String ModulationType
        {
            get
            {
                return _ModulationType;
            }
            set
            {
                if (value == _ModulationType)
                    return;

                _ModulationType = value;
                OnPropertyChanged(nameof(ModulationType));
            }
        }

        private String _Protocol = null;
        public String Protocol
        {
            get
            {
                return _Protocol;
            }
            set
            {
                if (value == _Protocol)
                    return;

                _Protocol = value;
                OnPropertyChanged(nameof(Protocol));
            }
        }

        private String _Country = null;
        public String Country
        {
            get
            {
                return _Country;
            }
            set
            {
                if (value == _Country)
                    return;

                _Country = value;                
                OnPropertyChanged(nameof(Country));
            }
        }

        private String _User = null;
        public String User
        {
            get
            {
                return _User;
            }
            set
            {
                if (value == _User)
                    return;

                _User = value;
                OnPropertyChanged(nameof(User));
            }
        }


        private String _Coordinates = null;
        public String Coordinates
        {
            get
            {
                return _Coordinates;
            }
            set
            {
                if (value == _Coordinates)
                    return;

                _Coordinates = value;
                OnPropertyChanged(nameof(Coordinates));
            }
        }

        private String _Description;
        public String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (value == _Description)
                    return;

                _Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private String _References = null;
        public String References
        {
            get
            {
                return _References;
            }
            set
            {
                if (value == _References)
                    return;

                _References = value;
                OnPropertyChanged(nameof(References));
            }    
            
        }

        private String _QSL = null;
        public String QSL
        {
            get
            {
                return _QSL;
            }
            set
            {
                if (value == _QSL)
                    return;

                _QSL = value;
                OnPropertyChanged(nameof(QSL));
            }
        }




        public Freq()
        {
            this.NumberFormat = new NumberFormatInfo();
            this.NumberFormat.NumberDecimalSeparator = ".";
            this.NumberFormat.NumberGroupSeparator = ",";
        }

    }
}
