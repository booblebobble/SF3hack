using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SF3hack.Properties;

public partial class OptionsForm : Form
{
    public OptionsForm()
    {
		InitializeComponent();

		cbAction.SelectedIndex = Settings.Default.Action;
		cbGuard.SelectedIndex = Settings.Default.Guard;
		cbStand.SelectedIndex = Settings.Default.Stand;
		cbCounterHit.SelectedIndex = Settings.Default.CounterHit;
		cbStun.SelectedIndex = Settings.Default.Stun;
		cbSAGauge.SelectedIndex = Settings.Default.SAGauge;
		cbAttackData.SelectedIndex = Settings.Default.AttackData;
		cbInputDisplay.SelectedIndex = Settings.Default.InputDisplay;
		cbLagSimulation.SelectedIndex = Settings.Default.LagSimulation;
		cbLoadState.Checked = Settings.Default.LoadStateOnPlayback;

		toolTip1.SetToolTip(this.cbGuard, "This is sort of retarded at the moment, but it's something.");
		toolTip2.SetToolTip(this.cbSAGauge, "Infinite breaks timer supers like Genei Jin.  Use Refill instead.");
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Close();
    }

	private void OptionsForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		Settings.Default.Action = cbAction.SelectedIndex;
		Settings.Default.Guard = cbGuard.SelectedIndex;
		Settings.Default.Stand = cbStand.SelectedIndex;
		Settings.Default.CounterHit = cbCounterHit.SelectedIndex;
		Settings.Default.Stun = cbStun.SelectedIndex;
		Settings.Default.SAGauge = cbSAGauge.SelectedIndex;
		Settings.Default.AttackData = cbAttackData.SelectedIndex;
		Settings.Default.InputDisplay = cbInputDisplay.SelectedIndex;
		Settings.Default.LagSimulation = cbLagSimulation.SelectedIndex;
		Settings.Default.LoadStateOnPlayback = cbLoadState.Checked;
		Settings.Default.Save();

		KeyboardManager.LoadStateOnPlayback = Settings.Default.LoadStateOnPlayback;
	}

}
