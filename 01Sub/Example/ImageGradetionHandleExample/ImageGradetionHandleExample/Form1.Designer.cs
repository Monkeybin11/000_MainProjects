namespace ImageGradetionHandleExample
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
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnLoad = new System.Windows.Forms.Button();
			this.imageBox1 = new Emgu.CV.UI.ImageBox();
			this.imageBox3 = new Emgu.CV.UI.ImageBox();
			this.btnShowOriginal = new System.Windows.Forms.Button();
			this.btnShowGrad = new System.Windows.Forms.Button();
			this.btnRes = new System.Windows.Forms.Button();
			this.btnScale = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.nudNormal = new System.Windows.Forms.NumericUpDown();
			this.nudGamma = new System.Windows.Forms.NumericUpDown();
			this.btnResscale = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudNormal)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudGamma)).BeginInit();
			this.SuspendLayout();
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(94, 12);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(76, 35);
			this.btnStart.TabIndex = 0;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(12, 12);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(76, 35);
			this.btnLoad.TabIndex = 1;
			this.btnLoad.Text = "Load";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// imageBox1
			// 
			this.imageBox1.Location = new System.Drawing.Point(24, 62);
			this.imageBox1.Name = "imageBox1";
			this.imageBox1.Size = new System.Drawing.Size(847, 838);
			this.imageBox1.TabIndex = 2;
			this.imageBox1.TabStop = false;
			// 
			// imageBox3
			// 
			this.imageBox3.Location = new System.Drawing.Point(877, 62);
			this.imageBox3.Name = "imageBox3";
			this.imageBox3.Size = new System.Drawing.Size(858, 838);
			this.imageBox3.TabIndex = 4;
			this.imageBox3.TabStop = false;
			// 
			// btnShowOriginal
			// 
			this.btnShowOriginal.Location = new System.Drawing.Point(775, 21);
			this.btnShowOriginal.Name = "btnShowOriginal";
			this.btnShowOriginal.Size = new System.Drawing.Size(45, 35);
			this.btnShowOriginal.TabIndex = 6;
			this.btnShowOriginal.Text = "Ori";
			this.btnShowOriginal.UseVisualStyleBackColor = true;
			this.btnShowOriginal.Click += new System.EventHandler(this.btnShowOriginal_Click);
			// 
			// btnShowGrad
			// 
			this.btnShowGrad.Location = new System.Drawing.Point(826, 21);
			this.btnShowGrad.Name = "btnShowGrad";
			this.btnShowGrad.Size = new System.Drawing.Size(45, 35);
			this.btnShowGrad.TabIndex = 8;
			this.btnShowGrad.Text = "Grad";
			this.btnShowGrad.UseVisualStyleBackColor = true;
			this.btnShowGrad.Click += new System.EventHandler(this.btnShowGrad_Click);
			// 
			// btnRes
			// 
			this.btnRes.Location = new System.Drawing.Point(1637, 21);
			this.btnRes.Name = "btnRes";
			this.btnRes.Size = new System.Drawing.Size(45, 35);
			this.btnRes.TabIndex = 9;
			this.btnRes.Text = "Res";
			this.btnRes.UseVisualStyleBackColor = true;
			this.btnRes.Click += new System.EventHandler(this.btnRes_Click);
			// 
			// btnScale
			// 
			this.btnScale.Location = new System.Drawing.Point(1688, 21);
			this.btnScale.Name = "btnScale";
			this.btnScale.Size = new System.Drawing.Size(45, 35);
			this.btnScale.TabIndex = 10;
			this.btnScale.Text = "Scale";
			this.btnScale.UseVisualStyleBackColor = true;
			this.btnScale.Click += new System.EventHandler(this.btnScale_Click);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(176, 12);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(78, 35);
			this.btnSave.TabIndex = 11;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(620, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 13);
			this.label1.TabIndex = 14;
			this.label1.Text = "Gamma";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(470, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 13);
			this.label2.TabIndex = 15;
			this.label2.Text = "Normal";
			// 
			// nudNormal
			// 
			this.nudNormal.Location = new System.Drawing.Point(437, 30);
			this.nudNormal.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nudNormal.Name = "nudNormal";
			this.nudNormal.Size = new System.Drawing.Size(120, 20);
			this.nudNormal.TabIndex = 16;
			// 
			// nudGamma
			// 
			this.nudGamma.Location = new System.Drawing.Point(601, 30);
			this.nudGamma.Name = "nudGamma";
			this.nudGamma.Size = new System.Drawing.Size(120, 20);
			this.nudGamma.TabIndex = 17;
			// 
			// btnResscale
			// 
			this.btnResscale.Location = new System.Drawing.Point(306, 12);
			this.btnResscale.Name = "btnResscale";
			this.btnResscale.Size = new System.Drawing.Size(78, 35);
			this.btnResscale.TabIndex = 18;
			this.btnResscale.Text = "Rescale";
			this.btnResscale.UseVisualStyleBackColor = true;
			this.btnResscale.Click += new System.EventHandler(this.btnResscale_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1745, 945);
			this.Controls.Add(this.btnResscale);
			this.Controls.Add(this.nudGamma);
			this.Controls.Add(this.nudNormal);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnScale);
			this.Controls.Add(this.btnRes);
			this.Controls.Add(this.btnShowGrad);
			this.Controls.Add(this.btnShowOriginal);
			this.Controls.Add(this.imageBox3);
			this.Controls.Add(this.imageBox1);
			this.Controls.Add(this.btnLoad);
			this.Controls.Add(this.btnStart);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imageBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudNormal)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudGamma)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnLoad;
		private Emgu.CV.UI.ImageBox imageBox1;
		private Emgu.CV.UI.ImageBox imageBox3;
		private System.Windows.Forms.Button btnShowOriginal;
		private System.Windows.Forms.Button btnShowGrad;
		private System.Windows.Forms.Button btnRes;
		private System.Windows.Forms.Button btnScale;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown nudNormal;
		private System.Windows.Forms.NumericUpDown nudGamma;
		private System.Windows.Forms.Button btnResscale;
	}
}

