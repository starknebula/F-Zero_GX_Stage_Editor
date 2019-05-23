using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using GameCube.Games.FZeroGX;

[CustomEditor(typeof(StageManager))]
public class Stage_Editor : Editor {

    private int buttonWidth = 40;
    private float whittenValue = 0.5f;
    StageManager editorTarget;
    public static int nextStage = 0;

    #region Menu
    [MenuItem("F-Zero GX Tools/Load Next Stage &n")]
    public static void LoadNextStage()
    {
        List<FZeroGXStage> stages = FZGX.AllStages;

        for (int i = 0; i < stages.Count; i++)
        {
            if (StageManager.currentStage == stages[i])
            {
                StageManager.currentStage = stages[MathX.Wrap(i + 1, stages.Count)];
                break;
            }
        }
    
        // Force Update, set dirty
        StageManager.Current.Update();
        EditorUtility.SetDirty(StageManager.Current);
    }

    [MenuItem("F-Zero GX Tools/Load Previous Stage &b")]
    public static void LoadPreviousStage()
    {
        // Load all stages into list
        List<FZeroGXStage> stages = FZGX.AllStages;

        // Loop through all stages in list
        for (int i = 0; i < stages.Count; i++)
        {
            // Once you find which stage we're at
            if (StageManager.currentStage == stages[i])
            {
                // Decrement stage
                StageManager.currentStage = stages[MathX.Wrap(i - 1, stages.Count)];
                break;
            }
        }

        // Force Update, set dirty
        StageManager.Current.Update();
        EditorUtility.SetDirty(StageManager.Current);
    }

    [MenuItem("F-Zero GX Tools/Reload Stage &r")]
    public static void ReloadStage()
    {
        StageManager.lastStage = (FZeroGXStage) (-1);
        StageManager.Current.Update();
        EditorUtility.SetDirty(StageManager.Current);
    }

    [MenuItem("F-Zero GX Tools/Cycle through all stages")]
    public static void LoadAll()
    {
        for (int i = 0; i <= 110; i++)
        {
            FZeroGXStage stage = ((FZeroGXStage)i);
            
            // Greater than int value
            if (stage.ToString().Length > 3)
                StageManager.ChangeStage(stage);
        }
    }
    #endregion

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        editorTarget = target as StageManager;

        // TODO: add browse button
        StageManager.resourcePath = EditorGUILayout.TextField(StageManager.resourcePath);

        // Label that displays current stage
        GUI.enabled = false;
        EditorGUILayout.LabelField(string.Format("Current Stage: {0} : {1}", ((int)StageManager.currentStage).ToString("00"), StageManager.currentStage.ToString().Replace('_', ' ')));
        GUI.enabled = true;

        // RUBY CUP
        GUI.color = Palette.rose_red.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("MCTR", FZeroGXStage.MUTE_CITY_Twist_Road);
        StageButton("CPSO", FZeroGXStage.CASINO_PALACE_Split_Oval);
        StageButton("SOSS", FZeroGXStage.SAND_OCEAN_Surface_Slide);
        StageButton("LLC",  FZeroGXStage.LIGHTNING_Loop_Cross);
        StageButton("AM",   FZeroGXStage.AEROPOLIS_Multiplex);
        EditorGUILayout.EndHorizontal();

        // SAPPHIRE CUP
        GUI.color = Palette.cobalt.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("BBDH", FZeroGXStage.BIG_BLUE_Drift_Highway);
        StageButton("PTAD", FZeroGXStage.PORT_TOWN_Aero_Dive);
        StageButton("GPMR", FZeroGXStage.GREEN_PLANT_Mobius_Ring);
        StageButton("PTLP", FZeroGXStage.PORT_TOWN_Long_Pipe);
        StageButton("MCSG", FZeroGXStage.MUTE_CITY_Serial_Gaps);
        EditorGUILayout.EndHorizontal();

        // EMERALD CUP
        GUI.color = Palette.lime_green.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("FFCK", FZeroGXStage.FIRE_FIELD_Cylinder_Knot);
        StageButton("GPI",  FZeroGXStage.GREEN_PLANT_Intersection);
        StageButton("CPDB", FZeroGXStage.CASINO_PALACE_Double_Branches);
        StageButton("LHP",  FZeroGXStage.LIGHTNING_Half_Pipe);
        StageButton("BBO",  FZeroGXStage.BIG_BLUE_Ordeal);
        EditorGUILayout.EndHorizontal();

        // DIAMOND CUP
        GUI.color = Palette.yellow.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("CTT",   FZeroGXStage.COSMO_TERMINAL_Trident);
        StageButton("SOLS",  FZeroGXStage.SAND_OCEAN_Lateral_Shift);
        StageButton("FFU",   FZeroGXStage.FIRE_FIELD_Undulation);
        StageButton("ADS",   FZeroGXStage.AEROPOLIS_Dragon_Slope);
        StageButton("PRSLS", FZeroGXStage.PHANTOM_ROAD_Slim_Line_Slits);
        EditorGUILayout.EndHorizontal();

        // AX CUP
        GUI.color = Palette.magenta_violet.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("ASD",  FZeroGXStage.AEROPOLIS_Screw_Drive);
        StageButton("OSMS", FZeroGXStage.OUTER_SPACE_Meteor_Stream);
        StageButton("PTCW", FZeroGXStage.PORT_TOWN_Cylinder_Wave);
        StageButton("LTR",  FZeroGXStage.LIGHTNING_Thunder_Road);
        StageButton("GPS",  FZeroGXStage.GREEN_PLANT_Spiral);
        StageButton("MCSO", FZeroGXStage.MUTE_CITY_Sonic_Oval);
        EditorGUILayout.EndHorizontal();

        // STORY
        GUI.color = Palette.grey.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("Story 1", FZeroGXStage.STORY_1);
        StageButton("Story 2", FZeroGXStage.STORY_2);
        StageButton("Story 3", FZeroGXStage.STORY_3);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        StageButton("Story 4", FZeroGXStage.STORY_4);
        StageButton("Story 5", FZeroGXStage.STORY_5);
        StageButton("Story 6", FZeroGXStage.STORY_6);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        StageButton("Story 7", FZeroGXStage.STORY_7);
        StageButton("Story 8", FZeroGXStage.STORY_8);
        StageButton("Story 9", FZeroGXStage.STORY_9);
        EditorGUILayout.EndHorizontal();

        // EXTRA
        GUI.color = Palette.grey.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("Grand Prix Podium", FZeroGXStage.EX_Grand_Prix_Podium);
        StageButton("Victory Lap",       FZeroGXStage.EX_Victory_Lap);
        EditorGUILayout.EndHorizontal();
    }

    private void StageButton(string buttonLabel, FZeroGXStage stage)
    {
        if (GUILayout.Button(buttonLabel, GUILayout.MinWidth(buttonWidth)))
        {
            StageManager.currentStage = stage;
            EditorUtility.SetDirty(editorTarget);
        }
    }
}
