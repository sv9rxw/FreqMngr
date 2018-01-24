using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
















namespace FreqMngr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
      
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Old Stuff to remember

        //public String[] Modulations { get; set; } = new string[] { Modulation.CW,
        //    Modulation.DSB,
        //    Modulation.USB,
        //    Modulation.LSB,
        //    Modulation.AM,
        //    Modulation.FM,
        //    Modulation.FSK,
        //    Modulation.PSK,
        //    Modulation.APSK,
        //    Modulation.MSK,
        //    Modulation.TCM,
        //    Modulation.PPM,
        //    Modulation.CPM,
        //    Modulation.SCFDMA,
        //    Modulation.WDM,
        //    Modulation.Unknown
        //};

        //private void TxtFrequency_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
        //    e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        //}

        //private void TxtBandwidth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
        //    e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        //}
        #endregion

    }
}
