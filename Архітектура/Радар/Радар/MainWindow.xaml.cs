using System;
using System.Windows;
//Підключаємо бібліотеку для зчиткування даних з Serial порту
using System.IO.Ports;


namespace Радар
{
    public partial class MainWindow : Window
    {
        //Оголошуємо змінні порту, вікна радару та інші
        SerialPort myPort;
        WinRadar winRadar;
        string[] ports;
        string Data;
        public MainWindow()
        {         
            InitializeComponent();

            ports = SerialPort.GetPortNames();

            AddItems(ports);

            selectPort.Text = ports[0];

            selectSpeed.Text = "9600";

            myPort = new SerialPort();

            myPort.DataReceived += MyPort_DataReceived;

            butRadar.IsEnabled = false;
        }
        //Обробник події DataReceived
        private void MyPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Action action = () =>
            {
                Data = myPort.ReadExisting();
                int angle = Convert.ToInt32(GetParam(Data)[0]);
                int distance = Convert.ToInt32(GetParam(Data)[1]);
                if(angle % 2 == 0)
                    winRadar.AnimLine(angle / 2, 1000, 1, 0.15, distance);
            };
                Dispatcher.Invoke(action);           
        }
        //Функція яка розбиває єдину строку в масив строк
        string[] GetParam(string str)
        {
             return str.Split('.', ',');
        }
        //Обробник події Click на кнопці старт
        private void ButStart_Click(object sender, RoutedEventArgs e)
        {

            if (butStart.Content.ToString() == "Пуск")
            {
                string portName = selectPort.SelectedItem.ToString();
                string portRateStr = selectSpeed.SelectedItem.ToString();

                butStart.Content = "Стоп";
                selectPort.IsEnabled = false;
                selectSpeed.IsEnabled = false;
                butRadar.IsEnabled = true;

                winRadar = new WinRadar();
                winRadar.InitializeComponent();
                winRadar.Closing += WinRadar_Closing;

                Connect(portName, portRateStr);
            }
            else
            { 
                butStart.Content = "Пуск";
                selectPort.IsEnabled = true;
                selectSpeed.IsEnabled = true;
                myPort.Close();
                butRadar.IsEnabled = false;
            }
        }
        //Обробник події Click на кнопці стоп
        private void WinRadar_Closing(object sender, EventArgs e)
        {
            butStart.Content = "Пуск";
            selectPort.IsEnabled = true;
            selectSpeed.IsEnabled = true;
            myPort.Close();
            butRadar.IsEnabled = false;
        }
        //Функція яка відкриває порт для зчитування
        void Connect(string name, string speed)
        {
                myPort.PortName = name;
                myPort.BaudRate = Convert.ToInt32(speed);
                myPort.Open();
        }
        //Функція яка додає всі наявні порти в випадаючий список
        void AddItems(string[] strs)
        {
            foreach (string str in strs)
            {
                selectPort.Items.Add(str);
            }
        }
        //Обробник події Click на кнопці радар
        private void ButRadar_Click(object sender, RoutedEventArgs e)
        {           
            winRadar.Show();
        }
    }
}
