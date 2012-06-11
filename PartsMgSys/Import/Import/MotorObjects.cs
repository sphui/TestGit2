using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Windows.Forms;

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
        #endregion
    }
}
