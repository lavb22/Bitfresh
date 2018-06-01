using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bitfresh_Core;

namespace Bitfresh_WindowsForm
{
    public partial class Configure : Form
    {
        public int frec, age;
        private Button cntc, conf, pbtn;

        public Configure(Button conbutton, Button confbutton, Button ProfBtn)
        {
            InitializeComponent();

            frecValue.Minimum = Constants.MinFrec;
            frecValue.Maximum = Constants.MaxFrec;

            ageValue.Minimum = Constants.MinAge;
            ageValue.Maximum = Constants.MaxAge;

            frecValue.Value = (int)Properties.Settings.Default["frecuency"];
            ageValue.Value = (int)Properties.Settings.Default["age"];
            cntc = conbutton;
            conf = confbutton;
            pbtn = ProfBtn;
            this.Icon = System.Drawing.Icon.FromHandle(Properties.Resources.gear.GetHicon());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            frecValue.Value = (int)Properties.Settings.Default["frecuency"];
            ageValue.Value = (int)Properties.Settings.Default["age"];
            this.enableBttons();
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["frecuency"] = (int)frecValue.Value;
            Properties.Settings.Default["age"] = (int)ageValue.Value;
            Properties.Settings.Default.Save();
            this.enableBttons();
            this.Hide();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            frecValue.Value = (int)Properties.Settings.Default["frecuency"];
            ageValue.Value = (int)Properties.Settings.Default["age"];
            this.enableBttons();
            this.Hide();
        }

        private void enableBttons()
        {
            cntc.Enabled = true;
            conf.Enabled = true;
            pbtn.Enabled = true;
        }
    }
}

