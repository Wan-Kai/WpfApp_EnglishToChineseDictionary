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
    /// Warning.xaml 的交互逻辑
    /// </summary>
    public partial class Warning : Window
    {
        public Warning(string s)
        {
            InitializeComponent();
            txb_warning.Inlines.Add(new Run(s));

        }

        private void btn_right_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
