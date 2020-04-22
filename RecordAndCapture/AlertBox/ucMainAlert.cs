using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RecordAndCapture;

namespace AlertBox.AlertBox
{
    public partial class ucMainAlert : UserControl
    {
        public ucMainAlert()
        {
            InitializeComponent();
        }

        public void SetDisplay(string caption, string content)
        {
            lbCaption.Text = caption;
            lbContent.Text = content;
        }
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            AlertBoxs.statusClose = false;
        }
    }
}
