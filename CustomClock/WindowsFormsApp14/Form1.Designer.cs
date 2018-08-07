namespace WindowsFormsApp14
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.clockBase1 = new WindowsFormsApp14.ClockBase();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // clockBase1
            // 
            this.clockBase1.Center = new System.Drawing.Point(210, 210);
            this.clockBase1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clockBase1.HourColor = System.Drawing.Color.Yellow;
            this.clockBase1.HourPoints = ((System.Collections.Generic.List<System.Drawing.Point>)(resources.GetObject("clockBase1.HourPoints")));
            this.clockBase1.Location = new System.Drawing.Point(0, 0);
            this.clockBase1.MinuteColor = System.Drawing.Color.Red;
            this.clockBase1.MinutePoints = ((System.Collections.Generic.List<System.Drawing.Point>)(resources.GetObject("clockBase1.MinutePoints")));
            this.clockBase1.Name = "clockBase1";
            this.clockBase1.Radius = 210;
            this.clockBase1.SecondColor = System.Drawing.Color.Lime;
            this.clockBase1.SecondPoints = ((System.Collections.Generic.List<System.Drawing.Point>)(resources.GetObject("clockBase1.SecondPoints")));
            this.clockBase1.Size = new System.Drawing.Size(420, 421);
            this.clockBase1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 421);
            this.Controls.Add(this.clockBase1);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Analog Clock";
            this.ResumeLayout(false);

        }

        #endregion

        private ClockBase clockBase1;
        private System.Windows.Forms.Timer timer1;
    }
}

