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
    /// Delete.xaml 的交互逻辑
    /// </summary>
    public partial class Delete : Window
    {
        //添加一个委托
        public delegate void PassDataBetweenFormHandler(object sender, PassDataWinFormEventArgs e);
        //添加一个PassDataBetweenFormHandler类型的事件
        public event PassDataBetweenFormHandler PassDataBetweenForm;

        public Delete(string Chinese,string English,string paraC,string paraE)
        {
            InitializeComponent();
            txb_warning.Inlines.Add(new Run("中文：" + Chinese + "   英文：" + English + "   中文释义：" + paraC + "   英文释义：" + paraE));
        }

        private void btn_right_Click(object sender, RoutedEventArgs e)
        {
            PassDataWinFormEventArgs status = new PassDataWinFormEventArgs(0);
            PassDataBetweenForm(this, status);
            this.Close();
        }

        private void btn_wrong_Click(object sender, RoutedEventArgs e)
        {
            PassDataWinFormEventArgs status = new PassDataWinFormEventArgs(1);
            PassDataBetweenForm(this, status);
            this.Close();
        }
    }
}
