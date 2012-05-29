// (C) Jan Boettcher, <http://www.ib-boettcher.de>
// Licensed under the EUPL V.1.1

using System;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorks.Interop.swconst;
using SolidWorksTools;

namespace DimensionRecorder
{
	/// <summary>
	/// Summary description for PMPage.
	/// </summary>
	public class DimRecPMPage
	{
		//Local Objects
		IPropertyManagerPage2 swPropertyPage;
		PMPHandler handler;
		ISldWorks iSwApp;
		DimensionRecorder userAddin;

		#region Property Manager Page Controls
		//Groups
        IPropertyManagerPageGroup groupDimensions;
        IPropertyManagerPageGroup groupObjects;
        IPropertyManagerPageGroup groupEvents;
		IPropertyManagerPageGroup groupOptions;

		//Controls
        internal IPropertyManagerPageCheckbox traceDialog = null;
        internal IPropertyManagerPageCheckbox traceDrag = null;
        internal IPropertyManagerPageCheckbox traceRegen = null;
        internal IPropertyManagerPageCheckbox traceSketch = null;
		internal IPropertyManagerPageCheckbox supprDouble = null;

        internal IPropertyManagerPageSelectionbox selectionDimensions;
        internal IPropertyManagerPageSelectionbox selectionObjects;

		//Control IDs
        public const int groupDimensionsID = 0;
        public const int groupObjectsID = 1;
        public const int groupEventsID = 2;
		public const int groupOptionsID		= 3;

        //Selection IDs
        public const int selectionDimensionsID = 10;
        public const int selectionObjectsID = 11;

        //OPtion IDs
        public const int cb_idSketch = 20;
		public const int cb_idDialog	= 21;
		public const int cb_idDrag		= 22;
		public const int optionDropDoubleID	= 23;
        public const int cb_idRegen = 24;
        #endregion

		public DimRecPMPage(DimensionRecorder addin)
		{
			userAddin = addin;
			iSwApp = (ISldWorks)userAddin.GetSldWorks();
			CreatePropertyManagerPage();
		}


		protected void CreatePropertyManagerPage()
		{
			int errors = -1;
			int options = (int)swPropertyManagerPageOptions_e.swPropertyManagerOptions_OkayButton |
				(int)swPropertyManagerPageOptions_e.swPropertyManagerOptions_CancelButton;

			handler = new PMPHandler(userAddin,this);
			swPropertyPage = (IPropertyManagerPage2)iSwApp.CreatePropertyManagerPage(Lang.DIMENSION_SELECTION, options, handler, ref errors);
			if (swPropertyPage != null && errors == (int)swPropertyManagerPageStatus_e.swPropertyManagerPage_Okay)
			{
				try
				{
					AddControls();
				}
				catch(Exception e)
				{
					iSwApp.SendMsgToUser2(e.Message,0,0);
				}
			}
		}


