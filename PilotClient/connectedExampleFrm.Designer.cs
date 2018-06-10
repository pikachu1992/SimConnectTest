namespace PilotClient
{
    partial class connectedExampleFrm
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
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.btnGetPositionAsync = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(12, 12);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(494, 510);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // btnGetPositionAsync
            // 
            this.btnGetPositionAsync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetPositionAsync.Location = new System.Drawing.Point(408, 528);
            this.btnGetPositionAsync.Name = "btnGetPositionAsync";
            this.btnGetPositionAsync.Size = new System.Drawing.Size(98, 23);
            this.btnGetPositionAsync.TabIndex = 1;
            this.btnGetPositionAsync.Text = "GetPositionAsnc";
            this.btnGetPositionAsync.UseVisualStyleBackColor = true;
            this.btnGetPositionAsync.Click += new System.EventHandler(this.btnGetPositionAsync_Click);
            // 
            // connectedExampleFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 563);
            this.Controls.Add(this.btnGetPositionAsync);
            this.Controls.Add(this.txtLog);
            this.Name = "connectedExampleFrm";
            this.Text = "Form1";
            this.SimConnectOpen += new System.EventHandler(this.connectedExampleFrm_SimConnectOpen);
            this.SimConnectClosed += new System.EventHandler(this.connectedExampleFrm_SimConnectClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Button btnGetPositionAsync;
    }
}

