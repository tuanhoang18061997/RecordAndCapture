using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Video.FFMPEG;
using AForge.Imaging.Filters;
using SweetAlertSharp;
using Tulpep.NotificationWindow;
using System.Windows.Controls;

namespace RecordAndCapture
{
    public partial class Form1 : Form
    {
        //System.Timers.Timer t;
        //int minutes, second;
        public Form1()
        {
            InitializeComponent();
        }

        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        private VideoCaptureDeviceForm captureDevice;

        private Bitmap video;
        private VideoFileWriter FileWriter = new VideoFileWriter();
        //private SaveFileDialog saveAvi;

        private void Form1_Load(object sender, EventArgs e)
        {

            /*
             * Tạo thời digital timer và set up thời gian là +1s 1 lần
             */
            //t = new System.Timers.Timer();
            //t.Interval = 1000;
            //t.Elapsed += T_Elapsed;

            /*
             End CounDown Timer
             */
            this.StartPosition = FormStartPosition.CenterScreen; // nằm giữa màn hình

            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice); //lọc ra những thiết bị camera hiện có

            foreach (FilterInfo Device in CaptureDevice)
            {
                cbNameDivice.Items.Add(Device.Name); // thêm vào cb những camera được kết nối
            }
            cbNameDivice.SelectedIndex = 0; // chọn camera đầu tiên

            FinalFrame = new VideoCaptureDevice();

            captureDevice = new VideoCaptureDeviceForm();

            FileWriter = new VideoFileWriter();

            // Check btnCapture and btnRecord
            if (string.IsNullOrEmpty(txtPatientID.Text))
            {
                button1.Enabled = false;
            }
            txtPatientID.TextChanged += txtPatientID_TextChanged;
        }

