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
using ADOX;
using MongoDB.Driver;

namespace Import
{
    public partial class InputForm1 : Form
    {
        public InputForm1()
        {
            InitializeComponent();
        }

        public static string SelectedTable = string.Empty;

        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            fdlg.Title = "Select file";
            fdlg.InitialDirectory = @"c:\";
            fdlg.FileName = txtMotorNO.Text;
            fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
            fdlg.FilterIndex = 1;
            fdlg.RestoreDirectory = true;
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                txtMotorNO.Text = fdlg.FileName;
                Import();
                Application.DoEvents();
            }
        }

        private void Import()
        {
            if (txtMotorNO.Text.Trim() != string.Empty)
            {
                try
                {
                    string[] strTables = GetTableExcel(txtMotorNO.Text);

                    frmSelectTables objSelectTable = new frmSelectTables(strTables);
                    objSelectTable.ShowDialog(this);
                    objSelectTable.Dispose();
                    if ((SelectedTable != string.Empty) && (SelectedTable != null))
                    {
                        DataTable dt = GetDataTableExcel(txtMotorNO.Text, SelectedTable);
                        List<DataColumn> emptyColumns = new List<DataColumn>();
                        dt.Columns.Add(new DataColumn(Properties.Resources.DeliverCase, typeof(string)));
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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == Properties.Resources.DeliverCase)
            {
                DataGridViewRow datarow = dataGridView1.Rows[e.RowIndex];
                if (datarow == null)
                    return;
                DataGridViewCell datacell = datarow.Cells[e.ColumnIndex];
                if (datacell == null)
                    return;
                if(string.Compare(datacell.Value as string, Properties.Resources.Delived) != 0 &&
                    string.Compare(datacell.Value as string, Properties.Resources.NotDeliver) != 0 &&
                    string.Compare(datacell.Value as string, Properties.Resources.DeliveredByBorrow) != 0 &&
                    string.Compare(datacell.Value as string, Properties.Resources.NotDeliverByBorrow) != 0)
                {
                    System.Windows.Forms.MessageBox.Show(Properties.Resources.InputwrongForDeliver);
                    datacell.Value = string.Empty;
                }
            }
        }

        private void buttonSaveDataBase_Click(object sender, EventArgs e)
        {
            var mongo = MongoServer.Create();

            var db = mongo.GetDatabase("PartsManage");
            var motorsdb = db.GetCollection("Motors");

            PartObject part = new PartObject("test1s");
            part.PartProperties.Add("a", "1");
            part.PartProperties.Add("b", string.Empty);
            MotorObject motor = new MotorObject("testmotor");
            motor.Parts.Add(part);

            //var json = new MongoJson();
            //var firstNoteDocument = json.DocumentFrom(motor);
            motorsdb.Insert(motor.ToMongoDocument());
        }

    }
}