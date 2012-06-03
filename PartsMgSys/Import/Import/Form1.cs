using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Windows.Documents;
using System.Windows.Controls;
using ADOX;
namespace Import
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string SelectedTable = string.Empty;

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select file";
            fdlg.InitialDirectory = @"c:\";
            fdlg.FileName = txtFileName.Text;
            fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = fdlg.FileName;
                Import();
                Application.DoEvents();
            }
        }

        private void Import()
        {
            if (txtFileName.Text.Trim() != string.Empty)
            {
                try
                {
                    string[] strTables = GetTableExcel(txtFileName.Text);

                    frmSelectTables objSelectTable = new frmSelectTables(strTables);
                    objSelectTable.ShowDialog(this);
                    objSelectTable.Dispose();
                    if ((SelectedTable != string.Empty) && (SelectedTable != null))
                    {
                        DataTable dt = GetDataTableExcel(txtFileName.Text, SelectedTable);
                        dataGridView1.DataSource = dt.DefaultView;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        public static DataTable GetDataTableExcel(string strFileName, string Table)
        {
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + strFileName + "; Extended Properties = \"Excel 8.0;HDR=Yes;IMEX=1\";");
            conn.Open();
            string strQuery = "SELECT * FROM [" + Table + "]";
            System.Data.OleDb.OleDbDataAdapter adapter = new System.Data.OleDb.OleDbDataAdapter(strQuery, conn);
            System.Data.DataSet ds = new System.Data.DataSet();
            adapter.Fill(ds);
            return ds.Tables[0];
        }

        public static string[] GetTableExcel(string strFileName)
        {
            string[] strTables = new string[100];
            Catalog oCatlog = new Catalog();
            ADOX.Table oTable = new ADOX.Table();
            ADODB.Connection oConn = new ADODB.Connection();
            oConn.Open("Provider=Microsoft.Jet.OleDb.4.0; Data Source = " + strFileName + "; Extended Properties = \"Excel 8.0;HDR=Yes;IMEX=1\";", "", "", 0);
            oCatlog.ActiveConnection = oConn;
            if (oCatlog.Tables.Count > 0)
            {
                int item = 0;
                foreach (ADOX.Table tab in oCatlog.Tables)
                {
                    if (tab.Type == "TABLE")
                    {
                        strTables[item] = tab.Name;
                        item++;
                    }
                }
            }
            return strTables;
        }

    }
}