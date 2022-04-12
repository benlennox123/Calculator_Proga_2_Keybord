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

namespace Calculator_Proga
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public const int n = 16; //ограничение на размер числа калькулятора

        //Ввод только цифр
        private void Window_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            if (!Int32.TryParse(e.Text, out val))
            {
                e.Handled = true;               //отклонить ввод НЕцифр
            }
            else
            {
                if (monitor.Text == "0")        //убрать 0 для ввода числа
                {
                    monitor.Text = "";
                }
                if (monitor.Text.Length < n) //ограничение на длину ввода
                {
                    monitor.Text += e.Text;
                }

            }
        }
        //

        //Проврка нажатия клавиши
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    enter.Command.Execute(enter.CommandParameter);
                    break;
                case Key.Add:
                    plus.Command.Execute(plus.CommandParameter);
                    break;
                case Key.Subtract:
                    minus.Command.Execute(minus.CommandParameter);
                    break;
                case Key.Multiply:
                    multiply.Command.Execute(multiply.CommandParameter);
                    break;
                case Key.Divide:
                    divide.Command.Execute(divide.CommandParameter);
                    break;
                case Key.Back:
                    back.Command.Execute(back.CommandParameter);
                    break;
                case Key.Decimal:
                    point.Command.Execute(point.CommandParameter);
                    break;
            }
        }
    }
}
