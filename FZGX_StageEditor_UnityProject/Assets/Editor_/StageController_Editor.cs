using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using FzgxData;
using GameCube.Games.FzeroGX;

[CustomEditor(typeof(Stage))]
public class Stage_Editor : Editor {

    private int buttonWidth = 40;
    private float whittenValue = 0.5f;
    Stage editorTarget;
    public static int nextStage = 0;

    #region Menu
    [MenuItem("F-Zero GX Tools/Load Next Stage &n")]
    public static void LoadNextStage()
    {
        List<FzgxStage> stages = FZGX.AllStages;

        for (int i = 0; i < stages.Count; i++)
        {
            if (Stage.currentStage == stages[i])
            {
                Stage.currentStage = stages[MathX.Wrap(i + 1, stages.Count)];
                break;
            }
        }
    
        // Force Update, set dirty
        Stage.Current.Update();
        EditorUtility.SetDirty(Stage.Current);
    }

    [MenuItem("F-Zero GX Tools/Load Previous Stage &b")]
    public static void LoadPreviousStage()
    {
        // Load all stages into list
        List<FzgxStage> stages = FZGX.AllStages;

        // Loop through all stages in list
        for (int i = 0; i < stages.Count; i++)
        {
            // Once you find which stage we're at
            if (Stage.currentStage == stages[i])
            {
                // Decrement stage
                Stage.currentStage = stages[MathX.Wrap(i - 1, stages.Count)];
                break;
            }
        }

        // Force Update, set dirty
        Stage.Current.Update();
        EditorUtility.SetDirty(Stage.Current);
    }

    [MenuItem("F-Zero GX Tools/Reload Stage &r")]
    public static void ReloadStage()
    {
        Stage.lastStage = (FzgxStage) (-1);
        Stage.Current.Update();
        EditorUtility.SetDirty(Stage.Current);
    }

    [MenuItem("F-Zero GX Tools/Cycle through all stages")]
    public static void LoadAll()
    {
        for (int i = 0; i <= 110; i++)
        {
            FzgxStage stage = ((FzgxStage)i);
            
            // Greater than int value
            if (stage.ToString().Length > 3)
                Stage.ChangeStage(stage);
        }
    }
    #endregion

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        editorTarget = target as Stage;

        Stage.resourcePath = EditorGUILayout.TextField(Stage.resourcePath);

        // Label that displays current stage
        GUI.enabled = false;
        EditorGUILayout.LabelField(string.Format("Current Stage: {0} : {1}", ((int)Stage.currentStage).ToString("00"), Stage.currentStage.ToString().Replace('_', ' ')));
        GUI.enabled = true;

        // RUBY CUP
        GUI.color = Palette.rose_red.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("MCTR", FzgxStage.MUTE_CITY_Twist_Road);
        StageButton("CPSO", FzgxStage.CASINO_PALACE_Split_Oval);
        StageButton("SOSS", FzgxStage.SAND_OCEAN_Surface_Slide);
        StageButton("LLC",  FzgxStage.LIGHTNING_Loop_Cross);
        StageButton("AM",   FzgxStage.AEROPOLIS_Multiplex);
        EditorGUILayout.EndHorizontal();

        // SAPPHIRE CUP
        GUI.color = Palette.cobalt.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("BBDH", FzgxStage.BIG_BLUE_Drift_Highway);
        StageButton("PTAD", FzgxStage.PORT_TOWN_Aero_Dive);
        StageButton("GPMR", FzgxStage.GREEN_PLANT_Mobius_Ring);
        StageButton("PTLP", FzgxStage.PORT_TOWN_Long_Pipe);
        StageButton("MCSG", FzgxStage.MUTE_CITY_Serial_Gaps);
        EditorGUILayout.EndHorizontal();

        // EMERALD CUP
        GUI.color = Palette.lime_green.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("FFCK", FzgxStage.FIRE_FIELD_Cylinder_Knot);
        StageButton("GPI",  FzgxStage.GREEN_PLANT_Intersection);
        StageButton("CPDB", FzgxStage.CASINO_PALACE_Double_Branches);
        StageButton("LHP",  FzgxStage.LIGHTNING_Half_Pipe);
        StageButton("BBO",  FzgxStage.BIG_BLUE_Ordeal);
        EditorGUILayout.EndHorizontal();

        // DIAMOND CUP
        GUI.color = Palette.yellow.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("CTT",   FzgxStage.COSMO_TERMINAL_Trident);
        StageButton("SOLS",  FzgxStage.SAND_OCEAN_Lateral_Shift);
        StageButton("FFU",   FzgxStage.FIRE_FIELD_Undulation);
        StageButton("ADS",   FzgxStage.AEROPOLIS_Dragon_Slope);
        StageButton("PRSLS", FzgxStage.PHANTOM_ROAD_Slim_Line_Slits);
        EditorGUILayout.EndHorizontal();

        // AX CUP
        GUI.color = Palette.magenta_violet.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("ASD",  FzgxStage.AEROPOLIS_Screw_Drive);
        StageButton("OSMS", FzgxStage.OUTER_SPACE_Meteor_Stream);
        StageButton("PTCW", FzgxStage.PORT_TOWN_Cylinder_Wave);
        StageButton("LTR",  FzgxStage.LIGHTNING_Thunder_Road);
        StageButton("GPS",  FzgxStage.GREEN_PLANT_Spiral);
        StageButton("MCSO", FzgxStage.MUTE_CITY_Sonic_Oval);
        EditorGUILayout.EndHorizontal();

        // STORY
        GUI.color = Palette.grey.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("Story 1", FzgxStage.STORY_1);
        StageButton("Story 2", FzgxStage.STORY_2);
        StageButton("Story 3", FzgxStage.STORY_3);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        StageButton("Story 4", FzgxStage.STORY_4);
        StageButton("Story 5", FzgxStage.STORY_5);
        StageButton("Story 6", FzgxStage.STORY_6);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        StageButton("Story 7", FzgxStage.STORY_7);
        StageButton("Story 8", FzgxStage.STORY_8);
        StageButton("Story 9", FzgxStage.STORY_9);
        EditorGUILayout.EndHorizontal();

        // EXTRA
        GUI.color = Palette.grey.Whitten(whittenValue);
        EditorGUILayout.BeginHorizontal();
        StageButton("Grand Prix Podium", FzgxStage.EX_Grand_Prix_Podium);
        StageButton("Victory Lap",       FzgxStage.EX_Victory_Lap);
        EditorGUILayout.EndHorizontal();
    }

    private void StageButton(string buttonLabel, FzgxStage stage)
    {
        if (GUILayout.Button(buttonLabel, GUILayout.MinWidth(buttonWidth)))
        {
            Stage.currentStage = stage;
            EditorUtility.SetDirty(editorTarget);
        }
    }
}
