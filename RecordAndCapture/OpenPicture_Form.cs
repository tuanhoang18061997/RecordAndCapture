using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
using AForge.Imaging.Filters;
using System.Drawing.Imaging;

namespace RecordAndCapture
{
    public partial class OpenPicture_Form : Form
    {
        public OpenPicture_Form()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            listImage.View = View.Details;

            listImage.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        //private void populate()
        //{
        //    using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true, ValidateNames = true, Filter = "JPG|*.jpg|JPEG|*.jpeg|PNG|*.png" })
        //    {
        //        if (ofd.ShowDialog() == DialogResult.OK)
        //        {
        //            //List<MediaFile> imageList = new List<MediaFile>();
        //            string[] files = ofd.FileNames;

        //            ImageList imgs = new ImageList();
        //            imgs.ImageSize = new Size(50, 50);
        //            imgs.ColorDepth = ColorDepth.Depth32Bit;

        //            foreach (string fileName in files)
        //            {
        //                listImage.Items.Add(Image.FromFile(fileName));
        //                //FileInfo fi = new FileInfo(fileName);
        //                //WebClient w = new WebClient();
        //                ////byte[] imageByte = w.DownloadData(imageList[fileName]);
        //                //MemoryStream stream = new MemoryStream(fi);

        //                //Image im = Image.FromStream(stream);
        //                //imgs.Images.Add(im);

        //                //Uri uri = new Uri(imageList[i]);

        //                //listImage.Items.Add(uri.Segments.Last(), fi);

        //            }
        //            for (int i = 0; i < imageList.Count; i++)
        //            {

        //            }
        //            listImage.LargeImageList = imgs;

        //        }
        //    }
        //}

        List<string> fileNames = new List<string>();

        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            fileNames.Clear();
            ImageList imageList = new ImageList();
            //for (int i = listImage.SelectedItems.Count - 1; i >= 0; i--)
            //{
            //    ListViewItem itm = listImage.SelectedItems[i];
            //    listImage.Items[itm.Index].Remove();
            //}
            if (TrackingImage_Box.Image != null)
            {
                lblPathFileImage.Text = "";
                TrackingImage_Box.Image.Dispose();
                TrackingImage_Box.Image = null;

            }
            if (ReviewPicture_Box_.Image != null)
            {
                lblPathFileImage.Text = "";

                ReviewPicture_Box_.Image.Dispose();
                ReviewPicture_Box_.Image = null;
            }

            this.listImage.Items.Clear();
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = true, ValidateNames = true, Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {

                    listImage.View = View.LargeIcon;

                    imageList.ImageSize = new Size(50, 50);
                    imageList.ColorDepth = ColorDepth.Depth32Bit;

                    foreach (string fileName in ofd.FileNames)
                    {
                        FileInfo fi = new FileInfo(fileName);
                        fileNames.Add(fi.FullName);

                    }
                    for (int c = 0; c < ofd.FileNames.Length; c++)
                    {
                        Image i = Image.FromFile(ofd.FileNames[c].ToString());
                        Image img = i.GetThumbnailImage(100, 100, null, new IntPtr());
                        imageList.Images.Add(new Bitmap(img));
                        listImage.LargeImageList = imageList;
                    }
                    for (int j = 0; j < imageList.Images.Count; j++)
                    {
                        ListViewItem lstItem = new ListViewItem();

                        lstItem.ImageIndex = j;

                        listImage.Items.Add(lstItem);

                    }

                }
            }
        }

        private void listImage_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                if (listImage.SelectedItems.Count > 0)
                {
                    ReviewPicture_Box_.Image = Image.FromFile(fileNames[listImage.FocusedItem.ImageIndex]);
                    if (lblPathFileImage.Text != null)
                    {
                        lblPathFileImage.Text = "";
                    }
                    lblPathFileImage.Text += Path.GetFullPath(fileNames[listImage.FocusedItem.Index]);
                    trackBar1.Value = 0;
                }
            }
            catch (Exception)
            {

            }

        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = listImage.SelectedItems.Count - 1; i >= 0; i--)
            {
                listImage.Items.RemoveAt(listImage.SelectedIndices[i]);
            }

            if (TrackingImage_Box.Image != null || ReviewPicture_Box_.Image != null)
            {
                TrackingImage_Box.Image.Dispose();
                TrackingImage_Box.Image = null;

                ReviewPicture_Box_.Image.Dispose();
                ReviewPicture_Box_.Image = null;
            }

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                Bitmap image = (Bitmap)ReviewPicture_Box_.Image;

                RotateBilinear ro = new RotateBilinear(trackBar1.Value, true);
                if (image != null)
                {
                    trackBar1.Enabled = true;
                    Bitmap image2 = ro.Apply(image);
                    TrackingImage_Box.Image = image2;
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("There was an error." + "Check the path to the bitmap.");
            }
        }

        private void btnDeleteFile_Click(object sender, EventArgs e)
        {

            if (ReviewPicture_Box_.Image != null && TrackingImage_Box.Image != null)
            {
                MessageBox.Show("Xóa ảnh trong 2 khung trước");
            }
            else
            {
                var result = MessageBox.Show("Bạn có chắc xóa file " + lblPathFileImage.Text, "Thông báo",
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    if (File.Exists(lblPathFileImage.Text))
                    {
                        File.Delete(lblPathFileImage.Text);
                        lblPathFileImage.Text = "";

                    }

                }
            }
        }
    }
}
// parentDir = System.IO.Path.GetDirectoryName(file);
// listImage.Items.Add(file);