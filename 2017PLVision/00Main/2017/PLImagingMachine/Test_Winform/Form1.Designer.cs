namespace Test_Winform
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
            this.btnconnection = new System.Windows.Forms.Button();
            this.btnmove = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnenumtolis = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnconnection
            // 
            this.btnconnection.Location = new System.Drawing.Point(12, 12);
            this.btnconnection.Name = "btnconnection";
            this.btnconnection.Size = new System.Drawing.Size(75, 23);
            this.btnconnection.TabIndex = 0;
            this.btnconnection.Text = "Connection";
            this.btnconnection.UseVisualStyleBackColor = true;
            this.btnconnection.Click += new System.EventHandler(this.btnconnection_Click);
            // 
            // btnmove
            // 
            this.btnmove.Location = new System.Drawing.Point(12, 41);
            this.btnmove.Name = "btnmove";
            this.btnmove.Size = new System.Drawing.Size(75, 23);
            this.btnmove.TabIndex = 1;
            this.btnmove.Text = "Move";
            this.btnmove.UseVisualStyleBackColor = true;
            this.btnmove.Click += new System.EventHandler(this.btnmove_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 70);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "TurnOn";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(131, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "inichange";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnenumtolis
            // 
            this.btnenumtolis.Location = new System.Drawing.Point(131, 51);
            this.btnenumtolis.Name = "btnenumtolis";
            this.btnenumtolis.Size = new System.Drawing.Size(75, 23);
            this.btnenumtolis.TabIndex = 4;
            this.btnenumtolis.Text = "enumtolsit";
            this.btnenumtolis.UseVisualStyleBackColor = true;
            this.btnenumtolis.Click += new System.EventHandler(this.btnenumtolis_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnenumtolis);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnmove);
            this.Controls.Add(this.btnconnection);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnconnection;
        private System.Windows.Forms.Button btnmove;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnenumtolis;
    }
}

