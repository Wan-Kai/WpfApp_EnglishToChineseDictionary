using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace WpfApp_EnglishToChineseDictionary
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
        }

        private void ModifyUI()
        {
            // 模拟一些工作正在进行
            Thread.Sleep(TimeSpan.FromSeconds(2));

          
        }
        

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            DataUtil data = new DataUtil();
            string userName = txt_userName.Text;
            string password = txt_password.Password;


            if (data.Login(userName, password) == 1)
            {
                User userWindow = new User();
                userWindow.Show();
                this.Close();
            }
            else if(data.Login(userName, password) == 2)
            {
                Manager managerWindow = new Manager();
                managerWindow.Show();
                this.Close();
            }
            else
            {
                Warning warningWindow = new Warning("错误的身份，请核对填入的信息");
                warningWindow.Show();
            }
        }
    }
}
