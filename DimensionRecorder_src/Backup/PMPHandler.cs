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
	/// A User defined class that implements IPropertyManagerPage2Handler2
	/// </summary>
	public class PMPHandler : IPropertyManagerPage2Handler2
	{
		ISldWorks iSwApp;
		DimensionRecorder dimensionRecorder;
        DimRecPMPage myPage;

		public PMPHandler(DimensionRecorder addin, DimRecPMPage myPage)
		{
			this.dimensionRecorder = addin;
			this.iSwApp = (ISldWorks)dimensionRecorder.GetSldWorks();
            this.myPage = myPage;
		}

		//Implement these methods from the interface
		public void AfterClose()
		{
			int IndentSize;
			IndentSize = System.Diagnostics.Debug.IndentSize;
			System.Diagnostics.Debug.WriteLine(IndentSize);
		}

		public void OnCheckboxCheck(int id, bool status)
		{

		}

        /// <summary>
        /// Close Event Callback
        /// </summary>
        /// <param name="reason"></param>
		public void OnClose(int reason)
		{
			//This function must contain code, even if it does nothing, to prevent the
			//.NET runtime environment from doing garbage collection at the wrong time.
			int IndentSize;
			IndentSize = System.Diagnostics.Debug.IndentSize;
			System.Diagnostics.Debug.WriteLine(IndentSize);


			// Handle Selected Dimensions according to the close reason
			switch (reason) 
			{
				case ((int)swPropertyManagerPageCloseReasons_e.swPropertyManagerPageClose_Okay):
					goto case ((int)swPropertyManagerPageCloseReasons_e.swPropertyManagerPageClose_Apply);
				case ((int)swPropertyManagerPageCloseReasons_e.swPropertyManagerPageClose_Apply): 
 					//Store the given dimensions
					ModelDoc2 doc = (ModelDoc2)(this.iSwApp.ActiveDoc);
					SelectionMgr selM = (SelectionMgr)doc.SelectionManager;

					int selCount = selM.GetSelectedObjectCount();

					// Register the selected dimensions in the apps list
					for (int i = 1; i <= selCount; i++) 
					{
						object obj = selM.GetSelectedObject(i);
                        this.dimensionRecorder.registerTarget(obj);
					}

					// register the traced events
					dimensionRecorder.storeTraceSettings();
                   

					break;
// default				case ((int)swPropertyManagerPageCloseReasons_e.swPropertyManagerPageClose_Cancel):
// default				case ((int)swPropertyManagerPageCloseReasons_e.swPropertyManagerPageClose_UnknownReason): 
// default				case ((int)swPropertyManagerPageCloseReasons_e.swPropertyManagerPageClose_Closed):
// default				case ((int)swPropertyManagerPageCloseReasons_e.swPropertyManagerPageClose_UserEscape):
				default:
					this.dimensionRecorder.clearTargets();
					break;
			}
		}

		public void OnComboboxEditChanged(int id, string text)
		{

		}

		public int OnActiveXControlCreated(int id, bool status)
		{
			return -1;
		}

		public void OnButtonPress(int id)
		{

		}

		public void OnComboboxSelectionChanged(int id, int item)
		{

		}

		public void OnGroupCheck(int id, bool status)
		{

		}

		public void OnGroupExpand(int id, bool status)
		{

		}

		public bool OnHelp()
		{

			try 
			{
				System.Diagnostics.Process.Start(this.dimensionRecorder.helpPath);
			} 
			catch (Exception ex) 
			{
				System.Windows.Forms.MessageBox.Show("An error occurred: " + ex.Message +"\n(" + this.dimensionRecorder.helpPath + ")");
			}		

			return true;
		}

		public void OnListboxSelectionChanged(int id, int item)
		{

		}

		public bool OnNextPage()
		{
			return true;
		}

		public void OnNumberboxChanged(int id, double val)
		{

		}

		public void OnOptionCheck(int id)
		{

		}

		public bool OnPreviousPage()
		{
			return true;
		}

		public void OnSelectionboxCalloutCreated(int id)
		{

		}

		public void OnSelectionboxCalloutDestroyed(int id)
		{

		}

		public void OnSelectionboxFocusChanged(int id)
		{

		}

		public void OnSelectionboxListChanged(int id, int item)
		{

		}

		public void OnTextboxChanged(int id, string text)
		{

		}
	}
}
