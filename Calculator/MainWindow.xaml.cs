using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private decimal _currentValue = 0;
        private decimal _previousValue = 0;
        private string _operation = "";
        private bool _isNew = true;
        private const int DisplayLength = 16;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string content = button.Content.ToString();

            if (char.IsDigit(content, 0) || content == ".")
            {
                if (_isNew)
                {
                    Display.Text = content == "." ? "0." : content;
                    _isNew = false;
                }
                else
                {
                    if (Display.Text.Length < DisplayLength)
                    {
                        Display.Text += content;
                    }
                }
                _currentValue = decimal.Parse(Display.Text);
            }
            else
            {
                switch (content)
                {
                    case "AC":
                        _currentValue = 0;
                        _previousValue = 0;
                        _operation = "";
                        Display.Text = "0";
                        _isNew = true;
                        break;

                    case "+/-":
                        _currentValue = -_currentValue;
                        Display.Text = _currentValue.ToString();
                        break;

                    case "%":
                        _currentValue /= 100;
                        Display.Text = _currentValue.ToString();
                        break;

                    case "=":
                        if (!string.IsNullOrEmpty(_operation))
                        {
                            Calculate();
                            _operation = "";
                            _previousValue = _currentValue;
                        }
                        _isNew = true;
                        break;

                    default:
                        if (!string.IsNullOrEmpty(_operation))
                        {
                            CalculateIntermediate();
                        }
                        else
                        {
                            _previousValue = _currentValue;
                        }
                        _operation = content;
                        _isNew = true;
                        break;
                }
            }
        }

        private void Calculate()
        {
            try
            {
                switch (_operation)
                {
                    case "+":
                        _currentValue = _previousValue + _currentValue;
                        break;
                    case "-":
                        _currentValue = _previousValue - _currentValue;
                        break;
                    case "×":
                        _currentValue = _previousValue * _currentValue;
                        break;
                    case "÷":
                        if (_currentValue != 0)
                        {
                            _currentValue = _previousValue / _currentValue;
                            _currentValue = Math.Round(_currentValue, 5);
                        }
                        else
                        {
                            Display.Text = "Undefined";
                            return;
                        }
                        break;
                }

                if (_currentValue.ToString().Length > DisplayLength)
                {
                    Display.Text = "Limit Exceeded";
                    ResetCalculator();
                }
                else
                {
                    Display.Text = _currentValue.ToString();
                }
            }
            catch (OverflowException)
            {
                Display.Text = "Limit Exceeded";
                ResetCalculator();
            }
        }

        private void CalculateIntermediate()
        {
            try
            {
                switch (_operation)
                {
                    case "+":
                        _previousValue += _currentValue;
                        break;
                    case "-":
                        _previousValue -= _currentValue;
                        break;
                    case "×":
                        _previousValue *= _currentValue;
                        break;
                    case "÷":
                        if (_currentValue != 0)
                        {
                            _previousValue = _previousValue / _currentValue;
                            _previousValue = Math.Round(_previousValue, 5);
                        }
                        else
                        {
                            Display.Text = "Undefined";
                            return;
                        }
                        break;
                }

                if (_previousValue.ToString().Length > DisplayLength)
                {
                    Display.Text = "Limit Exceeded";
                    ResetCalculator();
                }

                _currentValue = 0;
            }
            catch (OverflowException)
            {
                Display.Text = "Limit Exceeded";
                ResetCalculator();
            }
        }

        private void ResetCalculator()
        {
            _currentValue = 0;
            _previousValue = 0;
            _operation = "";
            _isNew = true;
        }
    }
}
