using System.Collections.Generic;

namespace GameCube.Games.FZeroGX
{
    public enum FZeroGXStage
    {
        AEROPOLIS_Multiplex = 5,
        AEROPOLIS_Dragon_Slope = 21,
        AEROPOLIS_Screw_Drive = 31,

        BIG_BLUE_Drift_Highway = 14,
        BIG_BLUE_Ordeal = 27,

        CASINO_PALACE_Split_Oval = 16,
        CASINO_PALACE_Double_Branches = 29,

        COSMO_TERMINAL_Trident = 24,

        FIRE_FIELD_Cylinder_Knot = 15,
        FIRE_FIELD_Undulation = 17,

        GREEN_PLANT_Intersection = 10,
        GREEN_PLANT_Mobius_Ring = 11,
        GREEN_PLANT_Spiral = 35,

        LIGHTNING_Loop_Cross = 8,
        LIGHTNING_Half_Pipe = 9,
        LIGHTNING_Thunder_Road = 34,

        MUTE_CITY_Twist_Road = 1,
        MUTE_CITY_Serial_Gaps = 3,
        MUTE_CITY_Sonic_Oval = 36,

        OUTER_SPACE_Meteor_Stream = 32,

        PHANTOM_ROAD_Slim_Line_Slits = 28,

        PORT_TOWN_Aero_Dive = 7,
        PORT_TOWN_Long_Pipe = 13,
        PORT_TOWN_Cylinder_Wave = 33,

        //SAND_OCEAN_Screw_Drive = 0,
        SAND_OCEAN_Lateral_Shift = 25,
        SAND_OCEAN_Surface_Slide = 26,

        STORY_1 = 37,
        STORY_2 = 38,
        STORY_3 = 39,
        STORY_4 = 40,
        STORY_5 = 41,
        STORY_6 = 42,
        STORY_7 = 43,
        STORY_8 = 44,
        STORY_9 = 45,

        EX_Grand_Prix_Podium = 49,
        EX_Victory_Lap = 50,
    }

    public static class FZGX
    {
        public static readonly List<FZeroGXStage> AllStages = new List<FZeroGXStage>()
        {
            // RUBY CUP
            FZeroGXStage.MUTE_CITY_Twist_Road,
            FZeroGXStage.CASINO_PALACE_Split_Oval,
            FZeroGXStage.SAND_OCEAN_Surface_Slide,
            FZeroGXStage.LIGHTNING_Loop_Cross,
            FZeroGXStage.AEROPOLIS_Multiplex,

            // SAPPHIRE CUP
            FZeroGXStage.BIG_BLUE_Drift_Highway,
            FZeroGXStage.PORT_TOWN_Aero_Dive,
            FZeroGXStage.GREEN_PLANT_Mobius_Ring,
            FZeroGXStage.PORT_TOWN_Long_Pipe,
            FZeroGXStage.MUTE_CITY_Serial_Gaps,

            // EMERALD CUP
            FZeroGXStage.FIRE_FIELD_Cylinder_Knot,
            FZeroGXStage.GREEN_PLANT_Intersection,
            FZeroGXStage.CASINO_PALACE_Double_Branches,
            FZeroGXStage.LIGHTNING_Half_Pipe,
            FZeroGXStage.BIG_BLUE_Ordeal,

            // DIAMOND CUP
            FZeroGXStage.COSMO_TERMINAL_Trident,
            FZeroGXStage.SAND_OCEAN_Lateral_Shift,
            FZeroGXStage.FIRE_FIELD_Undulation,
            FZeroGXStage.AEROPOLIS_Dragon_Slope,
            FZeroGXStage.PHANTOM_ROAD_Slim_Line_Slits,

            // AX CUP
            FZeroGXStage.AEROPOLIS_Screw_Drive,
            FZeroGXStage.OUTER_SPACE_Meteor_Stream,
            FZeroGXStage.PORT_TOWN_Cylinder_Wave,
            FZeroGXStage.LIGHTNING_Thunder_Road,
            FZeroGXStage.GREEN_PLANT_Spiral,
            FZeroGXStage.MUTE_CITY_Sonic_Oval,

            // STORY
            FZeroGXStage.STORY_1,
            FZeroGXStage.STORY_2,
            FZeroGXStage.STORY_3,
            FZeroGXStage.STORY_4,
            FZeroGXStage.STORY_5,
            FZeroGXStage.STORY_6,
            FZeroGXStage.STORY_7,
            FZeroGXStage.STORY_8,
            FZeroGXStage.STORY_9,

            // EXTRA
            FZeroGXStage.EX_Grand_Prix_Podium,
            FZeroGXStage.EX_Victory_Lap,
        };
    }
}