namespace SnifferDoodle
{
	partial class SnifferDoodle
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
			this.snifferPort = new System.IO.Ports.SerialPort(this.components);
			this.portPanel = new System.Windows.Forms.Panel();
			this.notBox = new System.Windows.Forms.CheckBox();
			this.maxHistoryEditBox = new System.Windows.Forms.MaskedTextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.clearButton = new System.Windows.Forms.Button();
			this.captureFilter = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.captureButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.portComboList = new System.Windows.Forms.ComboBox();
			this.snifferDataList = new ListViewNF();
			this.timeStamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.typeData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.commandData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.responseData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.details = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.snifferDataContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.snifferStrip = new System.Windows.Forms.StatusStrip();
			this.packetTimeDiff = new System.Windows.Forms.ToolStripStatusLabel();
			this.portPanel.SuspendLayout();
			this.snifferDataContextMenu.SuspendLayout();
			this.snifferStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// snifferPort
			// 
			this.snifferPort.BaudRate = 115200;
			this.snifferPort.ReadBufferSize = 65536;
			this.snifferPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.snifferPort_DataReceived);
			// 
			// portPanel
			// 
			this.portPanel.Controls.Add(this.notBox);
			this.portPanel.Controls.Add(this.maxHistoryEditBox);
			this.portPanel.Controls.Add(this.label3);
			this.portPanel.Controls.Add(this.clearButton);
			this.portPanel.Controls.Add(this.captureFilter);
			this.portPanel.Controls.Add(this.label2);
			this.portPanel.Controls.Add(this.captureButton);
			this.portPanel.Controls.Add(this.label1);
			this.portPanel.Controls.Add(this.portComboList);
			this.portPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.portPanel.Location = new System.Drawing.Point(0, 0);
			this.portPanel.Name = "portPanel";
			this.portPanel.Size = new System.Drawing.Size(688, 83);
			this.portPanel.TabIndex = 0;
			// 
			// notBox
			// 
			this.notBox.AutoSize = true;
			this.notBox.Location = new System.Drawing.Point(368, 46);
			this.notBox.Name = "notBox";
			this.notBox.Size = new System.Drawing.Size(43, 17);
			this.notBox.TabIndex = 8;
			this.notBox.Text = "Not";
			this.notBox.UseVisualStyleBackColor = true;
			this.notBox.CheckStateChanged += new System.EventHandler(this.notBox_CheckStateChanged);
			// 
			// maxHistoryEditBox
			// 
			this.maxHistoryEditBox.Location = new System.Drawing.Point(459, 43);
			this.maxHistoryEditBox.Mask = "######";
			this.maxHistoryEditBox.Name = "maxHistoryEditBox";
			this.maxHistoryEditBox.Size = new System.Drawing.Size(100, 20);
			this.maxHistoryEditBox.TabIndex = 7;
			this.maxHistoryEditBox.TypeValidationCompleted += new System.Windows.Forms.TypeValidationEventHandler(this.maxHistoryEditBox_TypeValidationCompleted);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.label3.Location = new System.Drawing.Point(414, 46);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(39, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "History";
			// 
			// clearButton
			// 
			this.clearButton.Location = new System.Drawing.Point(252, 6);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(75, 23);
			this.clearButton.TabIndex = 5;
			this.clearButton.Text = "Clear";
			this.clearButton.UseVisualStyleBackColor = true;
			this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
			// 
			// captureFilter
			// 
			this.captureFilter.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::SnifferDoodle.Properties.Settings.Default, "Filter", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.captureFilter.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.captureFilter.Location = new System.Drawing.Point(47, 43);
			this.captureFilter.Name = "captureFilter";
			this.captureFilter.Size = new System.Drawing.Size(315, 20);
			this.captureFilter.TabIndex = 4;
			this.captureFilter.Text = global::SnifferDoodle.Properties.Settings.Default.Filter;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(29, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Filter";
			// 
			// captureButton
			// 
			this.captureButton.Location = new System.Drawing.Point(171, 6);
			this.captureButton.Name = "captureButton";
			this.captureButton.Size = new System.Drawing.Size(75, 23);
			this.captureButton.TabIndex = 2;
			this.captureButton.Text = "Start";
			this.captureButton.UseVisualStyleBackColor = true;
			this.captureButton.Click += new System.EventHandler(this.captureButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(26, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Port";
			// 
			// portComboList
			// 
			this.portComboList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.portComboList.FormattingEnabled = true;
			this.portComboList.Location = new System.Drawing.Point(44, 6);
			this.portComboList.Name = "portComboList";
			this.portComboList.Size = new System.Drawing.Size(121, 21);
			this.portComboList.TabIndex = 0;
			this.portComboList.SelectedIndexChanged += new System.EventHandler(this.portComboList_SelectedIndexChanged);
			this.portComboList.SelectedValueChanged += new System.EventHandler(this.portComboList_SelectedValueChanged);
			// 
			// snifferDataList
			// 
			this.snifferDataList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timeStamp,
            this.typeData,
            this.commandData,
            this.responseData,
            this.details});
			this.snifferDataList.ContextMenuStrip = this.snifferDataContextMenu;
			this.snifferDataList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.snifferDataList.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.snifferDataList.FullRowSelect = true;
			this.snifferDataList.Location = new System.Drawing.Point(0, 83);
			this.snifferDataList.Name = "snifferDataList";
			this.snifferDataList.Size = new System.Drawing.Size(688, 374);
			this.snifferDataList.TabIndex = 1;
			this.snifferDataList.UseCompatibleStateImageBehavior = false;
			this.snifferDataList.View = System.Windows.Forms.View.Details;
			this.snifferDataList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.snifferDataList_ItemSelectionChanged);
			// 
			// timeStamp
			// 
			this.timeStamp.Text = "Time";
			this.timeStamp.Width = 164;
			// 
			// typeData
			// 
			this.typeData.Text = "Type";
			// 
			// commandData
			// 
			this.commandData.Text = "Command";
			this.commandData.Width = 210;
			// 
			// responseData
			// 
			this.responseData.Text = "Response";
			this.responseData.Width = 147;
			// 
			// details
			// 
			this.details.Text = "Details";
			this.details.Width = 103;
			// 
			// snifferDataContextMenu
			// 
			this.snifferDataContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem});
			this.snifferDataContextMenu.Name = "snifferDataContextMenu";
			this.snifferDataContextMenu.Size = new System.Drawing.Size(100, 26);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// snifferStrip
			// 
			this.snifferStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.packetTimeDiff});
			this.snifferStrip.Location = new System.Drawing.Point(0, 457);
			this.snifferStrip.Name = "snifferStrip";
			this.snifferStrip.Size = new System.Drawing.Size(688, 22);
			this.snifferStrip.TabIndex = 2;
			this.snifferStrip.Text = "statusStrip1";
			// 
			// packetTimeDiff
			// 
			this.packetTimeDiff.Name = "packetTimeDiff";
			this.packetTimeDiff.Size = new System.Drawing.Size(0, 17);
			// 
			// SnifferDoodle
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(688, 479);
			this.Controls.Add(this.snifferDataList);
			this.Controls.Add(this.snifferStrip);
			this.Controls.Add(this.portPanel);
			this.Name = "SnifferDoodle";
			this.Text = "Sniffer Doodle";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SnifferDoodle_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SnifferDoodle_FormClosed);
			this.Load += new System.EventHandler(this.SnifferDoodle_Load);
			this.ResizeEnd += new System.EventHandler(this.SnifferDoodle_ResizeEnd);
			this.portPanel.ResumeLayout(false);
			this.portPanel.PerformLayout();
			this.snifferDataContextMenu.ResumeLayout(false);
			this.snifferStrip.ResumeLayout(false);
			this.snifferStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.IO.Ports.SerialPort snifferPort;
		private System.Windows.Forms.Panel portPanel;
		private ListViewNF snifferDataList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox portComboList;
		private System.Windows.Forms.ColumnHeader timeStamp;
		private System.Windows.Forms.ColumnHeader commandData;
		private System.Windows.Forms.ColumnHeader responseData;
		private System.Windows.Forms.Button captureButton;
		private System.Windows.Forms.ColumnHeader typeData;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox captureFilter;
		private System.Windows.Forms.Button clearButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.MaskedTextBox maxHistoryEditBox;
		private System.Windows.Forms.CheckBox notBox;
		private System.Windows.Forms.ColumnHeader details;
		private System.Windows.Forms.ContextMenuStrip snifferDataContextMenu;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.StatusStrip snifferStrip;
		private System.Windows.Forms.ToolStripStatusLabel packetTimeDiff;
	}
}

