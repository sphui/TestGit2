using System;
using System.Text;

namespace DimensionRecorder
{
    /// <summary>
    /// Provides language specific strings
    /// </summary>
    internal class Lang
    {
        internal static Languages currentLanguage = Languages.none;
        internal enum Languages { none, english, german };
        private static string EXCEPTION_TEXT = "Invalid current language";

        internal static string SELECT_DIMENSIONS
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Select Dimensions";
                    case Languages.german: return "Dimensionen wählen";
                }
                throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string SELECT_DIMENSIONS_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Select dimensions to be recorded and set recording options.";
                    case Languages.german: return "Dimensionen zum Aufzeichnen wählen und Optionen setzen.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string START_RECORDING
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Start Recording";
                    case Languages.german: return "Aufnahme starten";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string START_RECORDING_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Start the recording of selected dimensions.";
                    case Languages.german: return "Aufnahme der ausgewählten Dimensionen starten.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string PAUSE_RECORDING
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Pause Recording";
                    case Languages.german: return "Aufnahme pausieren";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string PAUSE_RECORDING_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Pause Recording. Reactivate with the 'Start Recording' button.";
                    case Languages.german: return "Aufnahme unterbrechen. Zum Fortsetzen 'Aufnahme starten' drücken.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string SNAPSHOT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Snapshot";

                    case Languages.german: return "Momentaufnahme";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string SNAPSHOT_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Record dimensions now.";
                    case Languages.german: return "Dimensionen jetzt speichern";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string TAG_LAST
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Tag Last";
                    case Languages.german: return "letzten markieren";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string TAG_LAST_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Add a Tag to the last recorded set of dimensions/values.";
                    case Languages.german: return "Markierung zum letzten Datensatz hinzufügen";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string CANCEL_RECORDING
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Cancel Recording";
                    case Languages.german: return "Aufnahme abbrechen";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string CANCEL_RECORDING_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Cancel Recording. Recorded dimensions will be lost.";
                    case Languages.german: return "Aufnahme abbrechen. Aufgezeichnete Dimensionen gehen verloren";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string FINISH_RECORDING
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Finish Recording";
                    case Languages.german: return "Aufnahme abschließen";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string FINISH_RECORDING_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Finish recording. Dimensions will be written to spreadsheet";
                    case Languages.german: return "Aufnahme abschließen. Aufgezeichnete Dimensionen werden an die Tabellenkalkulation übertragen.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string ABOUT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "About";
                    case Languages.german: return "Info";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string ABOUT_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "About the software.";
                    case Languages.german: return "Informationen zur Software.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }


        //PMPage
        internal static string DIMENSION_SELECTION
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Dimension Selection";
                    case Languages.german: return "Dimensionsauswahl";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string DIMENSION_SELECTION_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Register the dimensions to be recorded";
                    case Languages.german: return "Aufzuzeichnende Dimensionen auswählen";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string DIMENSIONS
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Dimensions";
                    case Languages.german: return "Dimensionen";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string OBJECTS
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Objects";
                    case Languages.german: return "Objekte";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string POINTS
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Points";
                    case Languages.german: return "Punkte";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string EVENTS
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Events";
                    case Languages.german: return "Ereignisse";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string OPTIONS
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Options";
                    case Languages.german: return "Optionen";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string RECORD_DIMENSIONS
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Record Dimensions";
                    case Languages.german: return "Dimensionen Aufzeichnen";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string RECORD_DIMENSIONS_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Dimensions to be recorded.";
                    case Languages.german: return "Dimensionen, die aufgezeichnet werden.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string RECORD_POSITIONS
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Record Positions";
                    case Languages.german: return "Positionen aufzeichnen";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string RECORD_POSITIONS_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Objects to record positions of.";
                    case Languages.german: return "Objekte, deren Positionen aufgezeichnet werden.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string DRAG
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Drag";
                    case Languages.german: return "Ziehen";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string DRAG_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Record dimensions on component drag events";
                    case Languages.german: return "Dimensionen beim Ziehen von Komponenten aufzeichnen.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string CHANGE_DIMENSION
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Change Dimension";
                    case Languages.german: return "Dimension ändern";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string CHANGE_DIMENSION_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Record dimensions if any dimension value is changed by an user dialog.";
                    case Languages.german: return "Dimensionen beim Ändern einen beliebigen Dimension über einen Anwenderdialog.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string SKETCH_SOLVE
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Sketch Solve";
                    case Languages.german: return "Neuaufbau Skizze";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string SKETCH_SOLVE_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Record dimensions if a sketch was changed and rebuild.";
                    case Languages.german: return "Dimensionen beim Ändern einer Skizze aufzeichnen.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string MODEL_REGENERATION
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Model Regeneration";
                    case Languages.german: return "Modellneuaufbau";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string MODEL_REGENERATION_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Record dimensions after the model is regenerated.";
                    case Languages.german: return "Dimensionen nach dem Modellneuaufbau aufzeichnen.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string DROP_DOUBLE
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Drop Double";
                    case Languages.german: return "Doppel unterdrücken";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        internal static string DROP_DOUBLE_HINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Do not record dimension data sets twice in a row.";
                    case Languages.german: return "Jeden Dimensionsdatensatz nur einmal in Folge aufzeichnen.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        // Messages
        internal static string NO_EVENT_MESS
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "No event selected. Use 'snapshot' to record values.";
                    case Languages.german: return "Keine Ereignisse ausgewählt. Verwenden sie 'Momentaufnahme' um Werte aufzuzeichnen.";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        //Other
        internal static string POINT
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Point";
                    case Languages.german: return "Punkt";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string OK
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Ok";
                    case Languages.german: return "Ok";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string CANCEL
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Cancel";
                    case Languages.german: return "Abbrechen";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string ENTER_TAG
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Enter Tag";
                    case Languages.german: return "Markierung eingeben";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string TAG
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Tag";
                    case Languages.german: return "Markierung";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string INFORMATION
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Information";
                    case Languages.german: return "Information";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string LICENSE
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Licence";
                    case Languages.german: return "Lizenz";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        #region File Names
        internal static string HELP_FILE
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: 
                    case Languages.german: return "DimensionRecorder_EN.pdf";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }

        internal static string LICENSE_FILE
        {
            get
            {
                switch (currentLanguage)
                {
                    case Languages.none:
                    case Languages.english: return "Licence_EUPL_EN.txt";
                    case Languages.german: return "LiZenz_EUPL_DE.txt";
                } throw new Exception(Lang.EXCEPTION_TEXT);
            }
        }
        #endregion




    }
}
