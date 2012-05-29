// (C) Jan Boettcher, <http://www.ib-boettcher.de>
// Licensed under the EUPL V.1.1
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorks.Interop.swconst;
using SolidWorksTools;

namespace DimensionRecorder
{
    internal class Target
    {
        DimensionRecorder dimRec;
        string unitName;
        string targetName;
        internal Object targetObject;//Since the 'is' operator seems to be not expensive we can use is it to determine the object type at runtime
        //Owning component (only used for some trace objects)
        internal Component2 targetComponent; //
        List<TargetValue> values;
        UserUnit angUnit;
        UserUnit lengthUnit;

        internal Target(DimensionRecorder dimRec, Object targetObject)
        {
            //string[] possibleNames = ijbGeneral.COMInfoProvider.isAsignableToNames(targetObject,typeof(Dimension));
            if (!(targetObject is Dimension || targetObject is SketchPoint || targetObject is Feature))
            {


                throw new Exception("Invalid trace object type");
            }
            this.dimRec = dimRec;
            values = new List<TargetValue>();
            this.targetObject = targetObject;
            //model name
            string modelName = this.dimRec.iSwApp.IActiveDoc.GetTitle();
            modelName = Path.GetFileNameWithoutExtension(modelName);//!!!SW adds '.Part' or '.Assembly' to the full name!
            //Unit of this value (User units)
            this.angUnit = (UserUnit)this.dimRec.iSwApp.IActiveDoc.GetUserUnit((int)swUserUnitsType_e.swAngleUnit);
            this.lengthUnit = (UserUnit)this.dimRec.iSwApp.IActiveDoc.GetUserUnit((int)swUserUnitsType_e.swLengthUnit);
                //string angUnitName = angUnit.GetFullUnitName(false); //This returns eg. 'Grad'
            string angUnitName = this.angUnit.GetUnitsString(false); //This returns eg. 'Grad'
                //string lengthUnitName = lengthUnit.GetFullUnitName(false); //This returns eg. Millimeter'
            string lengthUnitName = this.lengthUnit.GetUnitsString(false); //This returns eg. 'mm'
            this.targetComponent = null;
            if (this.targetObject is Dimension)
            {
                Dimension dimension = (Dimension)targetObject;
                //Unit
                unitName = (dimension.GetType() == (int)swDimensionParamType_e.swDimensionParamTypeDoubleAngular) ? angUnitName : lengthUnitName;
                //Name
                targetName = dimension.FullName;

            }
            else if (this.targetObject is SketchPoint)
            {
                //Unit
                //Name
                SketchPoint sketchPoint = (SketchPoint)targetObject;
                int[] id = (int[])sketchPoint.GetID();
                Sketch sketch = sketchPoint.GetSketch();
                string sketchName = this.dimRec.iSwApp.IActiveDoc2.GetEntityName(sketch);
                targetName = string.Format("{0}{1}@{2}@{3}", Lang.POINT, id[1], sketchName, modelName);
            }
            else if (this.targetObject is Feature)
            {
                Feature feature = (Feature)targetObject;
                if (this.dimRec.iSwApp.IActiveDoc is AssemblyDoc)
                {
                    AssemblyDoc assy = (AssemblyDoc) this.dimRec.iSwApp.IActiveDoc;
                    Entity entity = (Entity)feature;
                    this.targetComponent = entity.IGetComponent2();
                }
                Object specificFeature = feature.GetSpecificFeature2();
                if (specificFeature != null && specificFeature is RefPoint) {
                    this.targetObject = specificFeature;
                    RefPoint refPoint = (RefPoint)this.targetObject;
                    string refPointName = this.dimRec.iSwApp.IActiveDoc2.GetEntityName(refPoint);
                    targetName = string.Format("{0}@{1}", refPointName, modelName);
                } else {
                    throw new Exception("Invalid trace object type");

                }
                //Unit
                unitName = lengthUnitName;
            }

        }