        private void txtPatientID_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = !string.IsNullOrEmpty(txtPatientID.Text);
        }

        //private void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    Invoke(new Action(() =>
        //    {
        //        second += 1;
        //        if (second == 60)
        //        {
        //            second = 0;
        //            minutes += 1;
        //        }
        //        txtResultCountTime.Text = string.Format("{0}:{1}", minutes.ToString().PadLeft(2, '0'), second.ToString().PadLeft(2, '0'));
        //    }));
        //}

        #region button Turn on Camera
        private void button1_Click(object sender, EventArgs e)
        {
            txtPatientID.Enabled = false;
            if (captureDevice.ShowDialog(this) == DialogResult.OK)
            {
                FinalFrame = captureDevice.VideoDevice;
                FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
                FinalFrame.Start();
            }
            // FinalFrame = new VideoCaptureDevice(CaptureDevice[cbNameDivice.SelectedIndex].MonikerString);

        }

        #endregion

        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                if (btnStop.Text == "Lưu")
                {
                    video = (Bitmap)eventArgs.Frame.Clone();
                    CameraBox.Image = (Bitmap)eventArgs.Frame.Clone();

                    FileWriter.WriteVideoFrame(video);
                }
                else
                {
                    video = (Bitmap)eventArgs.Frame.Clone();
                    CameraBox.Image = (Bitmap)eventArgs.Frame.Clone();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "New Frame", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            /* ^
            * Here it cannot convert implicitly from System.Drawing.Bitmap to
            * System.Windows.Media.ImageSource
            */
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FinalFrame == null)
            { return; }
            if (FinalFrame.IsRunning)
            {
                this.FinalFrame.Stop();
                FileWriter.Close();
            }
        }

        #region button Capture Picture
        private void btnCaptureImage_Click(object sender, EventArgs e)
        {

            if (CameraBox.Image == null)
            {
                MessageBox.Show("Vui lòng bật máy ảnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                CopyBox.Image = (Bitmap)CameraBox.Image.Clone();
            }
        }
        #endregion

        #region button Save Picture
        private void btnSave_Click(object sender, EventArgs e)
        {
            //string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string projectPath = Path.GetTempPath();                    // C:\Users\DELL\AppData\Local\Temp         
            string folderName = Path.Combine(projectPath, "Images");    // C:\Users\DELL\AppData\Local\Temp\Images
            folderName = Path.Combine(folderName, txtPatientID.Text);   // C:\Users\DELL\AppData\Local\Temp\Images\PatientID
            try
            {
                if (txtPatientID == null)
                {
                    MessageBox.Show("Vui lòng nhập Patient ID!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (!Directory.Exists(folderName) && txtPatientID.Text != null)
                    {

                        System.IO.Directory.CreateDirectory(folderName);


                        if (CopyBox.Image == null)
                        {
                            MessageBox.Show("Không có ảnh để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            CopyBox.Image.Save(folderName + @"\" + "IMG_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".jpg", ImageFormat.Jpeg);
                            SweetAlert.Show("Thông báo", "Lưu thành công!");
                        }
                    }
                    else
                    {

                        if (CopyBox.Image == null)
                        {
                            MessageBox.Show("Không có ảnh để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {

                            CopyBox.Image.Save(folderName + @"\" + "IMG" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".jpg", ImageFormat.Jpeg);

                            SweetAlert.Show("Thông báo", "Lưu thành công!");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region button Reset

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtPatientID.Text = "";
            txtPatientID.Enabled = true;
            //txtResultCountTime.Text = "00:00";
            if (FinalFrame == null)
            { return; }
            if (FinalFrame.IsRunning)
            {
                this.FinalFrame.Stop();
                FileWriter.Close();
                CameraBox.Image = null;
                CopyBox.Image = null;
            }
        }

        #endregion

        #region button Record Video

        private void btnRecord_Click(object sender, EventArgs e)
        {
            string projectPath = Path.GetTempPath();
            string folderName = Path.Combine(projectPath, "Videos");
            folderName = Path.Combine(folderName, txtPatientID.Text);
            try
            {
                if (!Directory.Exists(folderName))
                {
                    System.IO.Directory.CreateDirectory(folderName);

                    if (CameraBox.Image == null)
                    {
                        MessageBox.Show("Vui lòng bật máy ảnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (btnStop.Text == "Lưu")
                        {
                            MessageBox.Show("Vui lòng lưu video đang quay trước khi thao tác quay video tiếp theo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //saveAvi = new SaveFileDialog();
                            //saveAvi.Filter = "Mp4 Files (*.mp4)|*.mp4";

                            //if (saveAvi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            //{
                            int h = captureDevice.VideoDevice.VideoResolution.FrameSize.Height;
                            int w = captureDevice.VideoDevice.VideoResolution.FrameSize.Width;

                            FileWriter.Open(folderName + @"\" + "Video_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".mp4", w, h, 15, VideoCodec.MPEG4, 200000);
                            FileWriter.WriteVideoFrame(video);
                            btnStop.BackColor = Color.Red;
                            btnStop.Text = "Lưu";
                           
                            //}
                        }
                    }
                }
                else
                {
                    if (CameraBox.Image == null)
                    {
                        MessageBox.Show("Vui lòng bật máy ảnh!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (btnStop.Text == "Lưu")
                        {
                            MessageBox.Show("Vui lòng lưu video đang quay trước khi thao tác quay video tiếp theo!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //saveAvi = new SaveFileDialog();
                            //saveAvi.Filter = "Mp4 Files (*.mp4)|*.mp4";

                            //if (saveAvi.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            //{
                            int h = captureDevice.VideoDevice.VideoResolution.FrameSize.Height;
                            int w = captureDevice.VideoDevice.VideoResolution.FrameSize.Width;

                            FileWriter.Open(folderName + @"\" + "Video_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".mp4", w, h, 15, VideoCodec.MPEG4, 200000);
                            FileWriter.WriteVideoFrame(video);
                            btnStop.BackColor = Color.Red;
                            btnStop.Text = "Lưu";
                            MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //}
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Record", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region button Save Video
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnStop.Text == "Lưu")
                {
                    MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnStop.Text = "Dừng";
                    btnStop.BackColor = Color.Empty;
                    if (FinalFrame == null) { return; }
                    if (FinalFrame.IsRunning)
                    {
                        FileWriter.Close();
                        CameraBox.Image = null;
                    }
                }
                else
                {
                    this.FinalFrame.Stop();
                    FileWriter.Close();
                    CameraBox.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Stop Video", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        #endregion

        #region Open Image File
        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            OpenPicture_Form formPicture = new OpenPicture_Form();
            formPicture.ShowDialog();

        }


        #endregion

        #region Open Video Form
        private void btnOpenVideo_Click(object sender, EventArgs e)
        {

            // Create form Video and show it. Wait for form 2 to close before continue
            OpenVideo_Form newForm = new OpenVideo_Form();
            newForm.ShowDialog();

            // show this form after form 2 close
            this.WindowState = FormWindowState.Normal;
        }
        #endregion
    }
}