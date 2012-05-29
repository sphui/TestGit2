// (C) Jan Boettcher, <http://www.ib-boettcher.de>
// Licensed under the EUPL V.1.1
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DimensionRecorder
{
    public partial class TagRequestForm : Form
    {
        DimensionRecorder dimRec;

        internal TagRequestForm(DimensionRecorder dimRec)
        {
            this.dimRec = dimRec;

            InitializeComponent();

            this.buttonOk.Text = Lang.OK;
            this.buttonCancel.Text = Lang.CANCEL;
            this.Text = Lang.ENTER_TAG;

        }
        internal string getTag(List<string> tags)
        {
            this.comboBox1.Items.Clear();
            this.comboBox1.Items.AddRange(tags.ToArray());
            DialogResult res = this.ShowDialog();
            if (res == DialogResult.OK) return this.comboBox1.Text;
            else
            {
                this.comboBox1.Text="";
                return null;
            }
        }

        private void dialogShown(object sender, EventArgs e)
        {
            this.comboBox1.Focus();
        }
    }
}
