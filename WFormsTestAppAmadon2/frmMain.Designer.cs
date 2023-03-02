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
            tabControlMain = new TabControl();
            tabPageInicialization = new TabPage();
            splitContainerInicialization = new SplitContainer();
            btSettings = new Button();
            btTOC_test = new Button();
            btSearchIndex = new Button();
            btSearchTest = new Button();
            btInicializeParamLog = new Button();
            txInitializationMessages = new TextBox();
            tabPage2 = new TabPage();
            tabPageLog = new TabPage();
            txLog = new TextBox();
            toolStrip1 = new ToolStrip();
            toolStripButtonInicialization = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            toolStripButton3 = new ToolStripButton();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabelMessages = new ToolStripStatusLabel();
            tabControlMain.SuspendLayout();
            tabPageInicialization.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerInicialization).BeginInit();
            splitContainerInicialization.Panel1.SuspendLayout();
            splitContainerInicialization.Panel2.SuspendLayout();
            splitContainerInicialization.SuspendLayout();
            tabPageLog.SuspendLayout();
            toolStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControlMain
            // 
            tabControlMain.Controls.Add(tabPageInicialization);
            tabControlMain.Controls.Add(tabPage2);
            tabControlMain.Controls.Add(tabPageLog);
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Location = new Point(0, 34);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(1347, 827);
            tabControlMain.TabIndex = 0;
            // 
            // tabPageInicialization
            // 
            tabPageInicialization.Controls.Add(splitContainerInicialization);
            tabPageInicialization.Location = new Point(4, 34);
            tabPageInicialization.Name = "tabPageInicialization";
            tabPageInicialization.Padding = new Padding(3);
            tabPageInicialization.Size = new Size(1339, 789);
            tabPageInicialization.TabIndex = 0;
            tabPageInicialization.Text = "Inicialization";
            tabPageInicialization.UseVisualStyleBackColor = true;
            // 
            // splitContainerInicialization
            // 
            splitContainerInicialization.Dock = DockStyle.Fill;
            splitContainerInicialization.Location = new Point(3, 3);
            splitContainerInicialization.Name = "splitContainerInicialization";
            // 
            // splitContainerInicialization.Panel1
            // 
            splitContainerInicialization.Panel1.Controls.Add(btSettings);
            splitContainerInicialization.Panel1.Controls.Add(btTOC_test);
            splitContainerInicialization.Panel1.Controls.Add(btSearchIndex);
            splitContainerInicialization.Panel1.Controls.Add(btSearchTest);
            splitContainerInicialization.Panel1.Controls.Add(btInicializeParamLog);
            // 
            // splitContainerInicialization.Panel2
            // 
            splitContainerInicialization.Panel2.Controls.Add(txInitializationMessages);
            splitContainerInicialization.Size = new Size(1333, 783);
            splitContainerInicialization.SplitterDistance = 282;
            splitContainerInicialization.TabIndex = 0;
            // 
            // btSettings
            // 
            btSettings.Location = new Point(18, 258);
            btSettings.Name = "btSettings";
            btSettings.Size = new Size(176, 54);
            btSettings.TabIndex = 5;
            btSettings.Text = "Settings";
            btSettings.UseVisualStyleBackColor = true;
            btSettings.Click += btSettings_Click;
            // 
            // btTOC_test
            // 
            btTOC_test.Location = new Point(18, 198);
            btTOC_test.Name = "btTOC_test";
            btTOC_test.Size = new Size(176, 54);
            btTOC_test.TabIndex = 4;
            btTOC_test.Text = "TOC";
            btTOC_test.UseVisualStyleBackColor = true;
            btTOC_test.Click += btTOC_test_Click;
            // 
            // btSearchIndex
            // 
            btSearchIndex.Location = new Point(18, 138);
            btSearchIndex.Name = "btSearchIndex";
            btSearchIndex.Size = new Size(176, 54);
            btSearchIndex.TabIndex = 3;
            btSearchIndex.Text = "Search Index";
            btSearchIndex.UseVisualStyleBackColor = true;
            btSearchIndex.Click += btSearchIndex_Click;
            // 
            // btSearchTest
            // 
            btSearchTest.Location = new Point(18, 78);
            btSearchTest.Name = "btSearchTest";
            btSearchTest.Size = new Size(176, 54);
            btSearchTest.TabIndex = 2;
            btSearchTest.Text = "Search Test";
            btSearchTest.UseVisualStyleBackColor = true;
            btSearchTest.Click += btSearchTest_Click;
            // 
            // btInicializeParamLog
            // 
            btInicializeParamLog.Location = new Point(18, 18);
            btInicializeParamLog.Name = "btInicializeParamLog";
            btInicializeParamLog.Size = new Size(176, 54);
            btInicializeParamLog.TabIndex = 0;
            btInicializeParamLog.Text = "Init";
            btInicializeParamLog.UseVisualStyleBackColor = true;
            btInicializeParamLog.Click += btInicializeParamLog_Click;
            // 
            // txInitializationMessages
            // 
            txInitializationMessages.Dock = DockStyle.Fill;
            txInitializationMessages.Location = new Point(0, 0);
            txInitializationMessages.Multiline = true;
            txInitializationMessages.Name = "txInitializationMessages";
            txInitializationMessages.ScrollBars = ScrollBars.Vertical;
            txInitializationMessages.Size = new Size(1047, 783);
            txInitializationMessages.TabIndex = 1;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 34);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(1339, 789);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPageLog
            // 
            tabPageLog.Controls.Add(txLog);
            tabPageLog.Location = new Point(4, 34);
            tabPageLog.Name = "tabPageLog";
            tabPageLog.Size = new Size(1339, 789);
            tabPageLog.TabIndex = 2;
            tabPageLog.Text = "Log";
            tabPageLog.UseVisualStyleBackColor = true;
            // 
            // txLog
            // 
            txLog.Dock = DockStyle.Fill;
            txLog.Location = new Point(0, 0);
            txLog.Multiline = true;
            txLog.Name = "txLog";
            txLog.ScrollBars = ScrollBars.Vertical;
            txLog.Size = new Size(1339, 789);
            txLog.TabIndex = 0;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(24, 24);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButtonInicialization, toolStripButton2, toolStripButton3 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1347, 34);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonInicialization
            // 
            toolStripButtonInicialization.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButtonInicialization.ImageTransparentColor = Color.Magenta;
            toolStripButtonInicialization.Name = "toolStripButtonInicialization";
            toolStripButtonInicialization.Size = new Size(112, 29);
            toolStripButtonInicialization.Text = "Inicialization";
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(34, 29);
            toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton3.ImageTransparentColor = Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new Size(34, 29);
            toolStripButton3.Text = "toolStripButton3";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabelMessages });
            statusStrip1.Location = new Point(0, 861);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1347, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelMessages
            // 
            toolStripStatusLabelMessages.Name = "toolStripStatusLabelMessages";
            toolStripStatusLabelMessages.Size = new Size(1332, 15);
            toolStripStatusLabelMessages.Spring = true;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1347, 883);
            Controls.Add(tabControlMain);
            Controls.Add(toolStrip1);
            Controls.Add(statusStrip1);
            Name = "frmMain";
            Text = "Form1";
            Load += frmMain_Load;
            tabControlMain.ResumeLayout(false);
            tabPageInicialization.ResumeLayout(false);
            splitContainerInicialization.Panel1.ResumeLayout(false);
            splitContainerInicialization.Panel2.ResumeLayout(false);
            splitContainerInicialization.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerInicialization).EndInit();
            splitContainerInicialization.ResumeLayout(false);
            tabPageLog.ResumeLayout(false);
            tabPageLog.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private Button btSearchTest;
        private Button btSearchIndex;
        private Button btTOC_test;
        private Button btSettings;
    }
}