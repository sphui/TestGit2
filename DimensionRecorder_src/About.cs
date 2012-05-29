// (C) Jan Boettcher, <http://www.ib-boettcher.de>
// Licensed under the EUPL V.1.1

using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DimensionRecorder
{
	/// <summary>
	/// The About window
	/// </summary>
	public class About : System.Windows.Forms.Form
	{
		private DimensionRecorder dimRec;

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.LinkLabel linkLabel2;
		private System.Windows.Forms.HelpProvider helpProvider1;
		private System.ComponentModel.Container components = null;

		public About(DimensionRecorder dimRec)
		{
			this.dimRec = dimRec;
			InitializeComponent();
			this.linkLabel1.LinkClicked +=new LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
            this.linkLabel1.Links[0].LinkData = "http://www.stokerobot.com/";
			this.linkLabel2.LinkClicked +=new LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked);
            this.linkLabel2.Links[0].LinkData = "mailto:master@stokerobot.com";
			System.Reflection.Assembly assy = System.Reflection.Assembly.GetExecutingAssembly();
			System.Reflection.AssemblyName aName = assy.GetName();
			System.Version vers = aName.Version;
			string version = vers.ToString();
			object[] atts = assy.GetCustomAttributes(true);
			this.label3.Text = "v" + version;
			this.helpProvider1.HelpNamespace = this.dimRec.helpPath;
            //License Window
            //Read credits text
            string credPath = Path.Combine(dimRec.assyDir, "credits.txt");
            StreamReader sr = new StreamReader(credPath, System.Text.Encoding.Default);
            string credText = sr.ReadToEnd();
            sr.Close();
            //Read license text from file
            string licPath = Path.Combine(dimRec.assyDir,Lang.LICENSE_FILE);
            sr = new StreamReader(licPath, System.Text.Encoding.Default);
            string licText = sr.ReadToEnd();
            sr.Close();

            this.textBox1.Text = credText + Environment.NewLine + Lang.LICENSE + ":" + Environment.NewLine + Environment.NewLine + licText;
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Vom Windows Form-Designer generierter Code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(445, 240);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 20);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ok";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(192, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 28);
            this.label2.TabIndex = 3;
            this.label2.Text = "DimensionRecorder";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(9, 218);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(197, 28);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://www.stokerobot.com/";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(178, 204);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Arial", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(744, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 28);
            this.label3.TabIndex = 6;
            this.label3.Text = ">Version<";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(196, 45);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(655, 170);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = ">Text<";
            this.textBox1.WordWrap = false;
            // 
            // linkLabel2
            // 
            this.linkLabel2.Location = new System.Drawing.Point(21, 246);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(146, 14);
            this.linkLabel2.TabIndex = 8;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "master@stokerobot.com";
            this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // helpProvider1
            // 
            this.helpProvider1.HelpNamespace = "";
            // 
            // About
            // 
            this.AcceptButton = this.button1;
            this.ClientSize = new System.Drawing.Size(858, 274);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.helpProvider1.SetHelpKeyword(this, "");
            this.helpProvider1.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.TableOfContents);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "About";
            this.helpProvider1.SetShowHelp(this, true);
            this.Text = "DimensionRecorder";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try 
			{
				System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
			} 
			catch (Exception ex) 
			{
				System.Windows.Forms.MessageBox.Show("An error occurred: " + ex.Message);
			}



		}

		private void linkLabel2_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try 
			{
				System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
			} 
			catch (Exception ex) 
			{
				System.Windows.Forms.MessageBox.Show("An error occurred: " + ex.Message);
			}		
		}

		
	}
}
