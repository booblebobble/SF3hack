partial class Form2
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
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
		System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
		this.dgvPlayer1 = new System.Windows.Forms.DataGridView();
		this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
		this.Column1 = new System.Windows.Forms.DataGridViewImageColumn();
		this.dgvPlayer2 = new System.Windows.Forms.DataGridView();
		this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
		this.lblMaxCombo = new System.Windows.Forms.Label();
		this.lblMaxComboT = new System.Windows.Forms.Label();
		this.lblMaxDamage = new System.Windows.Forms.Label();
		this.lblMaxDamageT = new System.Windows.Forms.Label();
		this.lblComboDamage = new System.Windows.Forms.Label();
		this.lblComboDamageT = new System.Windows.Forms.Label();
		this.lblLastDamage = new System.Windows.Forms.Label();
		this.lblLastDamageT = new System.Windows.Forms.Label();
		this.btnConfig = new System.Windows.Forms.Button();
		this.lblStatus = new System.Windows.Forms.Label();
		this.btnClose = new System.Windows.Forms.Button();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		((System.ComponentModel.ISupportInitialize)(this.dgvPlayer1)).BeginInit();
		((System.ComponentModel.ISupportInitialize)(this.dgvPlayer2)).BeginInit();
		this.tableLayoutPanel1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.SuspendLayout();
		// 
		// dgvPlayer1
		// 
		this.dgvPlayer1.AllowUserToAddRows = false;
		this.dgvPlayer1.AllowUserToDeleteRows = false;
		this.dgvPlayer1.AllowUserToResizeColumns = false;
		this.dgvPlayer1.AllowUserToResizeRows = false;
		this.dgvPlayer1.BackgroundColor = System.Drawing.SystemColors.Control;
		this.dgvPlayer1.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.dgvPlayer1.CausesValidation = false;
		this.dgvPlayer1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
		this.dgvPlayer1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		this.dgvPlayer1.ColumnHeadersVisible = false;
		this.dgvPlayer1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column2,
            this.Column1});
		this.dgvPlayer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvPlayer1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
		this.dgvPlayer1.Enabled = false;
		this.dgvPlayer1.Location = new System.Drawing.Point(0, 0);
		this.dgvPlayer1.MultiSelect = false;
		this.dgvPlayer1.Name = "dgvPlayer1";
		this.dgvPlayer1.ReadOnly = true;
		this.dgvPlayer1.RowHeadersVisible = false;
		this.dgvPlayer1.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvPlayer1.ShowCellErrors = false;
		this.dgvPlayer1.ShowCellToolTips = false;
		this.dgvPlayer1.ShowEditingIcon = false;
		this.dgvPlayer1.ShowRowErrors = false;
		this.dgvPlayer1.Size = new System.Drawing.Size(134, 282);
		this.dgvPlayer1.TabIndex = 0;
		this.dgvPlayer1.TabStop = false;
		// 
		// Column2
		// 
		this.Column2.HeaderText = "Column2";
		this.Column2.Name = "Column2";
		this.Column2.ReadOnly = true;
		this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
		this.Column2.Width = 28;
		// 
		// Column1
		// 
		this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle1.NullValue = null;
		this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
		this.Column1.HeaderText = "Column1";
		this.Column1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
		this.Column1.Name = "Column1";
		this.Column1.ReadOnly = true;
		this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		// 
		// dgvPlayer2
		// 
		this.dgvPlayer2.AllowUserToAddRows = false;
		this.dgvPlayer2.AllowUserToDeleteRows = false;
		this.dgvPlayer2.AllowUserToResizeColumns = false;
		this.dgvPlayer2.AllowUserToResizeRows = false;
		this.dgvPlayer2.BackgroundColor = System.Drawing.SystemColors.Control;
		this.dgvPlayer2.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.dgvPlayer2.CausesValidation = false;
		this.dgvPlayer2.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
		this.dgvPlayer2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
		this.dgvPlayer2.ColumnHeadersVisible = false;
		this.dgvPlayer2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewImageColumn1});
		this.dgvPlayer2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.dgvPlayer2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
		this.dgvPlayer2.Enabled = false;
		this.dgvPlayer2.Location = new System.Drawing.Point(0, 0);
		this.dgvPlayer2.MultiSelect = false;
		this.dgvPlayer2.Name = "dgvPlayer2";
		this.dgvPlayer2.ReadOnly = true;
		this.dgvPlayer2.RowHeadersVisible = false;
		this.dgvPlayer2.ScrollBars = System.Windows.Forms.ScrollBars.None;
		this.dgvPlayer2.ShowCellErrors = false;
		this.dgvPlayer2.ShowCellToolTips = false;
		this.dgvPlayer2.ShowEditingIcon = false;
		this.dgvPlayer2.ShowRowErrors = false;
		this.dgvPlayer2.Size = new System.Drawing.Size(135, 282);
		this.dgvPlayer2.TabIndex = 1;
		this.dgvPlayer2.TabStop = false;
		// 
		// dataGridViewImageColumn1
		// 
		this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
		dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
		dataGridViewCellStyle2.NullValue = null;
		this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle2;
		this.dataGridViewImageColumn1.HeaderText = "Column1";
		this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
		this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
		this.dataGridViewImageColumn1.ReadOnly = true;
		this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
		// 
		// lblMaxCombo
		// 
		this.lblMaxCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblMaxCombo.AutoSize = true;
		this.lblMaxCombo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblMaxCombo.Location = new System.Drawing.Point(84, 51);
		this.lblMaxCombo.Name = "lblMaxCombo";
		this.lblMaxCombo.Size = new System.Drawing.Size(31, 13);
		this.lblMaxCombo.TabIndex = 93;
		this.lblMaxCombo.Text = "1234";
		// 
		// lblMaxComboT
		// 
		this.lblMaxComboT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblMaxComboT.AutoSize = true;
		this.lblMaxComboT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblMaxComboT.Location = new System.Drawing.Point(2, 51);
		this.lblMaxComboT.Name = "lblMaxComboT";
		this.lblMaxComboT.Size = new System.Drawing.Size(63, 13);
		this.lblMaxComboT.TabIndex = 92;
		this.lblMaxComboT.Text = "Max Combo";
		// 
		// lblMaxDamage
		// 
		this.lblMaxDamage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblMaxDamage.AutoSize = true;
		this.lblMaxDamage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblMaxDamage.Location = new System.Drawing.Point(84, 38);
		this.lblMaxDamage.Name = "lblMaxDamage";
		this.lblMaxDamage.Size = new System.Drawing.Size(31, 13);
		this.lblMaxDamage.TabIndex = 91;
		this.lblMaxDamage.Text = "1234";
		this.lblMaxDamage.Visible = false;
		// 
		// lblMaxDamageT
		// 
		this.lblMaxDamageT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblMaxDamageT.AutoSize = true;
		this.lblMaxDamageT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblMaxDamageT.Location = new System.Drawing.Point(2, 38);
		this.lblMaxDamageT.Name = "lblMaxDamageT";
		this.lblMaxDamageT.Size = new System.Drawing.Size(70, 13);
		this.lblMaxDamageT.TabIndex = 90;
		this.lblMaxDamageT.Text = "Max Damage";
		this.lblMaxDamageT.Visible = false;
		// 
		// lblComboDamage
		// 
		this.lblComboDamage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblComboDamage.AutoSize = true;
		this.lblComboDamage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblComboDamage.Location = new System.Drawing.Point(84, 25);
		this.lblComboDamage.Name = "lblComboDamage";
		this.lblComboDamage.Size = new System.Drawing.Size(31, 13);
		this.lblComboDamage.TabIndex = 89;
		this.lblComboDamage.Text = "1234";
		// 
		// lblComboDamageT
		// 
		this.lblComboDamageT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblComboDamageT.AutoSize = true;
		this.lblComboDamageT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblComboDamageT.Location = new System.Drawing.Point(2, 25);
		this.lblComboDamageT.Name = "lblComboDamageT";
		this.lblComboDamageT.Size = new System.Drawing.Size(83, 13);
		this.lblComboDamageT.TabIndex = 88;
		this.lblComboDamageT.Text = "Combo Damage";
		// 
		// lblLastDamage
		// 
		this.lblLastDamage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblLastDamage.AutoSize = true;
		this.lblLastDamage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblLastDamage.Location = new System.Drawing.Point(84, 12);
		this.lblLastDamage.Name = "lblLastDamage";
		this.lblLastDamage.Size = new System.Drawing.Size(31, 13);
		this.lblLastDamage.TabIndex = 87;
		this.lblLastDamage.Text = "1234";
		// 
		// lblLastDamageT
		// 
		this.lblLastDamageT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblLastDamageT.AutoSize = true;
		this.lblLastDamageT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblLastDamageT.Location = new System.Drawing.Point(2, 12);
		this.lblLastDamageT.Name = "lblLastDamageT";
		this.lblLastDamageT.Size = new System.Drawing.Size(70, 13);
		this.lblLastDamageT.TabIndex = 86;
		this.lblLastDamageT.Text = "Last Damage";
		// 
		// btnConfig
		// 
		this.btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.btnConfig.Location = new System.Drawing.Point(5, 118);
		this.btnConfig.Name = "btnConfig";
		this.btnConfig.Size = new System.Drawing.Size(59, 21);
		this.btnConfig.TabIndex = 85;
		this.btnConfig.Text = "Options";
		this.btnConfig.UseVisualStyleBackColor = true;
		this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
		// 
		// lblStatus
		// 
		this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblStatus.AutoSize = true;
		this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lblStatus.Location = new System.Drawing.Point(2, 74);
		this.lblStatus.Name = "lblStatus";
		this.lblStatus.Size = new System.Drawing.Size(37, 13);
		this.lblStatus.TabIndex = 84;
		this.lblStatus.Text = "debug";
		// 
		// btnClose
		// 
		this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.btnClose.Location = new System.Drawing.Point(70, 118);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(59, 21);
		this.btnClose.TabIndex = 83;
		this.btnClose.Text = "Close";
		this.btnClose.UseVisualStyleBackColor = true;
		this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
		// 
		// tableLayoutPanel1
		// 
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 165F));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
		this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
		this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 1);
		this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 0);
		this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 2;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(281, 438);
		this.tableLayoutPanel1.TabIndex = 81;
		// 
		// panel1
		// 
		this.panel1.Controls.Add(this.lblMaxCombo);
		this.panel1.Controls.Add(this.lblComboDamageT);
		this.panel1.Controls.Add(this.lblMaxComboT);
		this.panel1.Controls.Add(this.btnClose);
		this.panel1.Controls.Add(this.lblMaxDamage);
		this.panel1.Controls.Add(this.lblStatus);
		this.panel1.Controls.Add(this.lblMaxDamageT);
		this.panel1.Controls.Add(this.btnConfig);
		this.panel1.Controls.Add(this.lblComboDamage);
		this.panel1.Controls.Add(this.lblLastDamageT);
		this.panel1.Controls.Add(this.lblLastDamage);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 291);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(134, 144);
		this.panel1.TabIndex = 0;
		// 
		// panel2
		// 
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(143, 291);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(135, 144);
		this.panel2.TabIndex = 1;
		// 
		// panel3
		// 
		this.panel3.Controls.Add(this.dgvPlayer2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(143, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(135, 282);
		this.panel3.TabIndex = 2;
		// 
		// panel4
		// 
		this.panel4.Controls.Add(this.dgvPlayer1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(134, 282);
		this.panel4.TabIndex = 3;
		// 
		// Form2
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(281, 438);
		this.ControlBox = false;
		this.Controls.Add(this.tableLayoutPanel1);
		this.MaximizeBox = false;
		this.MinimizeBox = false;
		this.MinimumSize = new System.Drawing.Size(100, 100);
		this.Name = "Form2";
		((System.ComponentModel.ISupportInitialize)(this.dgvPlayer1)).EndInit();
		((System.ComponentModel.ISupportInitialize)(this.dgvPlayer2)).EndInit();
		this.tableLayoutPanel1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.ResumeLayout(false);

	}

	#endregion

	private System.Windows.Forms.DataGridView dgvPlayer1;
	private System.Windows.Forms.Label lblStatus;
	private System.Windows.Forms.Button btnClose;
	private System.Windows.Forms.Button btnConfig;
	private System.Windows.Forms.Label lblLastDamage;
	private System.Windows.Forms.Label lblLastDamageT;
	private System.Windows.Forms.Label lblMaxCombo;
	private System.Windows.Forms.Label lblMaxComboT;
	private System.Windows.Forms.Label lblMaxDamage;
	private System.Windows.Forms.Label lblMaxDamageT;
	private System.Windows.Forms.Label lblComboDamage;
	private System.Windows.Forms.Label lblComboDamageT;
	private System.Windows.Forms.DataGridView dgvPlayer2;
	private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	private System.Windows.Forms.Panel panel1;
	private System.Windows.Forms.Panel panel2;
	private System.Windows.Forms.Panel panel3;
	private System.Windows.Forms.Panel panel4;
	private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
	private System.Windows.Forms.DataGridViewImageColumn Column1;


}