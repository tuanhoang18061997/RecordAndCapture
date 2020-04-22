using AlertBox.AlertBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlertBox
{
    
    public partial class AlertBoxs : Form
    {
        public enum TypeAlert
        {
            AlertSuccess = 0,
            AlertInfo = 1,
            AlertWarning = 2,
            AlertError = 3,
        }
        public static AlertBoxs AlertBox;
        public static Boolean statusClose = true;
        int positionY = 0;
        public  AlertBoxs()
        {
            InitializeComponent();
        }
        public static void Show(string caption,string content,int typeAlert)
        {
            AlertBox = new AlertBoxs(); 
            AlertBox.Controls.Clear();
            switch (typeAlert)
            {
                case 0:
                    ucAlertSuccess ucSuccess = new ucAlertSuccess();
                    ucSuccess.SetDisplay(caption, content);
                    AlertBox.Controls.Add(ucSuccess);
                    break;
                case 1:
                    ucAlertInfo ucInfo = new ucAlertInfo();
                    ucInfo.SetDisplay(caption, content);
                    AlertBox.Controls.Add(ucInfo);
                    break;
                case 2:
                    ucAlertWarning unWarning = new ucAlertWarning();
                    unWarning.SetDisplay(caption, content);
                    AlertBox.Controls.Add(unWarning);
                    break;
                case 3:
                    ucAlertError ucError = new ucAlertError();
                    ucError.SetDisplay(caption, content);
                    AlertBox.Controls.Add(ucError);
                    break;

            }
            AlertBox.Show();
        }

        private void AlertBoxs_Load(object sender, EventArgs e)
        {
            this.Left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            this.Top = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void timeShow_Tick(object sender, EventArgs e)
        {
            statusClose = true;
            if(this.Top > Screen.PrimaryScreen.WorkingArea.Height - this.Height)
            {
                this.Top -= positionY;
                positionY += 1;
            }
            else
            {
                timeShow.Stop();
                timerClose.Start();
            }
        }

        private void timerClose_Tick(object sender, EventArgs e)
        {
            if(statusClose && this.Opacity > 0)
            {
                this.Opacity -= 0.025;
            }else if(!statusClose && this.Opacity > 0)
            {
                this.Opacity -= 0.25;
            }
            else
            {
                this.Close();
            }
        }
    }
}
