
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace VisualCryptography
{
    public partial class FormMain : Form
    {
        private Size IMAGE_SIZE = new Size(437, 106);
        private const int GENERATE_IMAGE_COUNT = 2;

        private Bitmap[] m_EncryptedImages;

        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            if (textBoxInput.Text != "")
            {
                if (m_EncryptedImages != null)
                {
                    for (int i = m_EncryptedImages.Length - 1; i > 0; i--)
                    {
                        m_EncryptedImages[i].Dispose();
                    }
                    Array.Clear(m_EncryptedImages, 0, m_EncryptedImages.Length);
                }

                m_EncryptedImages = GenerateImage(textBoxInput.Text);

                panelCanvas.Invalidate();
            }
        }

        private Bitmap[] GenerateImage(string inputText)
        {
            Bitmap finalImage = new Bitmap(IMAGE_SIZE.Width, IMAGE_SIZE.Height);
            Bitmap finalImage1 = new Bitmap(IMAGE_SIZE.Width, IMAGE_SIZE.Height);
            Bitmap tempImage = new Bitmap(IMAGE_SIZE.Width / 2, IMAGE_SIZE.Height);
            Bitmap imagecu;
            Bitmap[] image = new Bitmap[GENERATE_IMAGE_COUNT];
            
            Random rand = new Random();
            SolidBrush brush = new SolidBrush(Color.Black);
            Point mid = new Point(IMAGE_SIZE.Width / 2, IMAGE_SIZE.Height / 2);

            Graphics g = Graphics.FromImage(finalImage);
            Graphics gtemp = Graphics.FromImage(tempImage);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Font font = new Font("Times New Roman", 48);
            Color fontColor;
            Color fontColor1;
            Color fontColor2;
            Color fontColor3;


            g.DrawString(inputText, font, brush, mid, sf);
            gtemp.DrawImage(finalImage, 0, 0, tempImage.Width, tempImage.Height);


            for (int i = 0; i < image.Length; i++)
            {
                image[i] = new Bitmap(IMAGE_SIZE.Width, IMAGE_SIZE.Height);
            }

            
            int index = -1;
            int width = tempImage.Width;
            int height = tempImage.Height;
            
          
            for (int x = 0; x < width; x += 1)
            {
                for (int y = 0; y < height; y += 1)
                {
                    fontColor = tempImage.GetPixel(x, y);
                    index = rand.Next(image.Length);
                    if (fontColor.Name == Color.Empty.Name)
                    {
                        for (int i = 0; i < image.Length; i++)
                        {
                            if (index == 0)
                            {
                                image[i].SetPixel(x * 2, y, Color.Black);
                                image[i].SetPixel(x * 2 + 1, y, Color.Empty);
                            }
                            else
                            {
                                image[i].SetPixel(x * 2, y, Color.Empty);
                                image[i].SetPixel(x * 2 + 1, y, Color.Black);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < image.Length; i++)
                        {
                            if ((index + i) % image.Length == 0)
                            {
                                image[i].SetPixel(x * 2, y, Color.Black);
                                image[i].SetPixel(x * 2 + 1, y, Color.Empty);
                            }
                            else
                            {
                                image[i].SetPixel(x * 2, y, Color.Empty);
                                image[i].SetPixel(x * 2 + 1, y, Color.Black);
                            }
                        }
                    }
                }
            }
         
           image[0].Save(@"C:10.png");
           image[1].Save(@"C:11.png");
            
            Bitmap image0 = new Bitmap(@"10.png");
            Bitmap image1 = new Bitmap(@"11.png");
            finalImage1 = image0;

            for (int x = 0; x < image[0].Width; x += 1)
            {
                for (int y = 0; y < image[1].Height; y += 1)
                {
                    fontColor = image[0].GetPixel(x, y);
                    fontColor1 = image[1].GetPixel(x, y);
                    
                    if (fontColor.Name == "0"||fontColor1.Name == "0")
                    {
                        finalImage1.SetPixel(x, y, Color.Black);
                    }

                    else
                    {
                        finalImage1.SetPixel(x, y, Color.Empty);
                    }
                  
                }
            }
           
         
            finalImage1.Save(@"TEST1.bmp");
            brush.Dispose();
            tempImage.Dispose();
            finalImage.Dispose();
            finalImage1.Dispose();
            return image;
        }

        private void panelCanvas_Paint(object sender, PaintEventArgs e)
        {
            System.Console.WriteLine("hello");
            if (m_EncryptedImages != null)
            {
                Graphics g = e.Graphics;
                Rectangle rect = new Rectangle(0, 0, 0, 0);
                Color fontColor;
                Color fontColor1;
                
                for (int i = 0; i < m_EncryptedImages.Length; i++)
                {

                    rect.Size = m_EncryptedImages[i].Size;
                    g.DrawImage(m_EncryptedImages[i], rect);
                    rect.Y += m_EncryptedImages[i].Height + 5;
                }
                
                g.DrawLine(new Pen(new SolidBrush(Color.Black), 1), rect.Location, new Point(rect.Width, rect.Y));
                rect.Y += 5;

                for (int i = 0; i < m_EncryptedImages.Length; i++)
                {
                   
                        rect.Size = m_EncryptedImages[i].Size;
                        g.DrawImage(m_EncryptedImages[i], rect);
                        m_EncryptedImages[i].Dispose();
                }           
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_EncryptedImages == null)
            {
                MessageBox.Show("Please generate image first", this.Text);
                return;
            }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = Path.GetFileNameWithoutExtension(saveFileDialog1.FileName);
                string filePath = saveFileDialog1.FileName;
                string savePath = fileName;
                for (int i = 0; i < m_EncryptedImages.Length; i++)
                {
                    savePath = filePath.Replace(fileName, fileName + i);
                    m_EncryptedImages[i].Save(savePath, ImageFormat.Png);
                }
            }
        }
    }
}