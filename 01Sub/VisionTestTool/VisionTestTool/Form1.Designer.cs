namespace VisionTestTool
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
            this.imgBox = new Emgu.CV.UI.ImageBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Basic = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.Adv = new System.Windows.Forms.TabPage();
            this.Color = new System.Windows.Forms.TabPage();
            this.rtbGammaColor = new System.Windows.Forms.RichTextBox();
            this.rtbNormalizeColor = new System.Windows.Forms.RichTextBox();
            this.rtbThresColor = new System.Windows.Forms.RichTextBox();
            this.btnGammaColor = new System.Windows.Forms.Button();
            this.btnNormalizeColor = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnThresColor = new System.Windows.Forms.Button();
            this.colorConv = new System.Windows.Forms.TabPage();
            this.rtbRed = new System.Windows.Forms.RichTextBox();
            this.rtbGreen = new System.Windows.Forms.RichTextBox();
            this.btnConvColor = new System.Windows.Forms.Button();
            this.rtbblue = new System.Windows.Forms.RichTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.Modify = new System.Windows.Forms.TabPage();
            this.btnAllHStack = new System.Windows.Forms.Button();
            this.btnAllVStack = new System.Windows.Forms.Button();
            this.btnClearAllList = new System.Windows.Forms.Button();
            this.lblstatus = new System.Windows.Forms.Label();
            this.nudScale = new System.Windows.Forms.NumericUpDown();
            this.btnAllResize = new System.Windows.Forms.Button();
            this.btnAllCrop = new System.Windows.Forms.Button();
            this.btnAdd2List = new System.Windows.Forms.Button();
            this.btnLoadGrary = new System.Windows.Forms.Button();
            this.btnLoadColor = new System.Windows.Forms.Button();
            this.rtxLog = new System.Windows.Forms.RichTextBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNormalizeAll = new System.Windows.Forms.Button();
            this.nudNormalizeLevel = new System.Windows.Forms.NumericUpDown();
            this.btnCheckNorm = new System.Windows.Forms.Button();
            this.btnTestImgClear = new System.Windows.Forms.Button();
            this.btnTotal = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.Basic.SuspendLayout();
            this.Color.SuspendLayout();
            this.colorConv.SuspendLayout();
            this.Modify.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNormalizeLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // imgBox
            // 
            this.imgBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imgBox.Location = new System.Drawing.Point(331, 41);
            this.imgBox.Name = "imgBox";
            this.imgBox.Size = new System.Drawing.Size(665, 638);
            this.imgBox.TabIndex = 2;
            this.imgBox.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Basic);
            this.tabControl1.Controls.Add(this.Adv);
            this.tabControl1.Controls.Add(this.Color);
            this.tabControl1.Controls.Add(this.colorConv);
            this.tabControl1.Controls.Add(this.Modify);
            this.tabControl1.Location = new System.Drawing.Point(4, 19);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(325, 484);
            this.tabControl1.TabIndex = 3;
            // 
            // Basic
            // 
            this.Basic.Controls.Add(this.button1);
            this.Basic.Location = new System.Drawing.Point(4, 22);
            this.Basic.Name = "Basic";
            this.Basic.Padding = new System.Windows.Forms.Padding(3);
            this.Basic.Size = new System.Drawing.Size(317, 458);
            this.Basic.TabIndex = 0;
            this.Basic.Text = "Basic";
            this.Basic.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Threshold";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // Adv
            // 
            this.Adv.Location = new System.Drawing.Point(4, 22);
            this.Adv.Name = "Adv";
            this.Adv.Padding = new System.Windows.Forms.Padding(3);
            this.Adv.Size = new System.Drawing.Size(317, 458);
            this.Adv.TabIndex = 1;
            this.Adv.Text = "Adv";
            this.Adv.UseVisualStyleBackColor = true;
            // 
            // Color
            // 
            this.Color.Controls.Add(this.rtbGammaColor);
            this.Color.Controls.Add(this.rtbNormalizeColor);
            this.Color.Controls.Add(this.rtbThresColor);
            this.Color.Controls.Add(this.btnGammaColor);
            this.Color.Controls.Add(this.btnNormalizeColor);
            this.Color.Controls.Add(this.label3);
            this.Color.Controls.Add(this.label2);
            this.Color.Controls.Add(this.label1);
            this.Color.Controls.Add(this.btnThresColor);
            this.Color.Location = new System.Drawing.Point(4, 22);
            this.Color.Name = "Color";
            this.Color.Size = new System.Drawing.Size(317, 458);
            this.Color.TabIndex = 2;
            this.Color.Text = "Color";
            this.Color.UseVisualStyleBackColor = true;
            // 
            // rtbGammaColor
            // 
            this.rtbGammaColor.Location = new System.Drawing.Point(84, 171);
            this.rtbGammaColor.Name = "rtbGammaColor";
            this.rtbGammaColor.Size = new System.Drawing.Size(182, 21);
            this.rtbGammaColor.TabIndex = 29;
            this.rtbGammaColor.Text = "";
            // 
            // rtbNormalizeColor
            // 
            this.rtbNormalizeColor.Location = new System.Drawing.Point(84, 113);
            this.rtbNormalizeColor.Name = "rtbNormalizeColor";
            this.rtbNormalizeColor.Size = new System.Drawing.Size(182, 21);
            this.rtbNormalizeColor.TabIndex = 28;
            this.rtbNormalizeColor.Text = "";
            // 
            // rtbThresColor
            // 
            this.rtbThresColor.Location = new System.Drawing.Point(84, 51);
            this.rtbThresColor.Name = "rtbThresColor";
            this.rtbThresColor.Size = new System.Drawing.Size(182, 21);
            this.rtbThresColor.TabIndex = 27;
            this.rtbThresColor.Text = "";
            // 
            // btnGammaColor
            // 
            this.btnGammaColor.Location = new System.Drawing.Point(3, 169);
            this.btnGammaColor.Name = "btnGammaColor";
            this.btnGammaColor.Size = new System.Drawing.Size(75, 23);
            this.btnGammaColor.TabIndex = 14;
            this.btnGammaColor.Text = "Gamma";
            this.btnGammaColor.UseVisualStyleBackColor = true;
            this.btnGammaColor.Click += new System.EventHandler(this.btnGammaColor_Click);
            // 
            // btnNormalizeColor
            // 
            this.btnNormalizeColor.Location = new System.Drawing.Point(3, 111);
            this.btnNormalizeColor.Name = "btnNormalizeColor";
            this.btnNormalizeColor.Size = new System.Drawing.Size(75, 23);
            this.btnNormalizeColor.TabIndex = 7;
            this.btnNormalizeColor.Text = "Normalize";
            this.btnNormalizeColor.UseVisualStyleBackColor = true;
            this.btnNormalizeColor.Click += new System.EventHandler(this.btnNormalizeColor_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(251, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "R";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "G";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "B";
            // 
            // btnThresColor
            // 
            this.btnThresColor.Location = new System.Drawing.Point(3, 49);
            this.btnThresColor.Name = "btnThresColor";
            this.btnThresColor.Size = new System.Drawing.Size(75, 23);
            this.btnThresColor.TabIndex = 0;
            this.btnThresColor.Text = "Threshold";
            this.btnThresColor.UseVisualStyleBackColor = true;
            this.btnThresColor.Click += new System.EventHandler(this.btnThresColor_Click);
            // 
            // colorConv
            // 
            this.colorConv.Controls.Add(this.rtbRed);
            this.colorConv.Controls.Add(this.rtbGreen);
            this.colorConv.Controls.Add(this.btnConvColor);
            this.colorConv.Controls.Add(this.rtbblue);
            this.colorConv.Controls.Add(this.label10);
            this.colorConv.Controls.Add(this.label11);
            this.colorConv.Controls.Add(this.label12);
            this.colorConv.Location = new System.Drawing.Point(4, 22);
            this.colorConv.Name = "colorConv";
            this.colorConv.Size = new System.Drawing.Size(317, 458);
            this.colorConv.TabIndex = 3;
            this.colorConv.Text = "colorConv";
            this.colorConv.UseVisualStyleBackColor = true;
            // 
            // rtbRed
            // 
            this.rtbRed.Location = new System.Drawing.Point(143, 239);
            this.rtbRed.Name = "rtbRed";
            this.rtbRed.Size = new System.Drawing.Size(75, 75);
            this.rtbRed.TabIndex = 41;
            this.rtbRed.Text = "";
            // 
            // rtbGreen
            // 
            this.rtbGreen.Location = new System.Drawing.Point(143, 135);
            this.rtbGreen.Name = "rtbGreen";
            this.rtbGreen.Size = new System.Drawing.Size(75, 75);
            this.rtbGreen.TabIndex = 40;
            this.rtbGreen.Text = "";
            // 
            // btnConvColor
            // 
            this.btnConvColor.Location = new System.Drawing.Point(13, 14);
            this.btnConvColor.Name = "btnConvColor";
            this.btnConvColor.Size = new System.Drawing.Size(75, 23);
            this.btnConvColor.TabIndex = 39;
            this.btnConvColor.Text = "Conv";
            this.btnConvColor.UseVisualStyleBackColor = true;
            this.btnConvColor.Click += new System.EventHandler(this.btnConvColor_Click);
            // 
            // rtbblue
            // 
            this.rtbblue.Location = new System.Drawing.Point(143, 35);
            this.rtbblue.Name = "rtbblue";
            this.rtbblue.Size = new System.Drawing.Size(75, 75);
            this.rtbblue.TabIndex = 38;
            this.rtbblue.Text = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(174, 223);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(15, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "R";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(174, 119);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(15, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "G";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(174, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(14, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "B";
            // 
            // Modify
            // 
            this.Modify.Controls.Add(this.btnTotal);
            this.Modify.Controls.Add(this.btnTestImgClear);
            this.Modify.Controls.Add(this.btnCheckNorm);
            this.Modify.Controls.Add(this.nudNormalizeLevel);
            this.Modify.Controls.Add(this.btnNormalizeAll);
            this.Modify.Controls.Add(this.btnAllHStack);
            this.Modify.Controls.Add(this.btnAllVStack);
            this.Modify.Controls.Add(this.btnClearAllList);
            this.Modify.Controls.Add(this.lblstatus);
            this.Modify.Controls.Add(this.nudScale);
            this.Modify.Controls.Add(this.btnAllResize);
            this.Modify.Controls.Add(this.btnAllCrop);
            this.Modify.Controls.Add(this.btnAdd2List);
            this.Modify.Location = new System.Drawing.Point(4, 22);
            this.Modify.Name = "Modify";
            this.Modify.Size = new System.Drawing.Size(317, 458);
            this.Modify.TabIndex = 4;
            this.Modify.Text = "Modify";
            this.Modify.UseVisualStyleBackColor = true;
            // 
            // btnAllHStack
            // 
            this.btnAllHStack.Location = new System.Drawing.Point(3, 139);
            this.btnAllHStack.Name = "btnAllHStack";
            this.btnAllHStack.Size = new System.Drawing.Size(75, 23);
            this.btnAllHStack.TabIndex = 7;
            this.btnAllHStack.Text = "HStack";
            this.btnAllHStack.UseVisualStyleBackColor = true;
            this.btnAllHStack.Click += new System.EventHandler(this.btnAllHStack_Click);
            // 
            // btnAllVStack
            // 
            this.btnAllVStack.Location = new System.Drawing.Point(84, 139);
            this.btnAllVStack.Name = "btnAllVStack";
            this.btnAllVStack.Size = new System.Drawing.Size(75, 23);
            this.btnAllVStack.TabIndex = 6;
            this.btnAllVStack.Text = "VStack";
            this.btnAllVStack.UseVisualStyleBackColor = true;
            // 
            // btnClearAllList
            // 
            this.btnClearAllList.Location = new System.Drawing.Point(85, 3);
            this.btnClearAllList.Name = "btnClearAllList";
            this.btnClearAllList.Size = new System.Drawing.Size(75, 23);
            this.btnClearAllList.TabIndex = 5;
            this.btnClearAllList.Text = "PathClear";
            this.btnClearAllList.UseVisualStyleBackColor = true;
            this.btnClearAllList.Click += new System.EventHandler(this.btnClearAllList_Click);
            // 
            // lblstatus
            // 
            this.lblstatus.AutoSize = true;
            this.lblstatus.Location = new System.Drawing.Point(4, 29);
            this.lblstatus.Name = "lblstatus";
            this.lblstatus.Size = new System.Drawing.Size(35, 13);
            this.lblstatus.TabIndex = 4;
            this.lblstatus.Text = "label4";
            // 
            // nudScale
            // 
            this.nudScale.Location = new System.Drawing.Point(85, 110);
            this.nudScale.Name = "nudScale";
            this.nudScale.Size = new System.Drawing.Size(66, 20);
            this.nudScale.TabIndex = 3;
            // 
            // btnAllResize
            // 
            this.btnAllResize.Location = new System.Drawing.Point(4, 110);
            this.btnAllResize.Name = "btnAllResize";
            this.btnAllResize.Size = new System.Drawing.Size(75, 23);
            this.btnAllResize.TabIndex = 2;
            this.btnAllResize.Text = "Resize";
            this.btnAllResize.UseVisualStyleBackColor = true;
            this.btnAllResize.Click += new System.EventHandler(this.btnAllResize_Click);
            // 
            // btnAllCrop
            // 
            this.btnAllCrop.Location = new System.Drawing.Point(4, 81);
            this.btnAllCrop.Name = "btnAllCrop";
            this.btnAllCrop.Size = new System.Drawing.Size(75, 23);
            this.btnAllCrop.TabIndex = 1;
            this.btnAllCrop.Text = "Crop";
            this.btnAllCrop.UseVisualStyleBackColor = true;
            // 
            // btnAdd2List
            // 
            this.btnAdd2List.Location = new System.Drawing.Point(3, 3);
            this.btnAdd2List.Name = "btnAdd2List";
            this.btnAdd2List.Size = new System.Drawing.Size(75, 23);
            this.btnAdd2List.TabIndex = 0;
            this.btnAdd2List.Text = "Add to List";
            this.btnAdd2List.UseVisualStyleBackColor = true;
            this.btnAdd2List.Click += new System.EventHandler(this.btnAdd2List_Click);
            // 
            // btnLoadGrary
            // 
            this.btnLoadGrary.Location = new System.Drawing.Point(335, 12);
            this.btnLoadGrary.Name = "btnLoadGrary";
            this.btnLoadGrary.Size = new System.Drawing.Size(75, 23);
            this.btnLoadGrary.TabIndex = 0;
            this.btnLoadGrary.Text = "Load Gray";
            this.btnLoadGrary.UseVisualStyleBackColor = true;
            // 
            // btnLoadColor
            // 
            this.btnLoadColor.Location = new System.Drawing.Point(426, 12);
            this.btnLoadColor.Name = "btnLoadColor";
            this.btnLoadColor.Size = new System.Drawing.Size(75, 23);
            this.btnLoadColor.TabIndex = 4;
            this.btnLoadColor.Text = "Load Color";
            this.btnLoadColor.UseVisualStyleBackColor = true;
            this.btnLoadColor.Click += new System.EventHandler(this.btnLoadColor_Click);
            // 
            // rtxLog
            // 
            this.rtxLog.Location = new System.Drawing.Point(8, 509);
            this.rtxLog.Name = "rtxLog";
            this.rtxLog.Size = new System.Drawing.Size(317, 170);
            this.rtxLog.TabIndex = 5;
            this.rtxLog.Text = "";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(588, 12);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(507, 12);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNormalizeAll
            // 
            this.btnNormalizeAll.Location = new System.Drawing.Point(2, 202);
            this.btnNormalizeAll.Name = "btnNormalizeAll";
            this.btnNormalizeAll.Size = new System.Drawing.Size(75, 23);
            this.btnNormalizeAll.TabIndex = 8;
            this.btnNormalizeAll.Text = "NormAll";
            this.btnNormalizeAll.UseVisualStyleBackColor = true;
            this.btnNormalizeAll.Click += new System.EventHandler(this.btnNormalizeAll_Click);
            // 
            // nudNormalizeLevel
            // 
            this.nudNormalizeLevel.Location = new System.Drawing.Point(84, 205);
            this.nudNormalizeLevel.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudNormalizeLevel.Name = "nudNormalizeLevel";
            this.nudNormalizeLevel.Size = new System.Drawing.Size(66, 20);
            this.nudNormalizeLevel.TabIndex = 9;
            // 
            // btnCheckNorm
            // 
            this.btnCheckNorm.Location = new System.Drawing.Point(3, 231);
            this.btnCheckNorm.Name = "btnCheckNorm";
            this.btnCheckNorm.Size = new System.Drawing.Size(75, 23);
            this.btnCheckNorm.TabIndex = 10;
            this.btnCheckNorm.Text = "CheckNorm";
            this.btnCheckNorm.UseVisualStyleBackColor = true;
            this.btnCheckNorm.Click += new System.EventHandler(this.btnCheckNorm_Click);
            // 
            // btnTestImgClear
            // 
            this.btnTestImgClear.Location = new System.Drawing.Point(166, 3);
            this.btnTestImgClear.Name = "btnTestImgClear";
            this.btnTestImgClear.Size = new System.Drawing.Size(75, 23);
            this.btnTestImgClear.TabIndex = 11;
            this.btnTestImgClear.Text = "ImgClear";
            this.btnTestImgClear.UseVisualStyleBackColor = true;
            this.btnTestImgClear.Click += new System.EventHandler(this.btnTestImgClear_Click);
            // 
            // btnTotal
            // 
            this.btnTotal.Location = new System.Drawing.Point(197, 130);
            this.btnTotal.Name = "btnTotal";
            this.btnTotal.Size = new System.Drawing.Size(75, 59);
            this.btnTotal.TabIndex = 12;
            this.btnTotal.Text = "Total";
            this.btnTotal.UseVisualStyleBackColor = true;
            this.btnTotal.Click += new System.EventHandler(this.btnTotal_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 682);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.rtxLog);
            this.Controls.Add(this.btnLoadColor);
            this.Controls.Add(this.btnLoadGrary);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.imgBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.Basic.ResumeLayout(false);
            this.Color.ResumeLayout(false);
            this.Color.PerformLayout();
            this.colorConv.ResumeLayout(false);
            this.colorConv.PerformLayout();
            this.Modify.ResumeLayout(false);
            this.Modify.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNormalizeLevel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Emgu.CV.UI.ImageBox imgBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Basic;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage Adv;
        private System.Windows.Forms.TabPage colorConv;
        private System.Windows.Forms.Button btnLoadGrary;
        private System.Windows.Forms.Button btnLoadColor;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RichTextBox rtxLog;
        private System.Windows.Forms.Button btnConvColor;
        private System.Windows.Forms.RichTextBox rtbblue;
        private System.Windows.Forms.RichTextBox rtbRed;
        private System.Windows.Forms.RichTextBox rtbGreen;
        private System.Windows.Forms.TabPage Color;
        private System.Windows.Forms.RichTextBox rtbGammaColor;
        private System.Windows.Forms.RichTextBox rtbNormalizeColor;
        private System.Windows.Forms.RichTextBox rtbThresColor;
        private System.Windows.Forms.Button btnGammaColor;
        private System.Windows.Forms.Button btnNormalizeColor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnThresColor;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.TabPage Modify;
        private System.Windows.Forms.Button btnAllResize;
        private System.Windows.Forms.Button btnAllCrop;
        private System.Windows.Forms.Button btnAdd2List;
        private System.Windows.Forms.NumericUpDown nudScale;
        private System.Windows.Forms.Label lblstatus;
        private System.Windows.Forms.Button btnClearAllList;
        private System.Windows.Forms.Button btnAllHStack;
        private System.Windows.Forms.Button btnAllVStack;
        private System.Windows.Forms.Button btnNormalizeAll;
        private System.Windows.Forms.NumericUpDown nudNormalizeLevel;
        private System.Windows.Forms.Button btnCheckNorm;
        private System.Windows.Forms.Button btnTestImgClear;
        private System.Windows.Forms.Button btnTotal;
    }
}

