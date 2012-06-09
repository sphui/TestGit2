using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
