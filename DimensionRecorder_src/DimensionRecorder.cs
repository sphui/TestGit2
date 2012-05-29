// (C) Jan Boettcher, <http://www.ib-boettcher.de>
// Licensed under the EUPL V.1.1
using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;
using System.Collections;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorks.Interop.swconst;
using SolidWorksTools;
using Microsoft.Office.Interop.Excel;


namespace DimensionRecorder
{


    /// <summary>
    /// Summary description for DimensionRecorder. for testing
    /// </summary>
    [Guid("170A8FB9-A9A1-49BC-8A4E-71BE2AFCC3D4")]
    public class DimensionRecorder : ISwAddin
    {
        #region Assembly Variables
        /// <summary>
        /// The AddIn
        /// </summary>
        internal ISldWorks iSwApp;
        /// <summary>
        /// The installation directory
        /// </summary>
        internal string assyDir;
        /// <summary>
        /// Full name of the help file
        /// </summary>
        internal string helpPath;

        #endregion

        #region Local Variables
        SldWorks SwEventPtr;
        Hashtable openDocs;
        int addinID;
        int toolbarID;
        DimRecPMPage ppage;
        About aboutDialog;
        TagRequestForm tagDialog;

        //Callbacks
        internal delegate void MenuCallback();

        /// <summary>
        /// Flag to indicate if the dimensions are currently traced or not
        /// </summary>
        bool doTrace = false;
        bool isPaused = false;

        #endregion
        #region Public Variables

        // Access to trace options from Selection page
        #endregion

        #region Accessors

        #endregion


        #region SolidWorks Registration
        [ComRegisterFunctionAttribute]
        public static void RegisterFunction(Type t)
        {
            Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey hkcu = Microsoft.Win32.Registry.CurrentUser;
            string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + t.GUID.ToString() + "}";
            Microsoft.Win32.RegistryKey addinkey = hklm.CreateSubKey(keyname);
            addinkey.SetValue(null, 0);
            addinkey.SetValue("Description", "Record dimensions within SolidWorks");
            addinkey.SetValue("Title", "DimensionRecorder");
            keyname = "Software\\SolidWorks\\AddInsStartup\\{" + t.GUID.ToString() + "}";
            addinkey = hkcu.CreateSubKey(keyname);
            addinkey.SetValue(null, 0);
        }

        [ComUnregisterFunctionAttribute]
        public static void UnregisterFunction(Type t)
        {
            Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey hkcu = Microsoft.Win32.Registry.CurrentUser;
            string keyname = "SOFTWARE\\SolidWorks\\Addins\\{" + t.GUID.ToString() + "}";
            hklm.DeleteSubKey(keyname);
            keyname = "Software\\SolidWorks\\AddInsStartup\\{" + t.GUID.ToString() + "}";
            hkcu.DeleteSubKey(keyname);
        }

        #endregion

        #region ISwAddin Implementation
        /// <summary>
        /// The Constructor
        /// </summary>
        public DimensionRecorder()
        {
            //Variables
            System.Reflection.Assembly assy = System.Reflection.Assembly.GetCallingAssembly();
            this.assyDir = Directory.GetParent(assy.Location).FullName;

        }
        /// <summary>
        /// Connect to SolidWorks
        /// </summary>
        /// <param name="ThisSW"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public bool ConnectToSW(object ThisSW, int cookie)
        {
            iSwApp = (ISldWorks)ThisSW;
            SwEventPtr = (SldWorks)iSwApp;
            addinID = cookie;

            //detect sw language and set it if present
            string lang = this.iSwApp.GetCurrentLanguage();
            if (lang == "german") Lang.currentLanguage = Lang.Languages.german;
            else if (lang == "english") Lang.currentLanguage = Lang.Languages.english;
            else Lang.currentLanguage = Lang.Languages.none;

            //Setup callbacks
            iSwApp.SetAddinCallbackInfo(0, this, addinID);

            //Objects used in initialization of Addin
            openDocs = new Hashtable();
            AddMenus();
            AddToolbar();
            AddPMP();
            AttachEventHandlers();

            //Help
            this.helpPath = Path.Combine(assyDir, Lang.HELP_FILE);

            //Create the additional dialogs (do it here because we need the language information from sw)
            this.aboutDialog = new About(this);
            this.tagDialog = new TagRequestForm(this);


            //Show the about dialog on startup
            this.aboutDialog.ShowDialog();

            return true;
        }
        /// <summary>
        /// Disconnect from SolidWorks
        /// </summary>
        /// <returns></returns>
        public bool DisconnectFromSW()
        {
            RemoveMenus();
            RemoveToolbar();
            RemovePMP();
            DetachEventHandlers();

            iSwApp = null;
            SwEventPtr = null;
            //Retrieve all managed code pointers 
            GC.Collect();
            return true;
        }
        #endregion

