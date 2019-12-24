using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
using System.Text.RegularExpressions;

namespace WpfApp_EnglishToChineseDictionary
{
    /// <summary>
    /// Manager.xaml 的交互逻辑
    /// </summary>
    public partial class Manager : Window
    {
        private int status = 0;   //删除      0 确认        1 取消
        public Manager()
        {
            InitializeComponent();
            DataUtil data = new DataUtil();
            DataTable dt = data.ViewData();
            gridWords.ItemsSource = dt.DefaultView;
        }
        

        //0 正常  1 中文错误  2 英文错误  3 中文释义错误  4 英文释义错误  5 中文与英文重复
        private int check(string Chinese,string English,string paraC,string paraE) 
        {
            DataUtil data = new DataUtil();
            if (Chinese.Equals("") || !data.IsChinese(Chinese))
                return 1;

            if (English.Equals("") || !data.IsEnglish(English))
                return 2;

            if (paraC.Equals("") || data.IsEnglish(paraC))
                return 3;

            if (paraE.Equals("") || data.IsChinese(paraE))
                return 4;

            if (!data.check(Chinese, English))
                return 5;
            
            return 0;
        }
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            DataUtil data = new DataUtil();
            string Chinese = txt_Chinese.Text;
            string English = txt_English.Text;
            string paraC = txt_paraC.Text;
            string paraE = txt_paraE.Text;

            int checkNum = check(Chinese, English, paraC, paraE);
            switch (checkNum)
            {
                case 0:
                    data.AddData(Chinese, English, paraC, paraE);

                    txt_Chinese.Text = "";
                    txt_English.Text = "";
                    txt_paraC.Text = "";
                    txt_paraE.Text = "";
                    DataTable dt = data.ViewData();
                    gridWords.ItemsSource = dt.DefaultView;
                    break;
                case 1:
                    Warning warningWindow1 = new Warning("请输入正确的中文！");
                    warningWindow1.Show();
                    break;
                case 2:
                    Warning warningWindow2 = new Warning("请输入正确的英文！");
                    warningWindow2.Show();
                    break;
                case 3:
                    Warning warningWindow3 = new Warning("请输入正确的中文释义！");
                    warningWindow3.Show();
                    break;
                case 4:
                    Warning warningWindow4 = new Warning("请输入正确的英文释义！");
                    warningWindow4.Show();
                    break;
                case 5:
                    Warning warningWindow5 = new Warning("此中英文释义已存在！");
                    warningWindow5.Show();
                    break;
                default: break;
            }

        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = this.gridWords.SelectedItem;
            if(selectedRow == null)
            {
                Warning warningWindow = new Warning("请选择要删除的对象！");
                warningWindow.Show();
                return;
            }

            var selectedView = selectedRow as DataRowView;
            Delete deleteWindow = new Delete(selectedView["Chinese"].ToString(), selectedView["English"].ToString(),
                selectedView["paraphraseC"].ToString(), selectedView["paraphraseE"].ToString());
            deleteWindow.PassDataBetweenForm += new Delete.PassDataBetweenFormHandler(FrmChild_PassDataBetweenForm);
            deleteWindow.Show();
            deleteWindow.Activate();
            
        }

        private void FrmChild_PassDataBetweenForm(object sender, PassDataWinFormEventArgs e)
        {
            this.status = e.Status;

            DataUtil data = new DataUtil();
            var selectedRow = this.gridWords.SelectedItem;
            var selectedView = selectedRow as DataRowView;

            if (status == 0)
            {
                data.DeleteData(selectedView["Id"].ToString());
                DataTable dt = data.ViewData();
                gridWords.ItemsSource = dt.DefaultView;
            }
            else if (status == 1)
            {
                DataTable dt = data.ViewData();
                gridWords.ItemsSource = dt.DefaultView;
            }
        }

        private void btn_alter_Click(object sender, RoutedEventArgs e)
        {
            DataUtil data = new DataUtil();
            var selectedRow = this.gridWords.SelectedItem;
            var selectedView = selectedRow as DataRowView;
            data.UpdateData(selectedView["Id"].ToString(), selectedView["Chinese"].ToString(), 
                selectedView["English"].ToString(), selectedView["paraphraseC"].ToString(), selectedView["paraphraseE"].ToString());
            DataTable dt = data.ViewData();
            gridWords.ItemsSource = dt.DefaultView;
        }     
    }
}

public class PassDataWinFormEventArgs : EventArgs
{

    public PassDataWinFormEventArgs()
    {
        //
    }
    public PassDataWinFormEventArgs(int refStatus)
    {
        this._status = refStatus;
    }
    
    private int _status;
    
    public int Status
    {
        get { return _status; }
        set { _status = value; }
    }
}