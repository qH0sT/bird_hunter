using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KusAvcisi
{
    public partial class Form1 : Form
    {
        //KARNIZMIZ TEVECCÜH'E BU KADAR TOK MU???! BEN AÇIM!! https://www.youtube.com/watch?v=eyyrkQo2XtY
        public Form1()
        {
            InitializeComponent();
            InitializeResources();
            Nisan();
        }
        List<Kus> kuslar = new List<Kus>();
        Random rnd = new Random(Guid.NewGuid().GetHashCode());
        int mermi = 3;
        int puan = 0;
        int seviye = 1;
        public void Nisan()
        {
            Bitmap bmp = new Bitmap(ımageList1.Images[0],new Size(50,50));
            bmp.MakeTransparent();
            pictureBox2.Cursor = new Cursor(bmp.GetHicon());
        }
        class Kus
        {
            public int sayac = 0;
            PictureBox kus = new PictureBox();
            Form1 frm = null;
            public Kus(Form1 frm1)
            {
                frm = frm1;
                kus.Location = new Point(12, 44);
                //kus.BringToFront();
                kus.Name = "pictureBox1";
                kus.Size = new Size(55, 50);
                kus.ImageLocation = Environment.CurrentDirectory + "\\Res\\bird.gif";
                kus.SizeMode = PictureBoxSizeMode.Zoom;
                kus.TabIndex = 0;
                kus.TabStop = false;
                kus.BackColor = Color.Transparent;
                kus.Click += new EventHandler(vurus);
                frm1.pictureBox2.Controls.Add(kus);
            }
            private void vurus(object sender, EventArgs args)
            {
                if (frm.mermi > 0)
                {
                    //MessageBox.Show("Vurdun!!!");
                    SoundPlayer sp = new SoundPlayer(Environment.CurrentDirectory + "\\Res\\shot.wav");
                    sp.Play();
                    frm.puan += 5;
                    frm.label1.Text = "Puan: "+frm.puan.ToString();
                    frm.pictureBox2.Controls.Remove(kus);                    
                    frm.kuslar.Remove(this);
                    frm.mermi -= 1;
                    frm.mermi_ += 1;
                    frm.label2.Text = "Mermi: "+frm.mermi.ToString();
                    frm.label3.Text = "Kuş Sayısı: " + frm.kuslar.Count.ToString();
                    //frm.Text = frm.kuslar.Count.ToString();
                }
                else {
                MessageBox.Show("Mermin bitti :((","Mermi Sayısı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frm.timer1.Stop();
                    frm.timer2.Stop();
                    frm.pictureBox2.Controls.Clear();
                }
            }
            
            public void Hareket(int down, int up)
            {
                kus.Left += frm.rnd.Next(1,20); //Kuşun tekdüze hareket etmesini önlemek için Random sınıfı ile dalgalı bir uçuş sağlıyoruz.
                if (sayac <= down)
                {
                    kus.Top += 10;
                }
                else
                {
                    kus.Top -= 10;
                    if (sayac == up)
                    {
                        sayac = 0;
                    }
                }
                sayac++;
            }
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            /*
            if (mermi > 0)
            {
                mermi -= 1;
                label2.Text = mermi.ToString();
            }
            else
            {
                MessageBox.Show("Mermin yok :((");
            }
            */
        }

        private void timer1_Tick(object sender, EventArgs e) //kuşları hareket ettiren timer
        {
            foreach (Kus kusumuz in kuslar)
            {
                kusumuz.Hareket(15, 30);
            }
        }
        int kus_sayisi = 0;
        int mermi_ = 0;
        private async void timer2_Tick(object sender, EventArgs e)
        {
            if(kuslar.Count != 0)
            {
                MessageBox.Show("Kaybettin!!!","Süre Doldu",MessageBoxButtons.OK,MessageBoxIcon.Information);
                timer2.Stop();
                timer1.Stop();
            }
            else
            {
                //timer1.Stop();
                timer2.Stop();
                //MessageBox.Show("LEVEL");
                
                if (timer1.Interval > 10) { timer1.Interval -= 10; }
                if (timer2.Interval < 30000) { timer2.Interval += 10000; };
                int g = (kus_sayisi + 5);
                mermi = mermi_ + g + 1;
                mermi_ = 0;
                label2.Text = "Mermi: " + mermi.ToString();
                label4.Text = "Seviye: " + (seviye += 1);
                for (int j =0; j < g ; j++)
                {
                    Kus bird = new Kus(this);
                    kuslar.Add(bird);
                    kus_sayisi += 1;
                    
                    for (int k = 0; k < 5; k++)
                    {
                        bird.Hareket(15, 30);
                        await Task.Delay(250);
                    }
                    label3.Text = "Kuş Sayısı: " + kuslar.Count.ToString();
                }

                label3.Text = "Kuş Sayısı: " + kuslar.Count.ToString();
                //timer1.Start();
                timer2.Start();
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            for (int h = 0; h < 3; h++)
            {
                Kus bird = new Kus(this);
                kuslar.Add(bird);
                kus_sayisi += 1;
                for (int k = 0; k < 5; k++)
                {
                    bird.Hareket(15, 30);
                    await Task.Delay(250);
                }
            }
        }
        private void InitializeResources()
        {
            //label1.BackColor = System.Drawing.Color.Transparent;
            pictureBox2.ImageLocation = Environment.CurrentDirectory + "\\Res\\jungle_background.jpg";
            pictureBox1.Top = groupBox1.Top + 120;
            pictureBox1.ImageLocation = Environment.CurrentDirectory + "\\Res\\hunter_back.jpg";
            //pictureBox1.ImageLocation = Environment.CurrentDirectory + "\\Res\\jungle_background.jpg";
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (mermi > 0)
            {
                mermi -= 1;
                mermi_ += 1;
                label2.Text = "Mermi: "+ mermi.ToString();
                SoundPlayer sp = new SoundPlayer(Environment.CurrentDirectory + "\\Res\\shot.wav");
                sp.Play();
            }
            else
            {
                MessageBox.Show("Mermin bitti :((", "Mermi Sayısı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Stop();
                timer2.Stop();
                pictureBox2.Controls.Clear();
        
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            //pictureBox1.Left = e.X;
            //Avcıyı hareket ettiren event ama kasma yaptığı için iptal ettim. İsterseniz yorum işaretini kaldırıp aktif edebilirsiniz.
        }
    }
}