        internal static int[] objectFilters = { (int)swSelectType_e.swSelSKETCHPOINTS, (int)swSelectType_e.swSelDATUMPOINTS };
        internal static int[] dimensionFilters = { (int)swSelectType_e.swSelDIMENSIONS };
        /// <summary>
        /// Registers a new value
        /// </summary>
        /// <param name="index"></param>
        /// <returns>true if the value differs from the last registered value</returns>
        internal bool register(int index)
        {
            bool hasChanged = false;
            if (this.targetObject is Dimension)
            {
                Dimension dimension = (Dimension)targetObject;
                double[] values = (double[])dimension.GetValue3((int)swInConfigurationOpts_e.swThisConfiguration, null);//current config, user unit
                double value = values[0];
                if (this.values.Count == 0 || value != ((DimensionValue)(this.values[this.values.Count - 1])).value)
                {
                    //Is only registered if something changed
                    DimensionValue newValue = new DimensionValue(index, value);
                    this.values.Add(newValue);
                    hasChanged = true;
                }
            }
            else if (this.targetObject is SketchPoint)
            {
                SketchPoint sketchPoint = (SketchPoint)this.targetObject;
                //Positions come in system units -> convert to user units
                double faktor = this.lengthUnit.GetConversionFactor();
                double x = sketchPoint.X*faktor;
                double y = sketchPoint.Y*faktor;
                double z = sketchPoint.Z*faktor;
                if (this.values.Count == 0 || x != ((PositionValue)(this.values[this.values.Count - 1])).x || y != ((PositionValue)(this.values[this.values.Count - 1])).y || z != ((PositionValue)(this.values[this.values.Count - 1])).z)
                {
                    //Is only registered if something changed
                    PositionValue newValue = new PositionValue(index, x, y, z);
                    this.values.Add(newValue);
                    hasChanged = true;
                }

            }
            else if (this.targetObject is RefPoint)
            {
                RefPoint refPoint = (RefPoint)this.targetObject;
                MathPoint mathPoint = refPoint.GetRefPoint();

                //Transform the point into the top assemblies context if necessary
                if (this.targetComponent != null)
                {
                    MathUtility mu = (MathUtility)this.dimRec.iSwApp.GetMathUtility();
                    MathTransform mt = this.targetComponent.GetTotalTransform(false);
                    mathPoint = (MathPoint)mathPoint.MultiplyTransform(mt);
                }

                //Unit conversion 
                double faktor = this.lengthUnit.GetConversionFactor();
                double x = ((double[])mathPoint.ArrayData)[0] * faktor;
                double y = ((double[])mathPoint.ArrayData)[1] * faktor;
                double z = ((double[])mathPoint.ArrayData)[2] * faktor;

                if (this.values.Count == 0 || x != ((PositionValue)(this.values[this.values.Count - 1])).x || y != ((PositionValue)(this.values[this.values.Count - 1])).y || z != ((PositionValue)(this.values[this.values.Count - 1])).z)
                {
                    //Is only registered if something changed
                    PositionValue newValue = new PositionValue(index, x, y, z);
                    this.values.Add(newValue);
                    hasChanged = true;
                }

            }
            return hasChanged;
        }
        /// <summary>
        /// Clear all values
        /// </summary>
        internal void clear()
        {
            this.values.Clear();
        }
        /// <summary>
        /// Return the recorded values as an array of strings 
        /// 1. row: title 2. - nth row: values for timestamps
        /// Different columns for different subvalues (eg. 3 columns for a position (x,y,z-Value)
        /// </summary>
        /// <param name="indexStart">Timestamp-Index to start with</param>
        /// <param name="indexEnd">Timestamp-Index to end with</param>
        /// <returns></returns>
        internal void getResultsAsArrays(int indexStart, int indexEnd, out string[,] titles, out double[,] values)
        {
            int nrOfDataRows = indexEnd - indexStart + 1;
            int nrOfTitleRows = 2;
            int nrOfCols = 0;
            if (this.targetObject is Dimension) nrOfCols = 1;
            else if (this.targetObject is SketchPoint || this.targetObject is RefPoint) nrOfCols = 3;
            titles = new string[nrOfTitleRows, nrOfCols];
            values = new double[nrOfDataRows, nrOfCols];
            //The title rows
            if (this.targetObject is Dimension)
            {
                //The full name
                titles[0, 0] = this.targetName;
                //The unit
                titles[1, 0] = this.unitName;
            }
            else if (this.targetObject is SketchPoint || this.targetObject is RefPoint)
            {
                //The full name
                titles[0, 0] = "x@" + this.targetName;
                titles[0, 1] = "y@" + this.targetName;
                titles[0, 2] = "z@" + this.targetName;
                //The unit
                titles[1, 0] = this.unitName;
                titles[1, 1] = this.unitName;
                titles[1, 2] = this.unitName;
                
            }
            //There must be one value at least
            int j = 0;
            //TargetValue nextVal = this.values[j];
            //Start loop from zero to get the missing values, but record only the values within the given index range
            for (int i = 0; i <= indexEnd; i++)
            {
                if (j + 1 < this.values.Count && this.values[j + 1].index == i) j++;
                if (i < indexStart) continue;
                int rowIndex = i - indexStart;
                if (this.targetObject is Dimension)
                {
                    DimensionValue val = (DimensionValue)(this.values[j]);
                    values[rowIndex, 0] = val.value;

                }
                else if (this.targetObject is SketchPoint || this.targetObject is RefPoint)
                {
                    PositionValue val = (PositionValue)(this.values[j]);
                    values[rowIndex, 0] = val.x;
                    values[rowIndex, 1] = val.y;
                    values[rowIndex, 2] = val.z;
                }
            }
            return;
        }
    }
    internal abstract class TargetValue
    {
        internal int index;
    }
    internal class PositionValue : TargetValue
    {
        internal double x;
        internal double y;
        internal double z;
        internal PositionValue(int index, double x, double y, double z)
        {
            this.index = index;
            this.x = x;
            this.y = y;
            this.z = z;
        }

    }
    internal class DimensionValue : TargetValue
    {
        internal double value;
        internal DimensionValue(int index, double value)
        {
            this.index = index;
            this.value = value;
        }

    }
}
