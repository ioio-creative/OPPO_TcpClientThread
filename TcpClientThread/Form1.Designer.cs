﻿namespace TcpClientThread
{
    partial class fmTcpClientThread
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
            this.txtReceived = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtReceived
            // 
            this.txtReceived.Location = new System.Drawing.Point(12, 29);
            this.txtReceived.Multiline = true;
            this.txtReceived.Name = "txtReceived";
            this.txtReceived.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReceived.Size = new System.Drawing.Size(637, 471);
            this.txtReceived.TabIndex = 0;
            // 
            // fmTcpClientThread
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 553);
            this.Controls.Add(this.txtReceived);
            this.Name = "fmTcpClientThread";
            this.Text = "TCP Client Thread";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmTcpClientThread_FormClosed);
            this.Load += new System.EventHandler(this.fmTcpClientThread_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtReceived;
    }
}

