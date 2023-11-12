using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace model_metro
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserData userdata;
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            int startHour = (int)begtime_hour.Value;
            int startMinute = (int)begtime_min.Value;
            int endHour = (int)endtime_hour.Value;
            int endMinute = (int)endtime_min.Value;
            if (int.TryParse(RedBranch.Text, out int redBranchValue) && redBranchValue > 1 && redBranchValue < 60 && int.TryParse(GreenBranch.Text, out int GreenBranchValue))
            {
                userdata.SetUserData(startHour, startMinute, endHour, endMinute, redBranchValue, GreenBranchValue);
                string json = JsonConvert.SerializeObject(userdata);
                File.WriteAllText("userdata.json", json);
            }
            else MessageBox.Show($"Произошла ошибка при сохранении данных:", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private UserData DataLoaded()
        {
            UserData data;
            if (File.Exists("userdata.json"))
            {
                string json = File.ReadAllText("userdata.json");
                UserData userData = JsonConvert.DeserializeObject<UserData>(json);
                data = userData;
            }
            else data = new UserData();
            RedBranch.Text = data.RedBranchValue.ToString();
            GreenBranch.Text = data.GreenBranchValue.ToString();
            begtime_hour.Text = data.StartHour.ToString();
            begtime_min.Text = data.StartMinute.ToString();
            endtime_hour.Text = data.EndHour.ToString();    
            endtime_min.Text = data.EndMinute.ToString();   
            return data;
        }
        public MainWindow()
        {
            InitializeComponent();
            AnimateCirclesAsync();
            userdata = DataLoaded();
            Closing += MainWindow_Closing;
        }
        private async void AnimateCirclesAsync()
        {
            Ellipse[] circlesgreen = { circle1, circle2, circle3, circle4, circle5 };
            Ellipse[] circlesred = { circle6, circle7, circle3, circle8, circle9, circle10, circle11 };
            while (true)
            {
                for (int i = 0; i < circlesgreen.Length; i++)
                {
                    await AnimateCircleAsync(circlesred[i], true);
                    await AnimateCircleAsync(circlesgreen[i], false);
                }
                await AnimateCircleAsync(circlesred[circlesred.Length - 2], true);
                await AnimateCircleAsync(circlesred[circlesred.Length - 1], true);
                for (int i = circlesgreen.Length - 2; i >= 0; i--)
                {
                    await AnimateCircleAsync(circlesred[i + 2], true);
                    await AnimateCircleAsync(circlesgreen[i], false);
                }
                await AnimateCircleAsync(circlesred[0], true);
                await AnimateCircleAsync(circlesred[1], true);
            }
        }

        private async Task AnimateCircleAsync(Ellipse circle, bool branch)
        {
            SolidColorBrush originalColor = (SolidColorBrush)circle.Fill;
            if (branch) circle.Fill = Brushes.Red;
            else circle.Fill = Brushes.Green;
            
            DoubleAnimation opacityAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.5),
                AutoReverse = true
            };

            circle.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
            await Task.Delay(1000);
            circle.Fill = originalColor;
        }

        private void SimulateButton_Click(object sender, RoutedEventArgs e)
        {
            int startHour = (int)begtime_hour.Value;
            int startMinute = (int)begtime_min.Value;
            int endHour = (int)endtime_hour.Value;
            int endMinute = (int)endtime_min.Value;
            int startTime = startHour * 60 + startMinute;
            int endTime = endHour * 60 + endMinute;
            if (int.TryParse(RedBranch.Text, out int redBranchValue) && redBranchValue > 1 && redBranchValue < 60 && int.TryParse(GreenBranch.Text, out int GreenBranchValue) && GreenBranchValue > 1 && GreenBranchValue < 60 && (startTime<endTime))
            {  
                
                Metro m = new Metro(redBranchValue, GreenBranchValue, startTime, endTime);
                m.Simulate();
                string fileName = Microsoft.VisualBasic.Interaction.InputBox("Введите название файла:", "Ввод названия файла", "Data");
                if (!string.IsNullOrEmpty(fileName))
                {
                    try
                    {
                        m.ExportToExcel(fileName);                        
                        MessageBox.Show("Данные успешно сохранены в файл " + fileName+".xlsx");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при сохранении файла: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Название файла не введено.");
                }
            }
            else MessageBox.Show("Неверное значение.");
        }
    }
}


