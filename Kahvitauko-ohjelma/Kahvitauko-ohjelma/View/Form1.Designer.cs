
namespace Kahvitauko_ohjelma
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnOpenTime = new Button();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            Tuuli = new Label();
            button1 = new Button();
            dateTimePicker1 = new DateTimePicker();
            reset = new Button();
            richTextBox1 = new RichTextBox();
            SuspendLayout();
            // 
            // btnOpenTime
            // 
            btnOpenTime.Location = new Point(10, 10);
            btnOpenTime.Margin = new Padding(2, 2, 2, 2);
            btnOpenTime.Name = "btnOpenTime";
            btnOpenTime.Size = new Size(90, 27);
            btnOpenTime.TabIndex = 0;
            btnOpenTime.Text = "Aika";
            btnOpenTime.UseVisualStyleBackColor = true;
            btnOpenTime.Click += btnOpenTime_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(104, 14);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(38, 20);
            label1.TabIndex = 1;
            label1.Text = "Aika";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 71);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(55, 20);
            label2.TabIndex = 2;
            label2.Text = "Lämpö";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(152, 71);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(60, 20);
            label3.TabIndex = 3;
            label3.Text = "Aurinko";
            // 
            // Tuuli
            // 
            Tuuli.AutoSize = true;
            Tuuli.Location = new Point(297, 71);
            Tuuli.Margin = new Padding(2, 0, 2, 0);
            Tuuli.Name = "Tuuli";
            Tuuli.Size = new Size(41, 20);
            Tuuli.TabIndex = 4;
            Tuuli.Text = "Tuuli";
            // 
            // button1
            // 
            button1.Location = new Point(10, 42);
            button1.Margin = new Padding(2, 2, 2, 2);
            button1.Name = "button1";
            button1.Size = new Size(90, 27);
            button1.TabIndex = 5;
            button1.Text = "Sää";
            button1.UseVisualStyleBackColor = true;
            button1.Click += FetchWeatherButton_Click;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(104, 44);
            dateTimePicker1.Margin = new Padding(2, 2, 2, 2);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(241, 27);
            dateTimePicker1.TabIndex = 6;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // reset
            // 
            reset.Location = new Point(10, 323);
            reset.Margin = new Padding(2, 2, 2, 2);
            reset.Name = "reset";
            reset.Size = new Size(90, 27);
            reset.TabIndex = 8;
            reset.Text = "reset";
            reset.UseVisualStyleBackColor = true;
            reset.Click += reset_Click;
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(104, 94);
            richTextBox1.Margin = new Padding(2, 2, 2, 2);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(527, 258);
            richTextBox1.TabIndex = 10;
            richTextBox1.Text = "";
            richTextBox1.Click += richTextBox1_Load;
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(640, 360);
            Controls.Add(richTextBox1);
            Controls.Add(reset);
            Controls.Add(dateTimePicker1);
            Controls.Add(button1);
            Controls.Add(Tuuli);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnOpenTime);
            Margin = new Padding(2, 2, 2, 2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOpenTime;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label Tuuli;
        private Button button1;
        private DateTimePicker dateTimePicker1;
        private Button reset;
        private RichTextBox richTextBox1;
    }
}
