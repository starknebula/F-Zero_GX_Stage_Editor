using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using GX_Data;
using GX_Data.SplineData;

namespace FzgxData
{
    public class DisplaySpline : MonoBehaviour, IFZGXEditorStageEventReceiver
    {
        private static SplineDataClass splineData;
        public static SplineDataClass SplineData { get { return splineData; } }

        public void StageUnloaded(BinaryReader reader)
        {
            //throw new NotImplementedException();
        }
        public void StageLoaded(BinaryReader reader)
        {
            splineData = new SplineDataClass(reader);
        }


        void OnDrawGizmos()
        {
            if (splineData == null) return;
            ///////////////////////////////

            for (int i = 0; i < splineData.Base.Length; i++)
            {
                int index = splineData.Base[i].EditorDataID;
                int count = splineData.EditorData.Count;

                Gizmos.color = Handles.color = Palette.ColorWheel(index + (index % 2 == 0 ? 0 : count/2), count);
                Gizmos.color = (splineData.Base[i].EditorDataID % 2 == 0) ? Gizmos.color : Gizmos.color.Whitten(.75f);
                Handles.DrawDottedLine(splineData.Base[i].RuntimeData.startPosition, splineData.Base[i].RuntimeData.endPosition, 2f);

                Handles.DrawWireDisc(splineData.Base[i].RuntimeData.startPosition, splineData.Base[i].RuntimeData.startTangent, 2.5f);
                Handles.DrawWireDisc(splineData.Base[i].RuntimeData.endPosition, splineData.Base[i].RuntimeData.endTangent, 5.0f);
            }
        }

    }

    [CustomEditor(typeof(DisplaySpline))]
    public class DisplaySpline_Editor : Editor, IFZGXEditorStageEventReceiver
    {
        public List<SceneGUIToolbar> toolbars;

        public void StageLoaded(BinaryReader reader)
        {
            OnEnable();
        }
        public void StageUnloaded(BinaryReader reader)
        {
        }

        private void OnEnable()
        {
            if (DisplaySpline.SplineData == null) return;

            toolbars = new List<SceneGUIToolbar>();
   
            foreach (GX_Data.SplineData.Base data in DisplaySpline.SplineData.Base)
            {
                List<string> contents = new List<string>();
                contents.Add(string.Format("{0} {1}", "Merge from last", data.RuntimeData.mergeFromLast));

                ToolbarContent tab = new ToolbarContent(data.RuntimeDataAddress.ToString(), contents.ToArray());

                toolbars.Add(new SceneGUIToolbar(Camera.main, data.RuntimeData.startPosition, 240, new ToolbarContent[] { tab }));
            }
        }

        // Has to be selected
        private void OnSceneGUI()
        {
            if (DisplaySpline.SplineData == null) return;

            if (toolbars.Count != DisplaySpline.SplineData.EditorData.Count)
                OnEnable();

            GUI.skin = Resources.Load<GUISkin>("DebugGUI");

            Handles.BeginGUI();
            foreach (SceneGUIToolbar toolbar in toolbars)
                toolbar.Render();
            Handles.EndGUI();

        }
    }
}