﻿namespace DCTHKTUtility
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
            this.button1 = new System.Windows.Forms.Button();
            this.UpdateTFS = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(28, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(157, 48);
            this.button1.TabIndex = 0;
            this.button1.Text = "Get New Tickets";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UpdateTFS
            // 
            this.UpdateTFS.Location = new System.Drawing.Point(28, 78);
            this.UpdateTFS.Name = "UpdateTFS";
            this.UpdateTFS.Size = new System.Drawing.Size(157, 49);
            this.UpdateTFS.TabIndex = 1;
            this.UpdateTFS.Text = "Update Assigned Developer";
            this.UpdateTFS.UseVisualStyleBackColor = true;
            this.UpdateTFS.Click += new System.EventHandler(this.UpdateTFS_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(28, 148);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(157, 49);
            this.button2.TabIndex = 2;
            this.button2.Text = "Get Current assign Developer(Periodic)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(253, 12);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(157, 48);
            this.button3.TabIndex = 3;
            this.button3.Text = "Get Jira Ticket";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.UpdateTFS);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button UpdateTFS;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