        #region UI Methods
        /// <summary>
        /// Add the menu
        /// </summary>
        /// <returns></returns>
        public bool AddMenus()
        {
            int docType = -1;
            int pos;

            pos = 3;

            // No doc
            docType = (int)swDocumentTypes_e.swDocNONE;
            iSwApp.AddMenu(docType, "DimensionRecorder", pos);
            iSwApp.AddMenuItem2(docType, addinID, Lang.ABOUT + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(this.pickAbout), MethodNameProvider.getNameInt(isEnabledAbout), Lang.ABOUT_HINT);

            pos = 6;
            if (iSwApp.ActiveDoc != null)
            {
                IModelDoc2 modDoc = (IModelDoc2)iSwApp.ActiveDoc;
                IModelView modView = (IModelView)modDoc.GetFirstModelView();
                if (modView.FrameState != (int)swWindowState_e.swWindowMaximized)
                    pos = 5;
            }
            //!!! Accelerator keys would be nice with some menu entries but there seems to be a bug in the api (using the & symbol)
            // Parts
            docType = (int)swDocumentTypes_e.swDocPART;
            iSwApp.AddMenu(docType, "DimensionRecorder", pos);
            iSwApp.AddMenuItem4(docType, addinID, Lang.SELECT_DIMENSIONS + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(this.pickSelectTargets), MethodNameProvider.getNameInt(isEnabledSelectTargets), Lang.SELECT_DIMENSIONS_HINT,Path.Combine(this.assyDir,"select.bmp"));
            iSwApp.AddMenuItem4(docType, addinID, Lang.START_RECORDING + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(this.pickStartRecording), MethodNameProvider.getNameInt(isEnabledStart), Lang.START_RECORDING_HINT, Path.Combine(this.assyDir, "start.bmp"));
            iSwApp.AddMenuItem4(docType, addinID, Lang.PAUSE_RECORDING + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(this.pickPauseRecording), MethodNameProvider.getNameInt(isEnabledPause), Lang.PAUSE_RECORDING_HINT, Path.Combine(this.assyDir, "pause.bmp"));
            iSwApp.AddMenuItem4(docType, addinID, Lang.SNAPSHOT + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(this.pickSnapshot), MethodNameProvider.getNameInt(isEnabledSnapshot), Lang.SNAPSHOT_HINT, Path.Combine(this.assyDir, "snapshot.bmp"));
            iSwApp.AddMenuItem4(docType, addinID, Lang.TAG_LAST + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(this.pickTagLast), MethodNameProvider.getNameInt(isEnabledTagLast), Lang.TAG_LAST_HINT, Path.Combine(this.assyDir, "tag.bmp"));
            iSwApp.AddMenuItem4(docType, addinID, Lang.CANCEL_RECORDING + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(this.pickCancelRecording), MethodNameProvider.getNameInt(isEnabledCancel), Lang.CANCEL_RECORDING_HINT, Path.Combine(this.assyDir, "cancel.bmp"));
            iSwApp.AddMenuItem4(docType, addinID, Lang.FINISH_RECORDING + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(this.pickFinishRecording), MethodNameProvider.getNameInt(isEnabledFinish), Lang.FINISH_RECORDING_HINT, Path.Combine(this.assyDir, "save.bmp"));
            iSwApp.AddMenuItem2(docType, addinID, Lang.ABOUT + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(this.pickAbout), MethodNameProvider.getNameInt(isEnabledAbout), Lang.ABOUT_HINT);

            // Assemblies
            docType = (int)swDocumentTypes_e.swDocASSEMBLY;
            iSwApp.AddMenu(docType, "DimensionRecorder", pos);
            iSwApp.AddMenuItem4(docType, addinID, Lang.SELECT_DIMENSIONS + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(pickSelectTargets), MethodNameProvider.getNameInt(isEnabledSelectTargets), Lang.SELECT_DIMENSIONS_HINT, Path.Combine(this.assyDir, "select.bmp"));
            iSwApp.AddMenuItem4(docType, addinID, Lang.START_RECORDING + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(pickStartRecording), MethodNameProvider.getNameInt(isEnabledStart), Lang.START_RECORDING_HINT, Path.Combine(this.assyDir, "start.bmp"));
            iSwApp.AddMenuItem4(docType, addinID, Lang.PAUSE_RECORDING + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(pickPauseRecording), MethodNameProvider.getNameInt(isEnabledPause), Lang.PAUSE_RECORDING_HINT, Path.Combine(this.assyDir, "pause.bmp"));
            iSwApp.AddMenuItem4(docType, addinID, Lang.SNAPSHOT + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(pickSnapshot), MethodNameProvider.getNameInt(isEnabledSnapshot), Lang.SNAPSHOT_HINT, Path.Combine(this.assyDir, "snapshot.bmp"));
            iSwApp.AddMenuItem4(docType, addinID, Lang.TAG_LAST + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(this.pickTagLast), MethodNameProvider.getNameInt(isEnabledTagLast), Lang.TAG_LAST_HINT, Path.Combine(this.assyDir, "tag.bmp"));
            iSwApp.AddMenuItem4(docType, addinID, Lang.CANCEL_RECORDING + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(pickCancelRecording), MethodNameProvider.getNameInt(isEnabledCancel), Lang.CANCEL_RECORDING_HINT, Path.Combine(this.assyDir, "cancel.bmp"));
            iSwApp.AddMenuItem4(docType, addinID, Lang.FINISH_RECORDING + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(pickFinishRecording), MethodNameProvider.getNameInt(isEnabledFinish), Lang.FINISH_RECORDING_HINT, Path.Combine(this.assyDir, "save.bmp"));
            iSwApp.AddMenuItem2(docType, addinID, Lang.ABOUT + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(pickAbout), MethodNameProvider.getNameInt(isEnabledAbout), Lang.ABOUT_HINT);

            // No operations for drawings
            docType = (int)swDocumentTypes_e.swDocDRAWING;
            iSwApp.AddMenu(docType, "DimensionRecorder", pos);
            iSwApp.AddMenuItem2(docType, addinID, Lang.ABOUT + "@DimensionRecorder", -1, MethodNameProvider.getNameVoid(pickAbout), MethodNameProvider.getNameInt(isEnabledAbout), Lang.ABOUT_HINT);

            return true;
        }
        /// <summary>
        /// Remove the menu entries
        /// </summary>
        /// <returns></returns>
        public bool RemoveMenus()
        {
            int docType = -1;
            bool success = false;

            docType = (int)swDocumentTypes_e.swDocNONE;
            success = iSwApp.RemoveMenu(docType, "DimensionRecorder", "");

            docType = (int)swDocumentTypes_e.swDocPART;
            success = iSwApp.RemoveMenu(docType, "DimensionRecorder", "");

            docType = (int)swDocumentTypes_e.swDocASSEMBLY;
            success = iSwApp.RemoveMenu(docType, "DimensionRecorder", "");

            docType = (int)swDocumentTypes_e.swDocDRAWING;
            success = iSwApp.RemoveMenu(docType, "DimensionRecorder", "");

            return true;
        }
        /// <summary>
        /// Add the SW toolbar
        /// </summary>
        /// <returns></returns>
        public bool AddToolbar()
        {
            string smallImagePath = Path.Combine(this.assyDir, @"ToolbarSmall.bmp");
            string largeImagePath = Path.Combine(this.assyDir, @"ToolbarLarge.bmp");
            int docType = (int)swDocTemplateTypes_e.swDocTemplateTypePART |
                         (int)swDocTemplateTypes_e.swDocTemplateTypeASSEMBLY |
                         (int)swDocTemplateTypes_e.swDocTemplateTypeDRAWING;

            toolbarID = iSwApp.AddToolbar4(addinID, "DimensionRecorder", smallImagePath, largeImagePath, 0, docType);
            bool success = false;
            success = iSwApp.AddToolbarCommand2(addinID, toolbarID, 0, MethodNameProvider.getNameVoid(pickSelectTargets), MethodNameProvider.getNameInt(isEnabledSelectTargets), Lang.SELECT_DIMENSIONS, Lang.SELECT_DIMENSIONS_HINT);
            success = iSwApp.AddToolbarCommand2(addinID, toolbarID, 1, MethodNameProvider.getNameVoid(pickStartRecording), MethodNameProvider.getNameInt(isEnabledStart), Lang.START_RECORDING, Lang.START_RECORDING_HINT);
            success = iSwApp.AddToolbarCommand2(addinID, toolbarID, 2, MethodNameProvider.getNameVoid(pickPauseRecording), MethodNameProvider.getNameInt(isEnabledPause), Lang.PAUSE_RECORDING, Lang.PAUSE_RECORDING_HINT);
            success = iSwApp.AddToolbarCommand2(addinID, toolbarID, 3, MethodNameProvider.getNameVoid(pickSnapshot), MethodNameProvider.getNameInt(isEnabledSnapshot), Lang.SNAPSHOT, Lang.SNAPSHOT_HINT);
            success = iSwApp.AddToolbarCommand2(addinID, toolbarID, 4, MethodNameProvider.getNameVoid(pickTagLast), MethodNameProvider.getNameInt(isEnabledTagLast), Lang.TAG_LAST, Lang.TAG_LAST_HINT);
            success = iSwApp.AddToolbarCommand2(addinID, toolbarID, 5, MethodNameProvider.getNameVoid(pickCancelRecording), MethodNameProvider.getNameInt(isEnabledCancel), Lang.CANCEL_RECORDING,Lang.CANCEL_RECORDING_HINT);
            success = iSwApp.AddToolbarCommand2(addinID, toolbarID, 6, MethodNameProvider.getNameVoid(pickFinishRecording), MethodNameProvider.getNameInt(isEnabledFinish), Lang.FINISH_RECORDING,Lang.FINISH_RECORDING_HINT);
            return true;
        }
        /// <summary>
        /// remove the toolbar
        /// </summary>
        /// <returns></returns>
        public bool RemoveToolbar()
        {
            iSwApp.RemoveToolbar2(addinID, toolbarID);
            return true;
        }

