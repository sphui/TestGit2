// (C) Jan Boettcher, <http://www.ib-boettcher.de>
// Licensed under the EUPL V.1.1

using System;
using System.Collections;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using SolidWorks.Interop.swconst;
using SolidWorksTools;

namespace DimensionRecorder
{
	/// <summary>
	/// Summary description for EventHandling.
	/// </summary>
	public class DocumentEventHandler
	{
		protected ISldWorks iSwApp;
		protected ModelDoc2 document;
		protected DimensionRecorder userAddin;
		protected Hashtable openModelViews;

		public DocumentEventHandler(ModelDoc2 modDoc, DimensionRecorder addin)
		{
			document = modDoc;
			userAddin = addin;
			iSwApp = (ISldWorks)userAddin.GetSldWorks();
			openModelViews = new Hashtable();
		}

		virtual public bool AttachEventHandlers()
		{
			return true;
		}

		virtual public bool DetachEventHandlers()
		{
			return true;
		}
	}

	public class PartEventHandler : DocumentEventHandler
	{
		PartDoc doc;

		public PartEventHandler(ModelDoc2 modDoc, DimensionRecorder addin):base(modDoc,addin)
		{
			doc = (PartDoc)document;
		}

		override public bool AttachEventHandlers()
		{
			doc.DestroyNotify += new DPartDocEvents_DestroyNotifyEventHandler(OnDestroy);
			doc.NewSelectionNotify += new DPartDocEvents_NewSelectionNotifyEventHandler(OnNewSelection);
			doc.DimensionChangeNotify += new DPartDocEvents_DimensionChangeNotifyEventHandler(onDimensionChange);
			doc.SketchSolveNotify += new DPartDocEvents_SketchSolveNotifyEventHandler(onSketchSolve);
            doc.RegenPostNotify += new DPartDocEvents_RegenPostNotifyEventHandler(onRegen);

			return true;
		}

		override public bool DetachEventHandlers()
		{
			doc.DestroyNotify -= new DPartDocEvents_DestroyNotifyEventHandler(OnDestroy);
			doc.NewSelectionNotify -= new DPartDocEvents_NewSelectionNotifyEventHandler(OnNewSelection);
			doc.DimensionChangeNotify -= new DPartDocEvents_DimensionChangeNotifyEventHandler(onDimensionChange);
			doc.SketchSolveNotify -= new DPartDocEvents_SketchSolveNotifyEventHandler(onSketchSolve);
            doc.RegenPostNotify -= new DPartDocEvents_RegenPostNotifyEventHandler(onRegen);

			userAddin.DetachModelEventHandler(document);
			return true;
		}

		//Event Handlers
		public int OnDestroy()
		{
			DetachEventHandlers();
			return 0;
		}

		public int OnNewSelection()
		{
			return 0;
		}

		/// <summary>
		/// Called on change of a dimension
		/// </summary>
		/// <param name="dim"></param>
		/// <returns></returns>
		public int onDimensionChange(object dim) 
		{
			if (userAddin.traceDialog == false) return 0;
			userAddin.traceDimensions();
			return 0;
		}
		/// <summary>
		/// Called on Sketch solve
		/// </summary>
		/// <param name="dim"></param>
		/// <returns></returns>
		public int onSketchSolve(string txt) 
		{
			if (userAddin.traceSketch == false) return 0;
			userAddin.traceDimensions();
			return 0;
		}
        /// <summary>
        /// Called on the regeneration of the document
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public int onRegen()
        {
            if (userAddin.traceSketch == false) return 0;
            userAddin.traceDimensions();
            return 0;
        }

	}

	public class AssemblyEventHandler : DocumentEventHandler
	{
		AssemblyDoc doc;

		public AssemblyEventHandler(ModelDoc2 modDoc, DimensionRecorder addin):base(modDoc,addin)
		{
			doc = (AssemblyDoc)document;
		}

		override public bool AttachEventHandlers()
		{
			doc.DestroyNotify += new DAssemblyDocEvents_DestroyNotifyEventHandler(OnDestroy);
			doc.NewSelectionNotify += new DAssemblyDocEvents_NewSelectionNotifyEventHandler(OnNewSelection);
			doc.DimensionChangeNotify += new DAssemblyDocEvents_DimensionChangeNotifyEventHandler(onDimensionChange);
            doc.ComponentMoveNotify2 += new DAssemblyDocEvents_ComponentMoveNotify2EventHandler(onComponentMove);
            doc.SketchSolveNotify += new DAssemblyDocEvents_SketchSolveNotifyEventHandler(onSketchSolve);
            doc.RegenPostNotify += new DAssemblyDocEvents_RegenPostNotifyEventHandler(onRegen);

			return true;
		}

		override public bool DetachEventHandlers()
		{
			doc.DestroyNotify -= new DAssemblyDocEvents_DestroyNotifyEventHandler(OnDestroy);
			doc.NewSelectionNotify -= new DAssemblyDocEvents_NewSelectionNotifyEventHandler(OnNewSelection);
			doc.DimensionChangeNotify -= new DAssemblyDocEvents_DimensionChangeNotifyEventHandler(onDimensionChange);
			doc.ComponentMoveNotify2 -= new DAssemblyDocEvents_ComponentMoveNotify2EventHandler(onComponentMove);
            doc.SketchSolveNotify -= new DAssemblyDocEvents_SketchSolveNotifyEventHandler(onSketchSolve);
            doc.RegenPostNotify -= new DAssemblyDocEvents_RegenPostNotifyEventHandler(onRegen);
			userAddin.DetachModelEventHandler(document);
			return true;
		}

		//Event Handlers
		public int OnDestroy()
		{
			DetachEventHandlers();
			return 0;
		}

		public int OnNewSelection()
		{
			return 0;
		}

		/// <summary>
		/// Called on change of a dimension
		/// </summary>
		/// <param name="dim"></param>
		/// <returns></returns>
		public int onDimensionChange(object dim) 
		{
			if (userAddin.traceDialog == false) return 0;
			userAddin.traceDimensions();
			return 0;
		}
		/// <summary>
		/// Called on move of a component
		/// </summary>
		/// <param name="dim"></param>
		/// <returns></returns>
		public int onComponentMove(ref Object components) 
		{
			if (userAddin.traceDrag == false) return 0;
			userAddin.traceDimensions();
			return 0;
		}
		/// <summary>
		/// Called on the solve of a sketch
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public int onSketchSolve(string txt) 
		{
			if (userAddin.traceSketch == false) return 0;
			userAddin.traceDimensions();
			return 0;
		}
        /// <summary>
        /// Called on the regeneration of the document
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public int onRegen()
        {
            if (userAddin.traceRegen == false) return 0;
            userAddin.traceDimensions();
            return 0;
        }

	}
}
