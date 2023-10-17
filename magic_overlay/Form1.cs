// overlay.Form1
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using magic_overlay;

namespace magic_overlay
{

    public class Form1 : Form
    {
        public class UserImportDLL
        {
            [DllImport("User32")]
            public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

            [DllImport("User32")]
            public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        }

        private double res = 1.0;

        private Form form2 = new Form();

        private IContainer components = null;

        private PictureBox pictureBox1;

        private Button button1;

        private Button button2;

        private Button button3;

        private Label label1;

        private Label label2;

        private Label label3;

        private Label label4;

        private TrackBar trackBar1;

        private TrackBar trackBar2;

        private TrackBar trackBar3;

        private CheckBox checkBox1;

        private Button button4;

        [DllImport("User32")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("User32")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public Form1()
        {
            InitializeComponent();
            form2.TopMost = true;
            label4.Text = "圖片大小: " + trackBar2.Value + "x" + trackBar3.Value + "px";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            base.LocationChanged += Form1_LocationChanged;
            base.SizeChanged += Form1_LocationChanged;
            Form1_LocationChanged(sender, e);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int initialStyle = UserImportDLL.GetWindowLong(form2.Handle, -20);
            SetWindowLong(form2.Handle, -20, initialStyle | 0x80000 | 0x20);
            form2.Opacity = (double)trackBar1.Value / 100.0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form2.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form2.Show();
            form2.TransparencyKey = form2.BackColor;
            form2.FormBorderStyle = FormBorderStyle.None;
            int initialStyle = UserImportDLL.GetWindowLong(form2.Handle, -20);
            SetWindowLong(form2.Handle, -20, initialStyle | 0x80000 | 0x20);
            PositionForm2(base.Left - trackBar2.Value, base.Top);
            form2.Width = trackBar2.Value;
            form2.Height = trackBar3.Value;
        }

        private void PositionForm2(int fLeft, int fTop)
        {
            form2.Location = new Point(fLeft, fTop);
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            PositionForm2(base.Left - trackBar2.Value, base.Top);
            form2.Width = trackBar2.Value;
            form2.Height = trackBar3.Value;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string img_file = string.Empty;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|All files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                img_file = dialog.FileName;
                int img_w = Image.FromFile(img_file).Width;
                int img_h = Image.FromFile(img_file).Height;
                res = Convert.ToDouble(img_h) / Convert.ToDouble(img_w);
                if (res >= 1.0)
                {
                    trackBar3.Value = 700;
                    trackBar2.Value = Convert.ToInt32(700.0 / res);
                    trackBar3.Maximum = Convert.ToInt32(1000.0 * res);
                }
                else
                {
                    trackBar2.Value = 700;
                    trackBar3.Value = Convert.ToInt32(700.0 * res);
                    trackBar2.Maximum = Convert.ToInt32(1000.0 / res);
                }
                pictureBox1.Image = Image.FromFile(img_file);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                form2.BackgroundImage = Image.FromFile(img_file);
                form2.BackgroundImageLayout = ImageLayout.Stretch;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject() == null)
            {
                return;
            }
            IDataObject data = Clipboard.GetDataObject();
            if (data.GetDataPresent(DataFormats.Bitmap))
            {
                Bitmap myBitmap = (Bitmap)data.GetData(DataFormats.Bitmap, autoConvert: true);
                myBitmap.MakeTransparent(Color.White);
                pictureBox1.Image = myBitmap;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                form2.BackgroundImage = myBitmap;
                form2.BackgroundImageLayout = ImageLayout.Stretch;
                int img_w = myBitmap.Width;
                int img_h = myBitmap.Height;
                res = Convert.ToDouble(img_h) / Convert.ToDouble(img_w);
                if (res >= 1.0)
                {
                    trackBar3.Value = 700;
                    trackBar2.Value = Convert.ToInt32(700.0 / res);
                    trackBar3.Maximum = Convert.ToInt32(1000.0 * res);
                }
                else
                {
                    trackBar2.Value = 700;
                    trackBar3.Value = Convert.ToInt32(700.0 * res);
                    trackBar2.Maximum = Convert.ToInt32(1000.0 / res);
                }
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (Convert.ToInt32((double)trackBar2.Value * res) > trackBar3.Maximum)
                {
                    trackBar3.Value = trackBar3.Maximum;
                }
                else
                {
                    trackBar3.Value = Convert.ToInt32((double)trackBar2.Value * res);
                }
                form2.Height = trackBar3.Value;
            }
            form2.Width = trackBar2.Value;
            PositionForm2(base.Left - trackBar2.Value, base.Top);
            label4.Text = "圖片大小: " + trackBar2.Value + "x" + trackBar3.Value + "px";
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                if (Convert.ToInt32((double)trackBar3.Value / res) > trackBar2.Maximum)
                {
                    trackBar2.Value = trackBar2.Maximum;
                }
                else
                {
                    trackBar2.Value = Convert.ToInt32((double)trackBar3.Value / res);
                }
                form2.Width = trackBar2.Value;
            }
            form2.Height = trackBar3.Value;
            PositionForm2(base.Left - trackBar2.Value, base.Top);
            label4.Text = "圖片大小: " + trackBar2.Value + "x" + trackBar3.Value + "px";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            trackBar1 = new TrackBar();
            trackBar2 = new TrackBar();
            trackBar3 = new TrackBar();
            checkBox1 = new CheckBox();
            button4 = new Button();
            ((ISupportInitialize)pictureBox1).BeginInit();
            ((ISupportInitialize)trackBar1).BeginInit();
            ((ISupportInitialize)trackBar2).BeginInit();
            ((ISupportInitialize)trackBar3).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(250, 250);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(265, 78);
            button1.Name = "button1";
            button1.Size = new Size(98, 25);
            button1.TabIndex = 1;
            button1.Text = "隱藏";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(365, 78);
            button2.Name = "button2";
            button2.Size = new Size(98, 25);
            button2.TabIndex = 2;
            button2.Text = "顯示";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(3, 259);
            button3.Name = "button3";
            button3.Size = new Size(125, 25);
            button3.TabIndex = 3;
            button3.Text = "載入";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(265, 9);
            label1.Name = "label1";
            label1.Size = new Size(55, 15);
            label1.TabIndex = 8;
            label1.Text = "不透明度";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(265, 223);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 9;
            label2.Text = "垂直尺寸";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(265, 157);
            label3.Name = "label3";
            label3.Size = new Size(55, 15);
            label3.TabIndex = 10;
            label3.Text = "橫向尺寸";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(265, 131);
            label4.Name = "label4";
            label4.Size = new Size(31, 15);
            label4.TabIndex = 11;
            label4.Text = "尺寸";
            label4.Click += label4_Click;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(265, 41);
            trackBar1.Maximum = 100;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(204, 45);
            trackBar1.TabIndex = 12;
            trackBar1.Value = 100;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // trackBar2
            // 
            trackBar2.Location = new Point(265, 175);
            trackBar2.Maximum = 2000;
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(204, 45);
            trackBar2.TabIndex = 13;
            trackBar2.Scroll += trackBar2_Scroll;
            // 
            // trackBar3
            // 
            trackBar3.Location = new Point(265, 241);
            trackBar3.Maximum = 2000;
            trackBar3.Name = "trackBar3";
            trackBar3.Size = new Size(204, 45);
            trackBar3.TabIndex = 14;
            trackBar3.Scroll += trackBar3_Scroll;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(265, 109);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(74, 19);
            checkBox1.TabIndex = 15;
            checkBox1.Text = "維持比例";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged_1;
            // 
            // button4
            // 
            button4.Location = new Point(134, 259);
            button4.Name = "button4";
            button4.Size = new Size(125, 25);
            button4.TabIndex = 16;
            button4.Text = "載入剪貼簿";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(475, 298);
            Controls.Add(label2);
            Controls.Add(button4);
            Controls.Add(checkBox1);
            Controls.Add(trackBar3);
            Controls.Add(trackBar2);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(pictureBox1);
            Controls.Add(trackBar1);
            Name = "Form1";
            Text = "Overlay";
            Load += Form1_Load;
            ((ISupportInitialize)pictureBox1).EndInit();
            ((ISupportInitialize)trackBar1).EndInit();
            ((ISupportInitialize)trackBar2).EndInit();
            ((ISupportInitialize)trackBar3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}