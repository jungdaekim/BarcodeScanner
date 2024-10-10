using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagingToolkit.Barcode;
using BasselTech_CamCapture;

namespace WebCamBarcode
{
    public partial class Form1 : Form
    {
        Camera cam;
        Timer t;
        BackgroundWorker worker;
        Bitmap CapImage;
        public Form1()
        {
            InitializeComponent();

            t = new Timer();
            cam = new Camera(pictureBox1);
            worker = new BackgroundWorker();

            worker.DoWork += Worker_DoWork;
            t.Tick += T_Tick;
            t.Interval = 1;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            CapImage = cam.GetBitmap();
            if (CapImage != null && !worker.IsBusy)
                worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BarcodeDecoder decoder = new BarcodeDecoder();

            try
            {
                string decoded_text = decoder.Decode(CapImage).Text;
                MessageBox.Show(decoded_text);
            }
            catch (Exception)
            {

            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                cam.Start();
                t.Start();
                btnStop.Enabled = true;
                btnStart.Enabled = false;
            }
            catch (Exception ex)
            {
                cam.Stop();
                MessageBox.Show(ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            t.Stop();
            cam.Stop();
            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }
    }
}
