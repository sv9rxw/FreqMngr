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

namespace FreqMngr
{
    /// <summary>
    /// Interaction logic for FrequencyBox.xaml
    /// </summary>
    public partial class FrequencyBox : UserControl
    {
        private static readonly List<Key> DigitKeys = new List<Key> { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9,
            Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9};
        private static readonly List<Key> MoveForwardKeys = new List<Key> { Key.Right };
        private static readonly List<Key> MoveBackwardKeys = new List<Key> { Key.Left };
        private static readonly List<Key> OtherAllowedKeys = new List<Key> { Key.Tab, Key.Delete };

        private readonly List<TextBox> _segments = new List<TextBox>();
        private bool _suppressFrequencyUpdate = false;


        public FrequencyBox()
        {
            InitializeComponent();
            InitializeComponent();
            _segments.Add(FirstSegment);
            _segments.Add(SecondSegment);
            _segments.Add(ThirdSegment);
            _segments.Add(LastSegment);
        }

        public static readonly DependencyProperty FrequencyProperty = DependencyProperty.Register(
            "Address", typeof(string), typeof(FrequencyBox), new FrameworkPropertyMetadata(default(string), FrequencyChanged)
            {
                BindsTwoWayByDefault = true
            });

        private static void FrequencyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var freqTextBox = dependencyObject as FrequencyBox;
            var text = e.NewValue as string;

            if (text != null && freqTextBox != null)
            {
                freqTextBox._suppressFrequencyUpdate = true;
                var i = 0;

                // Get Hz (decimal part)
                var parts = text.Split(',');
                if (parts.Length!=2)
                {
                    freqTextBox._segments[3].Text = "000";
                }
                else
                {
                    freqTextBox._segments[3].Text = parts[1];
                }

                var thousands = parts[0].Split('.');

                if (thousands.Length==1)
                {
                    freqTextBox._segments[0].Text = "000";
                    freqTextBox._segments[1].Text = "000";
                    freqTextBox._segments[2].Text = thousands[0];
                }
                else if (thousands.Length==2)
                {
                    freqTextBox._segments[0].Text = "000";
                    freqTextBox._segments[1].Text = thousands[0];
                    freqTextBox._segments[2].Text = thousands[1];
                }
                else if (thousands.Length==3)
                {
                    freqTextBox._segments[0].Text = thousands[0];
                    freqTextBox._segments[1].Text = thousands[1];
                    freqTextBox._segments[2].Text = thousands[2];

                }
                freqTextBox._suppressFrequencyUpdate = false;
            }
        }

        public string Frequency
        {
            get { return (string)GetValue(FrequencyProperty); }
            set { SetValue(FrequencyProperty, value); }
        }

        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (DigitKeys.Contains(e.Key))
            {
                e.Handled = ShouldCancelDigitKeyPress();
                HandleDigitPress();
            }
            else if (MoveBackwardKeys.Contains(e.Key))
            {
                e.Handled = ShouldCancelBackwardKeyPress();
                HandleBackwardKeyPress();
            }
            else if (MoveForwardKeys.Contains(e.Key))
            {
                e.Handled = ShouldCancelForwardKeyPress();
                HandleForwardKeyPress();
            }
            else if (e.Key == Key.Back)
            {
                HandleBackspaceKeyPress();
            }
            else if (e.Key == Key.OemPeriod)
            {
                e.Handled = true;
                HandlePeriodKeyPress();
            }
            else
            {
                e.Handled = !AreOtherAllowedKeysPressed(e);
            }
        }

        private bool AreOtherAllowedKeysPressed(KeyEventArgs e)
        {
            return e.Key == Key.C && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0) ||
                   e.Key == Key.V && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0) ||
                   e.Key == Key.A && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0) ||
                   e.Key == Key.X && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0) ||
                   OtherAllowedKeys.Contains(e.Key);
        }

        private void HandleDigitPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox != null && currentTextBox.Text.Length == 3 &&
                currentTextBox.CaretIndex == 3 && currentTextBox.SelectedText.Length == 0)
            {
                MoveFocusToNextSegment(currentTextBox);
            }
        }

        private bool ShouldCancelDigitKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;
            return currentTextBox != null &&
                   currentTextBox.Text.Length == 3 &&
                   currentTextBox.CaretIndex == 3 &&
                   currentTextBox.SelectedText.Length == 0;
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_suppressFrequencyUpdate)
            {
                Frequency = string.Format("{0}.{1}.{2}.{3}", FirstSegment.Text, SecondSegment.Text, ThirdSegment.Text, LastSegment.Text);
            }

            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox != null && currentTextBox.Text.Length == 3 && currentTextBox.CaretIndex == 3)
            {
                MoveFocusToNextSegment(currentTextBox);
            }
        }

        private bool ShouldCancelBackwardKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;
            return currentTextBox != null && currentTextBox.CaretIndex == 0;
        }

        private void HandleBackspaceKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox != null && currentTextBox.CaretIndex == 0 && currentTextBox.SelectedText.Length == 0)
            {
                MoveFocusToPreviousSegment(currentTextBox);
            }
        }

        private void HandleBackwardKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox != null && currentTextBox.CaretIndex == 0)
            {
                MoveFocusToPreviousSegment(currentTextBox);
            }
        }

        private bool ShouldCancelForwardKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;
            return currentTextBox != null && currentTextBox.CaretIndex == 3;
        }

        private void HandleForwardKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox != null && currentTextBox.CaretIndex == currentTextBox.Text.Length)
            {
                MoveFocusToNextSegment(currentTextBox);
            }
        }

        private void HandlePeriodKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox != null && currentTextBox.Text.Length > 0 && currentTextBox.CaretIndex == currentTextBox.Text.Length)
            {
                MoveFocusToNextSegment(currentTextBox);
            }
        }

        private void MoveFocusToPreviousSegment(TextBox currentTextBox)
        {
            if (!ReferenceEquals(currentTextBox, FirstSegment))
            {
                var previousSegmentIndex = _segments.FindIndex(box => ReferenceEquals(box, currentTextBox)) - 1;
                currentTextBox.SelectionLength = 0;
                currentTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                _segments[previousSegmentIndex].CaretIndex = _segments[previousSegmentIndex].Text.Length;
            }
        }

        private void MoveFocusToNextSegment(TextBox currentTextBox)
        {
            if (!ReferenceEquals(currentTextBox, LastSegment))
            {
                currentTextBox.SelectionLength = 0;
                currentTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void DataObject_OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText)
            {
                e.CancelCommand();
                return;
            }

            var text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;

            int num;

            if (!int.TryParse(text, out num))
            {
                e.CancelCommand();
            }
        }


    }
}