        public bool AddPMP()
        {
            ppage = new DimRecPMPage(this);
            return true;
        }

        public bool RemovePMP()
        {
            ppage = null;
            return true;
        }
        #endregion

        #region Event Methods
        public bool AttachEventHandlers()
        {
            AttachSwEvents();
            //Listen for events on all currently open docs
            ModelDoc2 modDoc;
            modDoc = (ModelDoc2)iSwApp.GetFirstDocument();
            while (modDoc != null)
            {
                if (!openDocs.Contains(modDoc))
                {
                    AttachModelDocEventHandler(modDoc);
                }
                modDoc = (ModelDoc2)modDoc.GetNext();
            }
            return true;
        }

        private bool AttachSwEvents()
        {
            try
            {
                SwEventPtr.ActiveDocChangeNotify += new DSldWorksEvents_ActiveDocChangeNotifyEventHandler(OnDocChange);
                SwEventPtr.DocumentLoadNotify += new DSldWorksEvents_DocumentLoadNotifyEventHandler(OnDocLoad);
                SwEventPtr.FileNewNotify2 += new DSldWorksEvents_FileNewNotify2EventHandler(OnFileNew);
                SwEventPtr.ActiveModelDocChangeNotify += new DSldWorksEvents_ActiveModelDocChangeNotifyEventHandler(OnModelChange);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        private bool DetachSwEvents()
        {
            try
            {
                SwEventPtr.ActiveDocChangeNotify -= new DSldWorksEvents_ActiveDocChangeNotifyEventHandler(OnDocChange);
                SwEventPtr.DocumentLoadNotify -= new DSldWorksEvents_DocumentLoadNotifyEventHandler(OnDocLoad);
                SwEventPtr.FileNewNotify2 -= new DSldWorksEvents_FileNewNotify2EventHandler(OnFileNew);
                SwEventPtr.ActiveModelDocChangeNotify -= new DSldWorksEvents_ActiveModelDocChangeNotifyEventHandler(OnModelChange);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public bool AttachModelDocEventHandler(ModelDoc2 modDoc)
        {
            if (modDoc == null)
                return false;

            DocumentEventHandler docHandler = null;

            if (!openDocs.Contains(modDoc))
            {
                switch (modDoc.GetType())
                {
                    case (int)swDocumentTypes_e.swDocPART:
                        {
                            docHandler = new PartEventHandler(modDoc, this);
                            break;
                        }
                    case (int)swDocumentTypes_e.swDocASSEMBLY:
                        {
                            docHandler = new AssemblyEventHandler(modDoc, this);
                            break;
                        }
                    default:
                        {
                            return false; //Unsupported document type
                        }
                }
                docHandler.AttachEventHandlers();
                openDocs.Add(modDoc, docHandler);
            }
            return true;
        }

        public bool DetachModelEventHandler(ModelDoc2 modDoc)
        {
            DocumentEventHandler docHandler;
            docHandler = (DocumentEventHandler)openDocs[modDoc];
            openDocs.Remove(modDoc);
            modDoc = null;
            docHandler = null;
            return true;
        }

        public bool DetachEventHandlers()
        {
            DetachSwEvents();

            //Close events on all currently open docs
            DocumentEventHandler docHandler;
            int numKeys = openDocs.Count;
            object[] keys = new Object[numKeys];

            //Remove all document event handlers
            openDocs.Keys.CopyTo(keys, 0);
            foreach (ModelDoc2 key in keys)
            {
                docHandler = (DocumentEventHandler)openDocs[key];
                docHandler.DetachEventHandlers(); //This also removes the pair from the hash
                docHandler = null;
            }
            return true;
        }
        #endregion

        #region Event Handlers
        //Events
        public int OnDocChange()
        {
            this.modifyAccessibleTraces();
            return 0;
        }

        public int OnDocLoad(string docTitle, string docPath)
        {
            ModelDoc2 modDoc = (ModelDoc2)iSwApp.GetFirstDocument();
            while (modDoc != null)
            {
                if (modDoc.GetTitle() == docTitle)
                {
                    if (!openDocs.Contains(modDoc))
                    {
                        AttachModelDocEventHandler(modDoc);
                    }
                }
                modDoc = (ModelDoc2)modDoc.GetNext();
            }
            return 0;
        }

        public int OnFileNew(object newDoc, int docType, string templateName)
        {
            return 0;
        }

        public int OnModelChange()
        {
            return 0;
        }

        #endregion

        #region Utility Methods
        //Utility Methods
        public Object GetSldWorks()
        {
            return iSwApp;
        }
        #endregion

        #region UI Callbacks

        public void pickSelectTargets()
        {
            this.modifyAccessibleTraces();
            //Clear all selections
            ModelDoc2 doc = (ModelDoc2)(this.iSwApp.ActiveDoc);
            //Clear selection to prevent trace of unwanted entities (e.g. the point selected for dragging a curve)
            //!!!Maybe we can preserve the selected entities in future with a more elaborated approach
            doc.ClearSelection2(true);
            if (ppage != null)
                ppage.Show();
        }
        public void pickStartRecording()
        {
            this.isPaused = false;
            this.doTrace = true;
            this.timeStamps.Clear();
            this.traceDimensions();//Always record the first Position
        }
        public void pickPauseRecording()
        {
            this.isPaused = true;
            this.doTrace = false;
        }
        public void pickSnapshot()
        {
            this.isPaused = false;
            this.doTrace = true;
            this.traceDimensions();
        }
        public void pickCancelRecording()
        {
            this.isPaused = false;
            this.doTrace = false;
            this.clearTargets();
        }
        public void pickFinishRecording()
        {
            this.isPaused = false;
            this.doTrace = false;
            this.writeDimensions();
            this.clearTargets();
        }

        public void pickAbout()
        {
            this.aboutDialog.ShowDialog();
        }

        public void pickTagLast()
        {
            //Query and register a new tag
            this.registerTag();
        }

        public int isEnabledStart()
        {
            // If there are dimensions to trace one can trace them
            if (this.targets.Count <= 0) return 0;// There is nothing to trace
            if (this.doTrace) return 0; // Its already running
            return 1;
        }
        public int isEnabledPause()
        {
            if (!this.doTrace) return 0; // Its not running
            return 1;
        }
        public int isEnabledSnapshot()
        {
            if (!this.doTrace) return 0; // Its not running
            return 1;
        }
        public int isEnabledFinish()
        {
            if (!this.isPaused && !this.doTrace) return 0; //Its nor paused or running
            return 1;
        }
        public int isEnabledCancel()
        {
            if (!this.isPaused && !this.doTrace) return 0; //Its nor paused or running
            return 1;
        }
        public int isEnabledTagLast()
        {
            if (!this.doTrace) return 0; // Its not running
            return 1;
        }
        public int isEnabledSelectTargets()
        {
            if (this.doTrace) return 0;
            if (iSwApp.ActiveDoc != null)
                return 1;
            else
                return 0;
        }

        public int isEnabledAbout()
        {
            return 1;
        }


        //Toolbar Callbacks

        public void ToolbarCallback0()
        {

        }

        public int ToolbarEnable0()
        {
            return 1;
        }

        public void ToolbarCallback1()
        {

        }

        public int ToolbarEnable1()
        {
            return 1;
        }

        public void ToolbarCallback2()
        {

        }

        public int ToolbarEnable2()
        {
            return 1;
        }
        #endregion

        internal bool traceDrag = false;
        internal bool traceSketch = false;
        internal bool traceDialog = false;
        internal bool traceRegen = false;
        internal bool supprDouble = false;

        /// <summary>
        /// Array to keep selected dimensions
        /// </summary>
        List<Target> targets = new List<Target>();
        /// <summary>
        /// A timestamp for each record
        /// </summary>
        List<long> timeStamps = new List<long>();
        /// <summary>
        /// Tags already used
        /// </summary>
        List<string> usedTags = new List<string>();
        /// <summary>
        /// The list of added tag/index combinations
        /// </summary>
        List<Tag> tags = new List<Tag>();


        /// <summary>
        /// Register an object to be observed (only valid objects are registered)
        /// </summary>
        /// <param name="selectedObject"></param>
        public void registerTarget(Object selectedObject)
        {
            foreach (Target target in this.targets) if (target.targetObject.Equals(selectedObject)) return;//Only register once
            this.targets.Add(new Target(this, selectedObject));//This will fail if selected object is not a valid type (as designed)
        }
        /// <summary>
        /// Remove all content
        /// </summary>
        public void clearTargets()
        {
            foreach (Target target in this.targets) target.clear();
            this.targets.Clear();
            this.timeStamps.Clear();
            tags.Clear();
        }
        /// <summary>
        /// Trace the dimension values on change of dimension
        /// </summary>
        public void traceDimensions()
        {
            /// !!! Better unregister Eventhandlers in case of no trace
            if (!this.doTrace) return; // Do nothing if trace is off

            long now = DateTime.Now.Ticks;

            bool hasAnyChanges = false;
            int index = this.timeStamps.Count;

            foreach (Target target in this.targets)
            {
                bool hasChanges = target.register(index);
                hasAnyChanges = hasAnyChanges || hasChanges;
            }

            if (hasAnyChanges || !this.supprDouble)
            {
                //Register a new timestamp
                this.timeStamps.Add(now);//The index of the new timestamp is registered as the index in any changed targed value;
            }
        }
        /// <summary>
        /// Regsiter a new tag for the last entry
        /// </summary>
        internal void registerTag()
        {
            string newTagText = this.tagDialog.getTag(this.usedTags);
            if (newTagText != null && !this.usedTags.Contains(newTagText)) this.usedTags.Add(newTagText);
            //Register with timestamp
            Tag tag = new Tag();
            tag.text = newTagText;
            tag.index = this.timeStamps.Count - 1;
            //If the last index addresses the sam timestamp overwrite it -> only one tag for a timestamp
            if (this.tags.Count > 0 && this.tags[tags.Count - 1].index == tag.index)
            {
                this.tags[tags.Count - 1] = tag;
            }
            else
            {
                this.tags.Add(tag);
            }
        }
        /// <summary>
        /// Write Dimensions to excel sheet
        /// </summary>
        public void writeDimensions()
        {
            Application objApp;
            Workbooks objBooks;
            Workbook objBook;
            Worksheet objWorksheet;
            Range range;

            objApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
            objBooks = objApp.Workbooks;
            objBook = objBooks.Add("");

            objApp.Visible = true;

            objWorksheet = (Worksheet)objBook.ActiveSheet;
            objWorksheet.Name = "Recorded Dimensions";

            int startCol = 1;
            int startRow = 1;
            //The tags
            range = objWorksheet.get_Range(objWorksheet.Cells[startRow, startCol], objWorksheet.Cells[startRow, startCol]);
            range.Value2 = Lang.TAG;
            for (int i = 0; i < this.tags.Count; i++)
            {
                int index = this.tags[i].index;
                range = objWorksheet.get_Range(objWorksheet.Cells[startRow + 2 + index, startCol], objWorksheet.Cells[startRow + 2 + index, startCol]);
                range.Value2 = this.tags[i].text;
            }
            startCol++;
            //The titles
            foreach (Target target in targets)
            {
                string[,] partialTitles;
                double[,] partialValues;

                target.getResultsAsArrays(0, this.timeStamps.Count - 1,out partialTitles, out partialValues);
                int nrTitleRows = partialTitles.GetLength(0);
                int nrCols = partialTitles.GetLength(1);

                //The titles
                range = objWorksheet.get_Range(objWorksheet.Cells[startRow, startCol], objWorksheet.Cells[startRow + nrTitleRows - 1, startCol + nrCols - 1]);
                range.Value2 = partialTitles;

                int nrValueRows = partialValues.GetLength(0);
                //The values
                range = objWorksheet.get_Range(objWorksheet.Cells[startRow+nrTitleRows, startCol], objWorksheet.Cells[startRow + nrTitleRows + nrValueRows - 1, startCol + nrCols - 1]);
                range.Value2 = partialValues;

                startCol = startCol + nrCols;

            }

            objApp.UserControl = true;
        }

        /// <summary>
        /// Deppending on current doctype  modify the available trace modes
        /// </summary>
        public void modifyAccessibleTraces()
        {
            ModelDoc2 modDoc = (ModelDoc2)(this.iSwApp.ActiveDoc);
            switch (modDoc.GetType())
            {
                case (int)swDocumentTypes_e.swDocPART:
                    {
                        this.ppage.traceDialog.Checked = false;
                        ((PropertyManagerPageControl)this.ppage.traceDialog).Enabled = true;
                        this.ppage.traceDrag.Checked = false;
                        ((PropertyManagerPageControl)this.ppage.traceDrag).Enabled = false;
                        this.ppage.traceSketch.Checked = false;
                        ((PropertyManagerPageControl)this.ppage.traceSketch).Enabled = true;
                        this.ppage.traceRegen.Checked = false;
                        ((PropertyManagerPageControl)this.ppage.traceRegen).Enabled = true;
                        break;
                    }
                case (int)swDocumentTypes_e.swDocASSEMBLY:
                    {
                        this.ppage.traceDialog.Checked = false;
                        ((PropertyManagerPageControl)this.ppage.traceDialog).Enabled = true;
                        this.ppage.traceDrag.Checked = false;
                        ((PropertyManagerPageControl)this.ppage.traceDrag).Enabled = true;
                        this.ppage.traceSketch.Checked = false;
                        ((PropertyManagerPageControl)this.ppage.traceSketch).Enabled = true;
                        this.ppage.traceRegen.Checked = false;
                        ((PropertyManagerPageControl)this.ppage.traceRegen).Enabled = true;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        public void storeTraceSettings()
        {
            if (this.ppage == null) return;
            this.traceDrag = this.ppage.traceDrag.Checked;
            this.traceDialog = this.ppage.traceDialog.Checked;
            this.traceSketch = this.ppage.traceSketch.Checked;
            this.traceRegen = this.ppage.traceRegen.Checked;

            // Issue a warning if no trace event was selected
            if (!(this.traceDrag || this.traceDialog || this.traceSketch || this.traceRegen))
            {
                System.Windows.Forms.MessageBox.Show(Lang.NO_EVENT_MESS, Lang.INFORMATION,
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }


            this.supprDouble = this.ppage.supprDouble.Checked;
        }




    }

}
