using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Windows.Forms;
using System.Data;

namespace Import
{
    public class PartObject
    {
        #region Private Member
        private Dictionary<string, object> mProperties;
        #endregion

        #region Constructor
        public PartObject(string serialnumber)
        {
            SerialNumber = serialnumber;
            mProperties = new Dictionary<string, object>();
        }
        #endregion

        #region Accessors
        public string SerialNumber
        {
            get;
            set;
        }

        public Dictionary<string, object> PartProperties
        {
            get { return mProperties; }
            set {mProperties = value;}
        }
        #endregion

        #region public methods
        public BsonDocument ToMongoDocument()
        {
            BsonDocument partdoc = new BsonDocument();
            partdoc["PartSerialNumber"] = SerialNumber;

            BsonDocument propsdoc = new BsonDocument();
            propsdoc.Add(PartProperties);
            partdoc["PartProperties"] = propsdoc;
            return partdoc;
        }

        public bool FromMongoDocument(BsonDocument mongodoc)
        {
            SerialNumber = mongodoc.GetValue("PartSerialNumber").AsString;
            BsonDocument propsdoc = mongodoc.GetValue("PartProperties").AsBsonDocument;
            PartProperties = propsdoc.ToDictionary();
            return true;
        }
        #endregion
    }

    public class MotorObject
    {
        #region Private Member
        private List<PartObject> mParts;
        #endregion

        #region Constructor
        public MotorObject(string serialnumber)
        {
            mParts = new List<PartObject>();
            SerialNumber = serialnumber;
        }
        #endregion

        #region Accessors
        public string SerialNumber
        {
            get;
            set;
        }

        public List<PartObject> Parts
        {
            get { return mParts; }
            set { mParts = value; }
        }
        #endregion

        #region public methods
        public BsonDocument ToMongoDocument()
        {
            BsonDocument motordoc = new BsonDocument();
            motordoc["MotorSerialNumber"] = SerialNumber;

            BsonDocument partsdoc = new BsonDocument();
            int index = 0;
            foreach (PartObject part in Parts)
            {
                partsdoc["Part" + index.ToString()] = part.ToMongoDocument();
                index++;
            }
            motordoc["Parts"] = partsdoc;

            return motordoc;
        }

        public bool FromMongoDocument(BsonDocument mongodoc)
        {
            SerialNumber = mongodoc.GetValue("MotorSerialNumber").AsString;
            BsonDocument partsdoc = mongodoc.GetValue("Parts").AsBsonDocument;
            foreach (BsonValue partvar in partsdoc.Values)
            {
                BsonDocument partdoc = partvar as BsonDocument;
                PartObject part = new PartObject(string.Empty);
                part.FromMongoDocument(partdoc);
                Parts.Add(part);
            }
            return true;
        }

        public bool FromDataGridView(DataGridView datagridview)
        {
            List<string> clmnnames = new List<string>();
            foreach (DataGridViewColumn clmn in datagridview.Columns)
            {
                clmnnames.Add(clmn.Name);
            }

            int partnoindex = 0;
            if (!clmnnames.Contains("件号"))
            {
                System.Windows.Forms.MessageBox.Show("Didn't find part no!");
                return false;
            }
            else
            {
                partnoindex = clmnnames.IndexOf("件号");
                clmnnames.RemoveAt(partnoindex);
            }

            foreach (DataGridViewRow datarow in datagridview.Rows)
            {
                string partno = datarow.Cells[partnoindex].Value as string;
                if(string.IsNullOrEmpty(partno))
                    continue;
                PartObject part = new PartObject(datarow.Cells[partnoindex].Value as string);
                foreach (string clmnname in clmnnames)
                {
                    object cellvalue = datarow.Cells[clmnname].Value;
                    part.PartProperties.Add(clmnname, cellvalue == null || 
                        cellvalue is System.DBNull? string.Empty : cellvalue);
                }
                this.Parts.Add(part);
            }
            return true;
        }

        public DataTable ToDataTable()
        {
            if (Parts.Count == 0)
                return null;
            DataTable table = new DataTable(SerialNumber);
            PartObject part1 = Parts[0];
            foreach (string strkey in part1.PartProperties.Keys)
            {
                table.Columns.Add(strkey);
            }

            table.Columns.Add("件号");
            foreach (PartObject part in Parts)
            {
                Dictionary<string, object> dictoTable = new
                        Dictionary<string, object>(part.PartProperties);
                dictoTable["件号"] = part.SerialNumber;
                table.Rows.Add(dictoTable.Values.ToArray());
            }
            return table;
        }
        #endregion
    }
}
