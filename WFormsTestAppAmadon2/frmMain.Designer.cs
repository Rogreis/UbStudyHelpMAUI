namespace WFormsTestAppAmadon2
{
    partial class frmMain
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
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageInicialization = new System.Windows.Forms.TabPage();
            this.splitContainerInicialization = new System.Windows.Forms.SplitContainer();
            this.btTest = new System.Windows.Forms.Button();
            this.btInicializeParamLog = new System.Windows.Forms.Button();
            this.txInitializationMessages = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.txLog = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonInicialization = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelMessages = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControlMain.SuspendLayout();
            this.tabPageInicialization.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInicialization)).BeginInit();
            this.splitContainerInicialization.Panel1.SuspendLayout();
            this.splitContainerInicialization.Panel2.SuspendLayout();
            this.splitContainerInicialization.SuspendLayout();
            this.tabPageLog.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageInicialization);
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.Controls.Add(this.tabPageLog);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 34);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1347, 827);
            this.tabControlMain.TabIndex = 0;
            // 
            // tabPageInicialization
            // 
            this.tabPageInicialization.Controls.Add(this.splitContainerInicialization);
            this.tabPageInicialization.Location = new System.Drawing.Point(4, 34);
            this.tabPageInicialization.Name = "tabPageInicialization";
            this.tabPageInicialization.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInicialization.Size = new System.Drawing.Size(1339, 789);
            this.tabPageInicialization.TabIndex = 0;
            this.tabPageInicialization.Text = "Inicialization";
            this.tabPageInicialization.UseVisualStyleBackColor = true;
            // 
            // splitContainerInicialization
            // 
            this.splitContainerInicialization.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerInicialization.Location = new System.Drawing.Point(3, 3);
            this.splitContainerInicialization.Name = "splitContainerInicialization";
            // 
            // splitContainerInicialization.Panel1
            // 
            this.splitContainerInicialization.Panel1.Controls.Add(this.btTest);
            this.splitContainerInicialization.Panel1.Controls.Add(this.btInicializeParamLog);
            // 
            // splitContainerInicialization.Panel2
            // 
            this.splitContainerInicialization.Panel2.Controls.Add(this.txInitializationMessages);
            this.splitContainerInicialization.Size = new System.Drawing.Size(1333, 783);
            this.splitContainerInicialization.SplitterDistance = 282;
            this.splitContainerInicialization.TabIndex = 0;
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(31, 488);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(176, 54);
            this.btTest.TabIndex = 1;
            this.btTest.Text = "Test";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // btInicializeParamLog
            // 
            this.btInicializeParamLog.Location = new System.Drawing.Point(18, 18);
            this.btInicializeParamLog.Name = "btInicializeParamLog";
            this.btInicializeParamLog.Size = new System.Drawing.Size(176, 54);
            this.btInicializeParamLog.TabIndex = 0;
            this.btInicializeParamLog.Text = "Param && Log";
            this.btInicializeParamLog.UseVisualStyleBackColor = true;
            this.btInicializeParamLog.Click += new System.EventHandler(this.btInicializeParamLog_Click);
            // 
            // txInitializationMessages
            // 
            this.txInitializationMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txInitializationMessages.Location = new System.Drawing.Point(0, 0);
            this.txInitializationMessages.Multiline = true;
            this.txInitializationMessages.Name = "txInitializationMessages";
            this.txInitializationMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txInitializationMessages.Size = new System.Drawing.Size(1047, 783);
            this.txInitializationMessages.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1339, 789);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPageLog
            // 
            this.tabPageLog.Controls.Add(this.txLog);
            this.tabPageLog.Location = new System.Drawing.Point(4, 34);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Size = new System.Drawing.Size(1339, 789);
            this.tabPageLog.TabIndex = 2;
            this.tabPageLog.Text = "Log";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // txLog
            // 
            this.txLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txLog.Location = new System.Drawing.Point(0, 0);
            this.txLog.Multiline = true;
            this.txLog.Name = "txLog";
            this.txLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txLog.Size = new System.Drawing.Size(1339, 789);
            this.txLog.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonInicialization,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1347, 34);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonInicialization
            // 
            this.toolStripButtonInicialization.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonInicialization.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonInicialization.Name = "toolStripButtonInicialization";
            this.toolStripButtonInicialization.Size = new System.Drawing.Size(112, 29);
            this.toolStripButtonInicialization.Text = "Inicialization";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(34, 29);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(34, 29);
            this.toolStripButton3.Text = "toolStripButton3";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessages});
            this.statusStrip1.Location = new System.Drawing.Point(0, 861);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1347, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessages
            // 
            this.toolStripStatusLabelMessages.Name = "toolStripStatusLabelMessages";
            this.toolStripStatusLabelMessages.Size = new System.Drawing.Size(1332, 15);
            this.toolStripStatusLabelMessages.Spring = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1347, 883);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageInicialization.ResumeLayout(false);
            this.splitContainerInicialization.Panel1.ResumeLayout(false);
            this.splitContainerInicialization.Panel2.ResumeLayout(false);
            this.splitContainerInicialization.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerInicialization)).EndInit();
            this.splitContainerInicialization.ResumeLayout(false);
            this.tabPageLog.ResumeLayout(false);
            this.tabPageLog.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TabControl tabControlMain;
        private TabPage tabPageInicialization;
        private TabPage tabPage2;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButtonInicialization;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabelMessages;
        private SplitContainer splitContainerInicialization;
        private Button btInicializeParamLog;
        private TabPage tabPageLog;
        private TextBox txLog;
        private TextBox txInitializationMessages;
        private Button btTest;
    }
}