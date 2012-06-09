namespace Import
{
    partial class InputForm1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputForm1));
            this.buttonImport = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtMotorNO = new System.Windows.Forms.TextBox();
            this.lableMotorNO = new System.Windows.Forms.Label();
            this.buttonAddRow = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            this.buttonAddColumn = new System.Windows.Forms.Button();
            this.buttonSaveDataBase = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonImport
            // 
            resources.ApplyResources(this.buttonImport, "buttonImport");
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            // 
            // txtMotorNO
            // 
            resources.ApplyResources(this.txtMotorNO, "txtMotorNO");
            this.txtMotorNO.Name = "txtMotorNO";
            // 
            // lableMotorNO
            // 
            resources.ApplyResources(this.lableMotorNO, "lableMotorNO");
            this.lableMotorNO.Name = "lableMotorNO";
            // 
            // buttonAddRow
            // 
            resources.ApplyResources(this.buttonAddRow, "buttonAddRow");
            this.buttonAddRow.Name = "buttonAddRow";
            this.buttonAddRow.UseVisualStyleBackColor = true;
            // 
            // buttonNew
            // 
            resources.ApplyResources(this.buttonNew, "buttonNew");
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.UseVisualStyleBackColor = true;
            // 
            // buttonAddColumn
            // 
            resources.ApplyResources(this.buttonAddColumn, "buttonAddColumn");
            this.buttonAddColumn.Name = "buttonAddColumn";
            this.buttonAddColumn.UseVisualStyleBackColor = true;
            // 
            // buttonSaveDataBase
            // 
            resources.ApplyResources(this.buttonSaveDataBase, "buttonSaveDataBase");
            this.buttonSaveDataBase.Name = "buttonSaveDataBase";
            this.buttonSaveDataBase.UseVisualStyleBackColor = true;
            this.buttonSaveDataBase.Click += new System.EventHandler(this.buttonSaveDataBase_Click);
            // 
            // InputForm1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSaveDataBase);
            this.Controls.Add(this.buttonAddColumn);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.buttonAddRow);
            this.Controls.Add(this.txtMotorNO);
            this.Controls.Add(this.lableMotorNO);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.buttonImport);
            this.Name = "InputForm1";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonImport;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtMotorNO;
        private System.Windows.Forms.Label lableMotorNO;
        private System.Windows.Forms.Button buttonAddRow;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonAddColumn;
        private System.Windows.Forms.Button buttonSaveDataBase;
    }
}

