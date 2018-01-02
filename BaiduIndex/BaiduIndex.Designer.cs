namespace BaiduIndex
{
    partial class BaiduMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.RtbMsg = new System.Windows.Forms.RichTextBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.tourbtn = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.RtbMsg);
            this.panel1.Location = new System.Drawing.Point(12, 67);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 375);
            this.panel1.TabIndex = 0;
            // 
            // RtbMsg
            // 
            this.RtbMsg.BackColor = System.Drawing.SystemColors.InfoText;
            this.RtbMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RtbMsg.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.RtbMsg.Location = new System.Drawing.Point(0, 0);
            this.RtbMsg.Name = "RtbMsg";
            this.RtbMsg.Size = new System.Drawing.Size(231, 375);
            this.RtbMsg.TabIndex = 0;
            this.RtbMsg.Text = "";
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(256, 67);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(695, 375);
            this.webBrowser1.TabIndex = 4;
            // 
            // tourbtn
            // 
            this.tourbtn.Location = new System.Drawing.Point(12, 22);
            this.tourbtn.Name = "tourbtn";
            this.tourbtn.Size = new System.Drawing.Size(169, 23);
            this.tourbtn.TabIndex = 5;
            this.tourbtn.Text = "百度指数";
            this.tourbtn.UseVisualStyleBackColor = true;
            this.tourbtn.Click += new System.EventHandler(this.tourbtn_Click);
            // 
            // BaiduMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 452);
            this.Controls.Add(this.tourbtn);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.panel1);
            this.Name = "BaiduMain";
            this.Text = "百度指数数据";
            this.Load += new System.EventHandler(this.BaiduMain_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox RtbMsg;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button tourbtn;
    }
}

