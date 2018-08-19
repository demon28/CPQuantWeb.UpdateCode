using ParkingSqlHelper;
using SysOpenCode.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysOpenCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            int days = 0 - int.Parse(this.textBox1.Text);

            DateTime endday = DateTime.Today;
            DateTime sarday = DateTime.Today.AddDays(days);


            while (sarday<=endday)
            {
                

                string today = sarday.ToString("yyyyMMdd");
                string url = string.Format("http://kaijiang.500.com/static/public/ssc/xml/qihaoxml/{0}.xml?_A=IEAKFSZM1534655958600", today);

                sarday= sarday.AddDays(1);

                HttpClient client = new HttpClient();
                string html = await client.GetStringAsync(url);

                xml xml= XmlHelper.Deserialize<xml>(html);

                for (int j = 0; j < xml.row.Count; j++)
                {
                    Tcp_Hiscode t = new Tcp_Hiscode();
                    t.Expect = xml.row[j].expect;
                    t.Opencode = xml.row[j].opencode;
                    t.Datetime = DateTime.Parse( xml.row[j].opentime);

                    if (t.selectbyecpect(t.Expect))
                    {
                        continue;
                    }
                   

                    if (t.Insert())
                    {
                       //this.textBox1.Text += "\r\n" + t.Expect;
                    } 

                
                }


            }


            MessageBox.Show("更新完成！");

        }

    }
}
