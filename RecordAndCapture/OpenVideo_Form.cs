using AlertBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecordAndCapture
{
    public partial class OpenVideo_Form : Form
    {
        public OpenVideo_Form()
        {
            InitializeComponent();
        }

        private void OpenVideo_Form_Load(object sender, EventArgs e)
        {

            listFile.ValueMember = "Path";
            listFile.DisplayMember = "FileName";
            if (count <= 0)
            {
                listFile.Enabled = false;
            }

        }

        static BindingList<MediaFile> files = new BindingList<MediaFile>();
        int count = files.Count;
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            files.Clear();

            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true, ValidateNames = true, Filter = "MP4|*.mp4|AVI|*.avi|MWV|*.wmv|WAV|*.wav|MP3|*.mp3|MKV|*.mkv" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (files.Count + 1 > 0)
                    {
                        listFile.Enabled = true;
                        removeToolStripMenuItem.Enabled = true;
                    }
                    foreach (string fileName in ofd.FileNames)
                    {
                        FileInfo fi = new FileInfo(fileName);
                        files.Add(new MediaFile() { FileName = Path.GetFileNameWithoutExtension(fi.FullName), Path = fi.FullName });
                    }
                    listFile.DataSource = new BindingSource(files, null);
                    listFile.ValueMember = "Path";
                    listFile.DisplayMember = "FileName";
                }
            }
        }

        private void listFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            MediaFile file = listFile.SelectedItem as MediaFile;
            if (file != null)
            {
                axWindowsMediaPlayer.URL = file.Path;
                axWindowsMediaPlayer.Ctlcontrols.play();
                lblPathFileVideo.Text = file.Path;
            }
        }

        private void btnDeleteVideo_Click(object sender, EventArgs e)
        {

            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (File.Exists(lblPathFileVideo.Text))
            {
                int selectedIndex = listFile.SelectedIndex;
                // Remove the item in the List.

                var result = MessageBox.Show("Bạn có chắc xóa file " + lblPathFileVideo.Text, "Thông báo",
                                  MessageBoxButtons.YesNo,
                                  MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    File.Delete(lblPathFileVideo.Text);
                    axWindowsMediaPlayer.Ctlcontrols.stop();
                    files.RemoveAt(selectedIndex);
                    lblPathFileVideo.Text = "";
                    AlertBoxs.Show("Thông báo", "Xóa thành công!", (int)AlertBoxs.TypeAlert.AlertSuccess);
                }
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedIndex = listFile.SelectedIndex;
            if (selectedIndex == -1)
            {
                AlertBoxs.Show("Thông báo", "Không có hình ảnh trong danh sách", (int)AlertBoxs.TypeAlert.AlertInfo);
            }
            else
            {
                files.RemoveAt(selectedIndex);
                axWindowsMediaPlayer.Ctlcontrols.stop();
                AlertBoxs.Show("Thông báo", "Đã di chuyển file khỏi danh sách" , (int)AlertBoxs.TypeAlert.AlertInfo);
                listFile.DataSource = files;
            }
        }
    }
}
