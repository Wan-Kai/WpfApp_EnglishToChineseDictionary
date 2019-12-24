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
using System.Windows.Shapes;

namespace WpfApp_EnglishToChineseDictionary
{
    /// <summary>
    /// User.xaml 的交互逻辑
    /// </summary>
    public partial class User : Window
    {
        public User()
        {
            InitializeComponent();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            DataUtil data = new DataUtil();
            string explain = data.GetWord(txt_word.Text);
            
            if (explain.Equals(""))
            {
                Warning warningWindow = new Warning("抱歉，尚未收录");
                warningWindow.Show();
            }
            else
            {
                string[] explains = explain.Split('?');
                txb_showWord.Text = "单词：" + explains[0];
                txb_showExplain.Text = "释义：" + explains[1];
            }
           
        }

        private void btn_manager_Click(object sender, RoutedEventArgs e)
        {
            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

    }
}
