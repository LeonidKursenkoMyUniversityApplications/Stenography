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
using System.Windows.Forms;

namespace Stenography
{
    public partial class Form1 : Form
    {
        private Bitmap _inImage;
        private Bitmap _outImage;

        public Form1()
        {
            InitializeComponent();
        }

        private void loadInputImageButton_Click(object sender, EventArgs e)
        {
            _inImage = LoadImage();
            if(_inImage == null) return;
            pictureBox1.Image = _inImage;
        }

        private Bitmap LoadImage()
        {
            openFileDialog1.Title = "Завантажте зображення";
            openFileDialog1.InitialDirectory = "..\\image\\";
            openFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            Bitmap image = null;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    image = new Bitmap(openFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Невдалося відкрити зображення: " + ex.Message);
                }
            }
            return image;
        }

        private string SaveImage(Bitmap image)
        {
            saveFileDialog1.Title = "Збережіть зображення";
            saveFileDialog1.InitialDirectory = "..\\image\\";
            saveFileDialog1.Filter = "bmp files (*.bmp)|*.bmp|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    image.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
                }
                    catch (Exception e)
                {
                    MessageBox.Show("Невдалося зберегти зображення");
                    return "";
                }
        }
            return saveFileDialog1.FileName;
        }

        private void loadOutputImageButton_Click(object sender, EventArgs e)
        {
            _outImage = LoadImage();
            if (_outImage == null) return;
            pictureBox2.Image = _outImage;
        }

        private void stenographyButton_Click(object sender, EventArgs e)
        {
            string sourceMessage = richTextBox1.Text;
            try
            {
                pictureBox2.Image = null;
                _outImage = null;
                Bitmap outImage = Stenography.Encrypt(_inImage, sourceMessage);
                string imageName = SaveImage(outImage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void getMessageButton_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox2.Text = Stenography.Decrypt(_outImage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
