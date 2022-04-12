using Calculator_Proga.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Calculator_Proga.ViewModels
{
    class MainWindowViewModels : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        private bool negative = false; //отрицательное или положительное число
        private bool deci = false; //десятичная дробь или целое

        //Первое число
        private string number1;
        public string Number1
        {
            get => number1;
            set
            {
                number1 = value;
                OnPropertyChanged();
            }
        }
        //

        //Второе и последующее число
        private string number2;
        public string Number2
        {
            get => number2;
            set
            {
                number2 = value;
                OnPropertyChanged();
            }
        }
        //

        //Число ввода/вывода
        private string inOut = "0";
        public string InOut
        {
            get => inOut;
            set
            {
                inOut = value;
                OnPropertyChanged();
            }
        }
        //

        //Выбор команды на 2 числа
        private string action = "";
        public string Action
        {
            get => action;
            set
            {
                action = value;
                OnPropertyChanged();
            }
        }
        //

        //Выбор команды на 1 число
        private string equ = "";
        public string Equ
        {
            get => equ;
            set
            {
                equ = value;
                OnPropertyChanged();
            }
        }
        //

        //Результат
        private double result = 0;
        public double Result
        {
            get => result;
            set
            {
                result = value;
                OnPropertyChanged();
            }
        }
        //

        //Команда вычисления результата
        public ICommand CalcMathCommand { get; }

        private void OnCalcMathCommandExecute(object p)
        {
            Equ = p.ToString();
            if (Equ == "=")                 //Для действий в два значения
            {
                if (Result == 0)            //если ввели второе значение
                {
                    Number2 = InOut;
                }
                else                        //если НЕ ввели второе значение, а также для повторения прошлого действия к текущему результату
                {
                    Number1 = Result.ToString();
                }
                switch (Action)
                {
                    case "+":
                        Result = CalcMath.Add(Convert.ToDouble(Number1), Convert.ToDouble(Number2));
                        break;
                    case "-":
                        Result = CalcMath.Sub(Convert.ToDouble(Number1), Convert.ToDouble(Number2));
                        break;
                    case "x":
                        Result = CalcMath.Mul(Convert.ToDouble(Number1), Convert.ToDouble(Number2));
                        break;
                    case "/":
                        if(Number2!="0")
                        {
                            Result = CalcMath.Div(Convert.ToDouble(Number1), Convert.ToDouble(Number2));
                        }
                        else
                        {
                            MessageBox.Show("Деление на ноль");  //Без этой проверки не выдаёт ошибку при делении на 0, а показывает бесконечность. С проверкой можно ввести другое число
                        }
                        break;
                }
                InOut = Result.ToString();
            }
            else                            //для действий в одно значение
            {
                Number1 = "";               //результат получаем после результата, - обнуляем для верхней строки формулы
                Number2 = "";               //
                Action = "";                //
                negative = false;
                deci = false;
                switch (Equ)
                {
                    case "squ":
                        if(Convert.ToDouble(InOut)>=0)
                        {
                            Result = CalcMath.Squ(Convert.ToDouble(InOut));
                            Equ = "√" + InOut.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Корень из отрицательного числа"); //Без проверки ошибки нет, можно просто сбросить ввод
                        }
                        break;
                    case "pow":
                        Result = CalcMath.Pow(Convert.ToDouble(InOut));
                        Equ = InOut.ToString() + "²";
                        break;
                    case "dec":
                        if (InOut != "0")
                        {
                            Result = CalcMath.Dec(Convert.ToDouble(InOut));
                            Equ = "1/" + InOut.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Деление на ноль");  //Без этой проверки не выдаёт ошибку при делении на 0, а показывает бесконечность
                        }
                        break;
                }
                InOut = Result.ToString();
                Result = 0;     //обнуляю, чтобы не было ошибки при действие в два значения, если одно из значений результат команды в одно значение
            }
            //InOut = Result.ToString();
            //Result = 0;                     //обнуляю, чтобы не было ошибки при действие в два значения, если одно из значений результат команды в одно значение
            negative = false;
        }
        //

        //Команда ввода числа
        public ICommand ButtonNumberCommand { get; }

        private void OnButtonNumberCommandExecute(object p)
        {
            if (Result != 0 && Equ == "=")          //для ввода новых вычислений сразу после уже полученного результата, с полным сбросом
            {
                Number1 = "";
                Number2 = "";
                InOut = "0";
                Action = "";
                Equ = "";
                Result = 0;
                negative = false;
                deci = false;
            }
            if (InOut == "0")                       //чтобы число не начиналось с нуля
            {
                if ((string)p == ",")               //за исключением десятичных дробей
                {
                    InOut = "0";
                }
                else
                {
                    InOut = "";
                }
            }
            if (InOut.Length < MainWindow.n)        //Ограничение на размер числа
            {
                if ((string)p == "-" && negative == false)          //инверсия отрицательное/положительное число
                {
                    InOut = "-" + InOut;
                    negative = true;
                }
                else
                {
                    if ((string)p == "-" && negative == true)
                    {
                        if (InOut.Length != 0)
                        {
                            InOut = InOut.Substring(1);
                        }
                        negative = false;
                        if (InOut == "")                        //при инверсии нуля, вернуть 0
                        {
                            InOut = "0";
                        }
                    }
                    else
                    {
                        if (deci == false)                     //проверка на дробь
                        {

                            if ((string)p == ",")
                            {
                                InOut += p.ToString();
                                deci = true;                //определение десятичной дроби
                            }
                        }
                        //else
                        //{
                        if ((string)p != ",")
                        {
                            if ((string)p == "0" && InOut == "-")           //чтобы не было бесконечных нулей сразу после минуса
                            {
                                InOut = "-";
                            }
                            else
                            {
                                InOut += p.ToString();                  //ввод числа

                            }
                        }

                        //}
                    }
                }
            }
        }
        //

        //Команда кнопок выбора действия
        public ICommand ButtonActionCommand { get; }

        private void OnButtonActionCommandExecute(object p)
        {
            if(Equ!="=")            //Очищаем, если происходит действие после команды в одно значение
            {
                Equ = "";               
            }
            if (Action == "")                      //проверка, вводилось ли уже действие
            {
                Action = p.ToString();
                Number1 = InOut;
                InOut = "0";
                negative = false;
                deci = false;
            }
            else                                //если действие уже было: можно совершить смену действия, или начать новое действие
            {
                if (InOut == "0")           //разрешает смену действия только, при нулевом втором числе
                {
                    if (Action != p.ToString())
                    {
                        Action = p.ToString();
                    }
                }
                else                        //Для нового действия с промежуточным результатом
                {
                    if (Equ == "")            //Если результат уже есть, не позволит запустить получение промежуточного результата, без новых данных
                    {
                        OnCalcMathCommandExecute("=");
                    }
                    Number1 = Result.ToString();
                    Number2 = "";
                    InOut = "0";
                    Equ = "";
                    Result = 0;
                    negative = false;
                    deci = false;
                    if (Action != p.ToString())     //смена действия после действия, например 5+6-3 это 11-3
                    {
                        Action = p.ToString();
                    }
                }
            }
        }
        //

        //Команда кнопок редактирования ввода
        public ICommand ButtonEditCommand { get; }

        private void OnButtonEditCommandExecute(object p)
        {
            switch ((string)p)
            {
                case "⌫":
                    if (InOut.Length > 0)                              //Чтобы не удалить меньше нулевой строки
                    {
                        InOut = InOut.Remove(InOut.Length - 1);
                        if (InOut.Length == 0)
                        {
                            InOut = "0";                            //а при нулевой строке отобразить 0
                        }
                    }
                    break;
                case "CE":
                    InOut = "0";
                    deci = false;
                    negative = false;
                    break;
                case "C":
                    Number1 = "";
                    Number2 = "";
                    InOut = "0";
                    Action = "";
                    Equ = "";
                    Result = 0;
                    deci = false;
                    negative = false;
                    break;
            }
        }
        //

        //Команда расчёт процентов
        public ICommand ButtonProcCommand { get; }

        private void OnButtonProcCommandExecute(object p)
        {
            if(Number1=="" || Number1==null)
            {
                InOut = (Convert.ToDouble(InOut) / 100).ToString();         //получение процента пример: 5% - это 0,05
            }
            else
            {
                InOut= ((Convert.ToDouble(Number1)* Convert.ToDouble(InOut)) / 100).ToString();     //получение процента от числа пример: 100 + 5% это 100+(5% от 100)
            }
        }
        //

        //Команды математических вычислений
        public MainWindowViewModels()
        {
            CalcMathCommand = new RelayCommand(OnCalcMathCommandExecute);
            ButtonNumberCommand = new RelayCommand(OnButtonNumberCommandExecute);
            ButtonActionCommand = new RelayCommand(OnButtonActionCommandExecute);
            ButtonEditCommand = new RelayCommand(OnButtonEditCommandExecute);
            ButtonProcCommand = new RelayCommand(OnButtonProcCommandExecute);
        }
        //
    }
}