		//Controls are displayed on the page top to bottom in the order 
		//in which they are added to the object.
		protected void AddControls()
		{
			short controlType = -1;
			short align = -1;
			int options = -1;


			//Add the groups
			options = (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Expanded |
					  (int)swAddGroupBoxOptions_e.swGroupBoxOptions_Visible;


			groupDimensions = (IPropertyManagerPageGroup)swPropertyPage.AddGroupBox(groupDimensionsID, Lang.DIMENSIONS, options);
            groupObjects = (IPropertyManagerPageGroup)swPropertyPage.AddGroupBox(groupObjectsID, Lang.POINTS, options);
            groupEvents = (IPropertyManagerPageGroup)swPropertyPage.AddGroupBox(groupEventsID, Lang.EVENTS, options);
            groupOptions = (IPropertyManagerPageGroup)swPropertyPage.AddGroupBox(groupOptionsID, Lang.OPTIONS, options);


			//Selection of dimensions
			controlType = (int)swPropertyManagerPageControlType_e.swControlType_Selectionbox;
			align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
			options = (int)swAddControlOptions_e.swControlOptions_Enabled | 
				(int)swAddControlOptions_e.swControlOptions_Visible;

			selectionDimensions = (IPropertyManagerPageSelectionbox)groupDimensions.AddControl(selectionDimensionsID, controlType, Lang.RECORD_DIMENSIONS, align, options, Lang.RECORD_DIMENSIONS_HINT);
			if (selectionDimensions != null)
			{
				selectionDimensions.Height = 40;
				selectionDimensions.SetSelectionFilters(Target.dimensionFilters);
			}

            //Selection of objects
            controlType = (int)swPropertyManagerPageControlType_e.swControlType_Selectionbox;
            align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
            options = (int)swAddControlOptions_e.swControlOptions_Enabled |
                (int)swAddControlOptions_e.swControlOptions_Visible;

            selectionObjects = (IPropertyManagerPageSelectionbox)groupObjects.AddControl(selectionObjectsID, controlType, Lang.RECORD_POSITIONS, align, options, Lang.RECORD_POSITIONS_HINT);
            if (selectionObjects != null)
            {
                selectionObjects.Height = 40;
                selectionObjects.SetSelectionFilters(Target.objectFilters);
            }	


			//Trace options
			traceDialog = null;
			traceDrag = null;
            traceSketch = null;
            traceRegen = null;

			// Trace drag events
			controlType = (int)swPropertyManagerPageControlType_e.swControlType_Checkbox;
			align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
			options = (int)swAddControlOptions_e.swControlOptions_Enabled | 
			(int)swAddControlOptions_e.swControlOptions_Visible;

			traceDrag = (IPropertyManagerPageCheckbox)groupEvents.AddControl(cb_idDrag, controlType, Lang.DRAG, align, options, Lang.DRAG_HINT);

			// Trace dimension change events
			controlType = (int)swPropertyManagerPageControlType_e.swControlType_Checkbox;
			align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
			options = (int)swAddControlOptions_e.swControlOptions_Enabled | 
				(int)swAddControlOptions_e.swControlOptions_Visible;

            traceDialog = (IPropertyManagerPageCheckbox)groupEvents.AddControl(cb_idDialog, controlType, Lang.CHANGE_DIMENSION, align, options, Lang.CHANGE_DIMENSION_HINT);

			// Trace Sketch change events
			controlType = (int)swPropertyManagerPageControlType_e.swControlType_Checkbox;
			align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
			options = (int)swAddControlOptions_e.swControlOptions_Enabled | 
				(int)swAddControlOptions_e.swControlOptions_Visible;

            traceSketch = (IPropertyManagerPageCheckbox)groupEvents.AddControl(cb_idSketch, controlType, Lang.SKETCH_SOLVE, align, options, Lang.SKETCH_SOLVE_HINT);

            // Regenerate events
            controlType = (int)swPropertyManagerPageControlType_e.swControlType_Checkbox;
            align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
            options = (int)swAddControlOptions_e.swControlOptions_Enabled |
                (int)swAddControlOptions_e.swControlOptions_Visible;

            traceRegen = (IPropertyManagerPageCheckbox)groupEvents.AddControl(cb_idRegen, controlType, Lang.MODEL_REGENERATION, align, options, Lang.MODEL_REGENERATION_HINT);



			// Other Options
			// do not record double
			controlType = (int)swPropertyManagerPageControlType_e.swControlType_Checkbox;
			align = (int)swPropertyManagerPageControlLeftAlign_e.swControlAlign_LeftEdge;
			options = (int)swAddControlOptions_e.swControlOptions_Enabled | 
			(int)swAddControlOptions_e.swControlOptions_Visible;

            supprDouble = (IPropertyManagerPageCheckbox)groupOptions.AddControl(optionDropDoubleID, controlType, Lang.DROP_DOUBLE, align, options, Lang.DROP_DOUBLE_HINT);
			
		}

		public void Show()
		{
			swPropertyPage.Show();
		}
	}
}
