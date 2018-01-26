using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Wpf.DataGrid;

namespace FreqMngr.Helpers
{
    class CustomDataGridControl : DataGridControl
    {
        public static readonly DependencyProperty BindableSelectedItemsProperty =
             DependencyProperty.Register("BindableSelectedItems", typeof(IList), typeof(CustomDataGridControl), new UIPropertyMetadata(null));    

        public IList BindableSelectedItems
        {
            get
            {
                IList list = (IList)GetValue(BindableSelectedItemsProperty);
                if (list == null)
                    Debug.WriteLine("BindableSelectedItems: list==null");
                else
                    Debug.WriteLine("BindableSelectedItems: count==" + list.Count.ToString());

                return list;
            }
            set { SetValue(BindableSelectedItemsProperty, value); }
        }

        public CustomDataGridControl()
        {
            SelectionChanged += CustomDataGridControl_SelectionChanged;
        }

        private void CustomDataGridControl_SelectionChanged(object sender, DataGridSelectionChangedEventArgs e)
        {
            BindableSelectedItems = SelectedItems;
        }
    }
}
