partial class OptionsForm
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
		this.button1 = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cbAction = new System.Windows.Forms.ComboBox();
		this.cbGuard = new System.Windows.Forms.ComboBox();
		this.cbStand = new System.Windows.Forms.ComboBox();
		this.cbCounterHit = new System.Windows.Forms.ComboBox();
		this.cbStun = new System.Windows.Forms.ComboBox();
		this.cbSAGauge = new System.Windows.Forms.ComboBox();
		this.cbAttackData = new System.Windows.Forms.ComboBox();
		this.cbInputDisplay = new System.Windows.Forms.ComboBox();
		this.cbLagSimulation = new System.Windows.Forms.ComboBox();
		this.cbLoadState = new System.Windows.Forms.CheckBox();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
		this.SuspendLayout();
		// 
		// button1
		// 
		this.button1.Location = new System.Drawing.Point(12, 280);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(159, 23);
		this.button1.TabIndex = 0;
		this.button1.Text = "Done";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(this.button1_Click);
		// 
		// label1
		// 
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(12, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(37, 13);
		this.label1.TabIndex = 1;
		this.label1.Text = "Action";
		// 
		// label2
		// 
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(12, 37);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(36, 13);
		this.label2.TabIndex = 2;
		this.label2.Text = "Guard";
		// 
		// label3
		// 
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(12, 65);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(35, 13);
		this.label3.TabIndex = 3;
		this.label3.Text = "Stand";
		// 
		// label4
		// 
		this.label4.AutoSize = true;
		this.label4.Enabled = false;
		this.label4.Location = new System.Drawing.Point(12, 93);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(60, 13);
		this.label4.TabIndex = 4;
		this.label4.Text = "Counter Hit";
		// 
		// label5
		// 
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(12, 121);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(29, 13);
		this.label5.TabIndex = 5;
		this.label5.Text = "Stun";
		// 
		// label6
		// 
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(12, 149);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(56, 13);
		this.label6.TabIndex = 6;
		this.label6.Text = "SA Gauge";
		// 
		// label7
		// 
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(12, 177);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(64, 13);
		this.label7.TabIndex = 7;
		this.label7.Text = "Attack Data";
		// 
		// label8
		// 
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(12, 205);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(68, 13);
		this.label8.TabIndex = 8;
		this.label8.Text = "Input Display";
		// 
		// label9
		// 
		this.label9.AutoSize = true;
		this.label9.Enabled = false;
		this.label9.Location = new System.Drawing.Point(12, 233);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(76, 13);
		this.label9.TabIndex = 9;
		this.label9.Text = "Lag Simulation";
		// 
		// cbAction
		// 
		this.cbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbAction.FormattingEnabled = true;
		this.cbAction.Items.AddRange(new object[] {
            "Stand",
            "Crouch",
            "Jump",
            "Human"});
		this.cbAction.Location = new System.Drawing.Point(94, 6);
		this.cbAction.Name = "cbAction";
		this.cbAction.Size = new System.Drawing.Size(77, 21);
		this.cbAction.TabIndex = 10;
		// 
		// cbGuard
		// 
		this.cbGuard.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbGuard.FormattingEnabled = true;
		this.cbGuard.Items.AddRange(new object[] {
            "No Block",
            "Auto Block",
            "All Block",
            "Random"});
		this.cbGuard.Location = new System.Drawing.Point(94, 34);
		this.cbGuard.Name = "cbGuard";
		this.cbGuard.Size = new System.Drawing.Size(77, 21);
		this.cbGuard.TabIndex = 11;
		// 
		// cbStand
		// 
		this.cbStand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbStand.FormattingEnabled = true;
		this.cbStand.Items.AddRange(new object[] {
            "Normal",
            "Quick",
            "Random"});
		this.cbStand.Location = new System.Drawing.Point(94, 62);
		this.cbStand.Name = "cbStand";
		this.cbStand.Size = new System.Drawing.Size(77, 21);
		this.cbStand.TabIndex = 12;
		// 
		// cbCounterHit
		// 
		this.cbCounterHit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbCounterHit.Enabled = false;
		this.cbCounterHit.FormattingEnabled = true;
		this.cbCounterHit.Items.AddRange(new object[] {
            "Off",
            "On",
            "Random"});
		this.cbCounterHit.Location = new System.Drawing.Point(94, 90);
		this.cbCounterHit.Name = "cbCounterHit";
		this.cbCounterHit.Size = new System.Drawing.Size(77, 21);
		this.cbCounterHit.TabIndex = 13;
		// 
		// cbStun
		// 
		this.cbStun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbStun.FormattingEnabled = true;
		this.cbStun.Items.AddRange(new object[] {
            "Normal",
            "None"});
		this.cbStun.Location = new System.Drawing.Point(94, 118);
		this.cbStun.Name = "cbStun";
		this.cbStun.Size = new System.Drawing.Size(77, 21);
		this.cbStun.TabIndex = 14;
		// 
		// cbSAGauge
		// 
		this.cbSAGauge.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbSAGauge.FormattingEnabled = true;
		this.cbSAGauge.Items.AddRange(new object[] {
            "Normal",
            "Infinite",
            "Refill"});
		this.cbSAGauge.Location = new System.Drawing.Point(94, 146);
		this.cbSAGauge.Name = "cbSAGauge";
		this.cbSAGauge.Size = new System.Drawing.Size(77, 21);
		this.cbSAGauge.TabIndex = 15;
		// 
		// cbAttackData
		// 
		this.cbAttackData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbAttackData.FormattingEnabled = true;
		this.cbAttackData.Items.AddRange(new object[] {
            "Off",
            "On"});
		this.cbAttackData.Location = new System.Drawing.Point(94, 174);
		this.cbAttackData.Name = "cbAttackData";
		this.cbAttackData.Size = new System.Drawing.Size(77, 21);
		this.cbAttackData.TabIndex = 16;
		// 
		// cbInputDisplay
		// 
		this.cbInputDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbInputDisplay.FormattingEnabled = true;
		this.cbInputDisplay.Items.AddRange(new object[] {
            "Off",
            "On"});
		this.cbInputDisplay.Location = new System.Drawing.Point(94, 202);
		this.cbInputDisplay.Name = "cbInputDisplay";
		this.cbInputDisplay.Size = new System.Drawing.Size(77, 21);
		this.cbInputDisplay.TabIndex = 17;
		// 
		// cbLagSimulation
		// 
		this.cbLagSimulation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cbLagSimulation.Enabled = false;
		this.cbLagSimulation.FormattingEnabled = true;
		this.cbLagSimulation.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
		this.cbLagSimulation.Location = new System.Drawing.Point(94, 230);
		this.cbLagSimulation.Name = "cbLagSimulation";
		this.cbLagSimulation.Size = new System.Drawing.Size(77, 21);
		this.cbLagSimulation.TabIndex = 18;
		// 
		// cbLoadState
		// 
		this.cbLoadState.AutoSize = true;
		this.cbLoadState.Location = new System.Drawing.Point(12, 257);
		this.cbLoadState.Name = "cbLoadState";
		this.cbLoadState.Size = new System.Drawing.Size(155, 17);
		this.cbLoadState.TabIndex = 19;
		this.cbLoadState.Text = "Load state before playback";
		this.cbLoadState.UseVisualStyleBackColor = true;
		// 
		// toolTip1
		// 
		this.toolTip1.UseAnimation = false;
		// 
		// OptionsForm
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(181, 311);
		this.Controls.Add(this.cbLoadState);
		this.Controls.Add(this.cbLagSimulation);
		this.Controls.Add(this.cbInputDisplay);
		this.Controls.Add(this.cbAttackData);
		this.Controls.Add(this.cbSAGauge);
		this.Controls.Add(this.cbStun);
		this.Controls.Add(this.cbCounterHit);
		this.Controls.Add(this.cbStand);
		this.Controls.Add(this.cbGuard);
		this.Controls.Add(this.cbAction);
		this.Controls.Add(this.label9);
		this.Controls.Add(this.label8);
		this.Controls.Add(this.label7);
		this.Controls.Add(this.label6);
		this.Controls.Add(this.label5);
		this.Controls.Add(this.label4);
		this.Controls.Add(this.label3);
		this.Controls.Add(this.label2);
		this.Controls.Add(this.label1);
		this.Controls.Add(this.button1);
		this.MaximizeBox = false;
		this.MinimizeBox = false;
		this.Name = "OptionsForm";
		this.ShowInTaskbar = false;
		this.Text = "Training Options";
		this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OptionsForm_FormClosing);
		this.ResumeLayout(false);
		this.PerformLayout();

    }

    #endregion

	private System.Windows.Forms.Button button1;
	private System.Windows.Forms.Label label1;
	private System.Windows.Forms.Label label2;
	private System.Windows.Forms.Label label3;
	private System.Windows.Forms.Label label4;
	private System.Windows.Forms.Label label5;
	private System.Windows.Forms.Label label6;
	private System.Windows.Forms.Label label7;
	private System.Windows.Forms.Label label8;
	private System.Windows.Forms.Label label9;
	private System.Windows.Forms.ComboBox cbAction;
	private System.Windows.Forms.ComboBox cbGuard;
	private System.Windows.Forms.ComboBox cbStand;
	private System.Windows.Forms.ComboBox cbCounterHit;
	private System.Windows.Forms.ComboBox cbStun;
	private System.Windows.Forms.ComboBox cbSAGauge;
	private System.Windows.Forms.ComboBox cbAttackData;
	private System.Windows.Forms.ComboBox cbInputDisplay;
	private System.Windows.Forms.ComboBox cbLagSimulation;
	private System.Windows.Forms.CheckBox cbLoadState;
	private System.Windows.Forms.ToolTip toolTip1;
	private System.Windows.Forms.ToolTip toolTip2;
}