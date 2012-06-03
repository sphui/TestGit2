namespace Import
{
    partial class frmSelectTables
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectTables));
            this.lstViewTables = new System.Windows.Forms.ListView();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstViewTables
            // 
            this.lstViewTables.Location = new System.Drawing.Point(7, 9);
            this.lstViewTables.Name = "lstViewTables";
            this.lstViewTables.Size = new System.Drawing.Size(224, 306);
            this.lstViewTables.TabIndex = 0;
            this.lstViewTables.UseCompatibleStateImageBehavior = false;
            this.lstViewTables.View = System.Windows.Forms.View.List;
            this.lstViewTables.SelectedIndexChanged += new System.EventHandler(this.lstViewTables_SelectedIndexChanged);
            this.lstViewTables.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstViewTables_ItemSelectionChanged);
            // 
            // btnOk
            // 
            this.btnOk.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOk.BackgroundImage")));
            this.btnOk.Location = new System.Drawing.Point(165, 316);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(68, 33);
            this.btnOk.TabIndex = 1;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmSelectTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(231)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(237, 349);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lstViewTables);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectTables";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Tables";
            this.Load += new System.EventHandler(this.Select_Tables_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstViewTables;
        private System.Windows.Forms.Button btnOk;
    }
}