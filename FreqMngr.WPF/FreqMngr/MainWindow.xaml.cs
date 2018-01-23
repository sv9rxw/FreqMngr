using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using FreqMngr.ViewModels;
using System.Xml.Linq;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Collections.ObjectModel;

using Xceed.Wpf;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.DataGrid;
using System.Globalization;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;

namespace FreqMngr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String DbFilePath { get; set; } = "FreqDB.xml";
        private FreqGroup Root { get; set; } = null;        
        private XmlDocument Doc { get; set; } = new XmlDocument();
        
        private bool PnlEditModeAddMew = false;
        private List<FreqGroup> AvailableGroups = new List<FreqGroup>();

        List<Freq> FreqsClipboard = null;


        public String[] Modulations { get; set; } = new string[] { Modulation.CW,
            Modulation.DSB,
            Modulation.USB,
            Modulation.LSB,
            Modulation.AM,
            Modulation.FM,
            Modulation.FSK,
            Modulation.PSK,
            Modulation.APSK,
            Modulation.MSK,
            Modulation.TCM,
            Modulation.PPM,
            Modulation.CPM,
            Modulation.SCFDMA,
            Modulation.WDM,
            Modulation.Unknown
        };



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            Doc.Save(DbFilePath);
        }

        #region PnlEdit

        private void PnlEdit_Show()
        {
            this.ClmFreqs.Width = new GridLength(2.0, GridUnitType.Star);
            this.ClmEdit.Width = new GridLength(3.0, GridUnitType.Star);
        }

        private void PnlEdit_Hide()
        {
            this.ClmFreqs.Width = new GridLength(3.0, GridUnitType.Star);
            this.ClmEdit.Width = new GridLength(0.0, GridUnitType.Star);
        }

        private void TxtFrequency_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void TxtBandwidth_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            PnlEdit_Hide();
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            if (this.PnlEditModeAddMew == true)
            {
                if (this.CmbGroup.SelectedItem == null)
                {
                    System.Windows.MessageBox.Show("Please, select a Group");
                    return;
                }

                if (String.IsNullOrWhiteSpace(this.TxtName.Text) == true)
                {
                    System.Windows.MessageBox.Show("Please, enter a valid name");
                    return;
                }

                if (String.IsNullOrWhiteSpace(this.TxtFrequency.Text) == true)
                {
                    System.Windows.MessageBox.Show("Please, enter a valid frequency");
                    return;
                }
                double freqDouble;
                try
                {
                    freqDouble = double.Parse(this.TxtFrequency.Text);
                }
                catch (Exception expt)
                {
                    System.Windows.MessageBox.Show("Please, enter a valid frequency: " + expt.Message);
                    return;
                }

                if (String.IsNullOrWhiteSpace(this.TxtBandwidth.Text) == false)
                {
                    double bw;
                    try
                    {
                        bw = double.Parse(this.TxtBandwidth.Text);
                    }
                    catch (Exception expt)
                    {
                        System.Windows.MessageBox.Show("Please, enter a valid bandwidth: " + expt.Message);
                        return;
                    }
                }


                // Construct a new XML node for the new Freq           
                FreqGroup group = this.CmbGroup.SelectedItem as FreqGroup;

                Freq freq = new Freq(Doc,
                    group,
                    this.TxtName.Text,
                    this.TxtFrequency.Text,
                    this.TxtBandwidth.Text,
                    (String)this.CmbModulation.SelectedItem,
                    this.TxtModulationType.Text,
                    this.TxtProtocol.Text,
                    this.TxtCountry.Text,
                    this.TxtUser.Text,
                    this.TxtCoordinates.Text,
                    this.TxtDescription.Text,
                    this.TxtURLs.Text);

                group.AddNewFreq(freq);

                PnlEdit_Hide();

                if (TreeGroups.SelectedItem is TreeViewItem)
                {
                    TreeViewItem item = TreeGroups.SelectedItem as TreeViewItem;
                    this.DataGridFreqs.ItemsSource = ((FreqGroup)item.Tag).AllFreqs;
                }
            }
            else
            {

            }
        }

        #endregion

        #region Loading Methods
        private FreqGroup ParseXML(XmlDocument doc, String xmlFilePath)
        {
            FreqGroup root;
                                                   
            doc.Load(xmlFilePath);

            XmlNodeList roots = doc.ChildNodes;
            if (roots.Count!=2)
            {
                System.Windows.MessageBox.Show("roots.count!=1 " + roots[0].Name);
                return null;
            }

            if (roots[0].Name!="xml")
            {
                System.Windows.MessageBox.Show("Invalid XML file. Does not contain xml node");
                return null;
            }

            if (roots[1].Name!="group")
            {
                System.Windows.MessageBox.Show("Invalid XML file. Does not contain root group node");
                return null;
            }

            XmlNode rootNode = roots[1];

            //Special construction for root node
            root = new FreqGroup(rootNode);
                       
            GetChildren(root);

            Debug.WriteLine("Finished");

            return root;
        }

        private void GetChildren(FreqGroup parent)
        {
            XmlNodeList childrenNodeList = parent.XmlNode.ChildNodes;
            if (childrenNodeList.Count == 0) return;

            foreach(XmlNode node in childrenNodeList)
            {
                if (node.Name == "group")
                {                                        
                    FreqGroup group = new FreqGroup(node);
                    parent.ChildGroups.Add(group);
                    GetChildren(group);
                }
                else if (node.Name=="freq")
                {                                        
                    Freq freq = new Freq(node, parent);
                    parent.Freqs.Add(freq);
                }                                             
            }            
        }

        private void PrintFreqsTree(FreqGroup parent)
        {
            Debug.WriteLine("Group '" + parent.Name + "'");
            if (parent.ChildGroups!=null)
            {
                if (parent.ChildGroups.Count>0)
                {
                    foreach(FreqGroup group in parent.ChildGroups)
                    {
                        Debug.WriteLine(" Group '" + group.Name + "'");
                        PrintFreqsTree(group);
                    }
                }
            }

            if (parent.Freqs != null)
            {
                if (parent.Freqs.Count > 0)
                {
                    foreach (Freq freq in parent.Freqs)
                    {
                        Debug.WriteLine("   Freq: '" + freq.Name + "' frequency='" + freq.Frequency + "'");
                    }
                }
            }

        }

        private TreeViewItem GetTreeViewItems(FreqGroup parent)
        {
            TreeViewItem result = null;

            result = new TreeViewItem();            
            result.Header = parent.Name;
            result.Tag = parent;
            result.IsExpanded = true;
            this.AvailableGroups.Add(parent);
            this.CmbGroup.Items.Add(parent);

            if (parent.ChildGroups!=null)
            {
                foreach(FreqGroup group in parent.ChildGroups)
                {
                    TreeViewItem itemGroup = GetTreeViewItems(group);                    
                    result.Items.Add(itemGroup);                                        
                }
            }
                                    
            return result;
        }

        public MainWindow()
        {
            InitializeComponent();
            
            foreach(String mod in this.Modulations)
            {
                this.CmbModulation.Items.Add(mod);
            }
        }

        private void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {   
            // Parse XML and construct classes and chain the,      
            Root = this.ParseXML(Doc, DbFilePath);


            //PrintFreqsTree(Root);

            // Get all FreqGroups and fill TreeView 
            TreeViewItem rootItem = GetTreeViewItems(Root);
            this.TreeGroups.Items.Add(rootItem);
            rootItem.IsExpanded = true;            
            
            // Set ItemsSource for DataGrid
            this.DataGridFreqs.ItemsSource = Root.AllFreqs;
            this.TxtEntriesCount.Text = Root.AllFreqs.Count.ToString();                     
        }

        #endregion

        #region DataGridFreqs Stuff

        private List<Freq> GetSelectedFreqs()
        {
            List<Freq> selectedFreqs = new List<Freq>();
            var items = this.DataGridFreqs.SelectedItems;
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item is Freq)
                    {
                        Freq freq = item as Freq;
                        selectedFreqs.Add(freq);
                    }
                }
            }
            return selectedFreqs;
        }

        private void DataGridFreqs_SelectionChanged(object sender, DataGridSelectionChangedEventArgs e)
        {
            object selectedItem = this.DataGridFreqs.SelectedItem;
            if (selectedItem!=null)
            {
                if (selectedItem is Freq)
                {
                    Freq freq = selectedItem as Freq;                    
                    //System.Windows.MessageBox.Show("aaa : '" + String.Format(freq.NumberFormat, "{0:n0}", freq.FrequencyInHz) + "'");
                }
            }
        }

        private void DataGridFreqs_TextInput(object sender, TextCompositionEventArgs e)
        {
            //System.Windows.MessageBox.Show("aaaa");
        }

        private void MnuDelete_Click(object sender, RoutedEventArgs e)
        {
            bool removed = false;
            var items = this.DataGridFreqs.SelectedItems;
            if (items!=null)
            {
                foreach(var item in items)
                {
                    if (item is Freq)
                    {
                        Freq freq = item as Freq;
                        FreqGroup parent = freq.GetParent();
                        parent.RemoveFreq(freq);
                        removed = true;
                    }
                }
            }
            if (removed==true)
            {                
                this.DataGridFreqs.ItemsSource = Root.AllFreqs;
            }

        }

        private void MnuNew_Click(object sender, RoutedEventArgs e)
        {
            this.PnlEditModeAddMew = true;

            PnlEdit_Show();

            if (this.TreeGroups.SelectedItem!=null)
            {
                TreeViewItem groupObj = this.TreeGroups.SelectedItem as TreeViewItem;
                if (groupObj.Tag!=null)                                    
                    this.CmbGroup.SelectedItem = (FreqGroup)groupObj.Tag;                
            }
        }

        private void MnuEdit_Click(object sender, RoutedEventArgs e)
        {
            this.PnlEditModeAddMew = false;

            object selectedItem = this.DataGridFreqs.SelectedItem;
            if (selectedItem != null)
            {
                if (selectedItem is Freq)
                {
                    Freq freq = selectedItem as Freq;
                    //System.Windows.MessageBox.Show("aaa : '" + String.Format(freq.NumberFormat, "{0:n0}", freq.FrequencyInHz) + "'");

                    this.ClmFreqs.Width = new GridLength(2.0, GridUnitType.Star);
                    this.ClmEdit.Width = new GridLength(3.0, GridUnitType.Star);
                    this.LblTitle.Text = "Edit frequency";
                    this.TxtName.Text = freq.Name;
                    this.TxtFrequency.Text = freq.Frequency;
                    this.TxtBandwidth.Text = freq.Bandwidth;
                    this.CmbGroup.SelectedItem = freq.GetParent();
                    this.CmbModulation.SelectedItem = freq.Modulation;
                    this.TxtModulationType.Text = freq.ModulationType;
                    this.TxtProtocol.Text = freq.Protocol;
                    this.TxtCountry.Text = freq.Country;
                    this.TxtUser.Text = freq.User;
                    this.TxtCoordinates.Text = freq.Coordinates;
                    this.TxtDescription.Text = freq.Description;
                    this.TxtURLs.Text = freq.URLs;
                }
            }
        }        

        private void MnuCut_Click(object sender, RoutedEventArgs e)
        {
            List<Freq> selectedFreqs = GetSelectedFreqs();

            if (selectedFreqs.Count > 0)
            {
                FreqsClipboard = new List<Freq>();
                foreach (Freq freq in selectedFreqs)
                {
                    //Remove freq to cut from parent group lists
                    FreqGroup parentGroup = freq.GetParent();
                    parentGroup.RemoveFreq(freq);

                    //Unlink freq with previous parent group
                    freq.SetParent(null);

                    //Add to clipboard
                    FreqsClipboard.Add(freq);
                }

                RefreshDataGrid();
            }
        }

        private void MnuCopy_Click(object sender, RoutedEventArgs e)
        {
            List<Freq> selectedFreqs = GetSelectedFreqs();

            if (selectedFreqs.Count > 0)
            {
                FreqsClipboard = new List<Freq>();
                foreach (Freq freq in selectedFreqs)
                {
                    Freq newFreq = freq.Clone();

                    //Unlink cloned Freq with old parent group
                    newFreq.SetParent(null);
                    FreqsClipboard.Add(newFreq);
                }

                RefreshDataGrid();
            }
        }

        private void MnuPaste_Click(object sender, RoutedEventArgs e)
        {
            if (FreqsClipboard != null && FreqsClipboard.Count > 0)
            {
                // Get selected FreqGroup
                FreqGroup newParent = GetSelectedGroup();
                if (newParent == null)
                {
                    System.Windows.MessageBox.Show("No group is selected to paste freqs");
                    return;
                }

                foreach (Freq freq in FreqsClipboard)
                {
                    // Link with new parent group                     
                    freq.SetParent(newParent);
                    newParent.AddNewFreq(freq);
                }

                // Clear clipboard
                FreqsClipboard.Clear();
                FreqsClipboard = null;

                // Refresh DataGrid
                RefreshDataGrid();
            }
        }

        private void MnuMain_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            List<Freq> selectedFreqs = GetSelectedFreqs();
            if (selectedFreqs == null || selectedFreqs.Count == 0)
            {
                this.MnuCut.IsEnabled = false;
                this.MnuCopy.IsEnabled = false;
            }
            else
            {
                this.MnuCut.IsEnabled = true;
                this.MnuCopy.IsEnabled = true;
            }
        }

        private void RefreshDataGrid()
        {
            FreqGroup selectedGroup = GetSelectedGroup();
            if (selectedGroup == null)
                DataGridFreqs.ItemsSource = Root.AllFreqs;
            else
                DataGridFreqs.ItemsSource = selectedGroup.AllFreqs;
        }

        #endregion

        #region TreeGroups Stuff

        private FreqGroup GetSelectedGroup()
        {
            if (this.TreeGroups.SelectedItem != null)
            {
                TreeViewItem groupObj = this.TreeGroups.SelectedItem as TreeViewItem;
                if (groupObj.Tag != null)
                    return (groupObj.Tag as FreqGroup);
            }
            return null;
        }

        private void MnuGroupsNew_Click(object sender, RoutedEventArgs e)
        {
            object selectedItem = TreeGroups.SelectedItem;
            if (selectedItem is TreeViewItem)
            {
                TreeViewItem item = selectedItem as TreeViewItem;
                FreqGroup parent = (FreqGroup)item.Tag;

                InputDialog inputDialog = new InputDialog("New group name: ", String.Empty);
                if (inputDialog.ShowDialog() == true)
                {
                    String groupName = inputDialog.Answer;

                    if (String.IsNullOrWhiteSpace(groupName) == true)
                    {
                        System.Windows.MessageBox.Show("Invalid group name");
                        return;
                    }
                    FreqGroup newGroup = new FreqGroup(Doc, parent, groupName);

                    parent.AddNewGroup(newGroup);

                    this.DataGridFreqs.ItemsSource = parent.AllFreqs;
                    TreeViewItem treeViewItem = GetTreeViewItems(Root);
                    this.TreeGroups.Items.Clear();
                    this.TreeGroups.Items.Add(treeViewItem);
                    treeViewItem.IsExpanded = true;

                }
            }
        }

        private void TreeGroups_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            FreqGroup selectedGroup = GetSelectedGroup();
            if (selectedGroup==null)
            {
                this.MnuGroupsNew.IsEnabled = false;
                this.MnuGroupsRename.IsEnabled = false;
                this.MnuGroupsDelete.IsEnabled = false;
            }
            else
            {
                this.MnuGroupsNew.IsEnabled = true;
                this.MnuGroupsRename.IsEnabled = true;
                this.MnuGroupsDelete.IsEnabled = true;
            }
        }

        private void TreeGroups_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TreeGroups.SelectedItem is TreeViewItem)
            {
                TreeViewItem item = TreeGroups.SelectedItem as TreeViewItem;
                this.DataGridFreqs.ItemsSource = ((FreqGroup)item.Tag).AllFreqs;
            }
        }

        #endregion
    }
}
