using System.Collections.Generic;

namespace GameCube.Games.FzeroGX
{
    public enum FzgxStage
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
        public static readonly List<FzgxStage> AllStages = new List<FzgxStage>()
        {
            // RUBY CUP
            FzgxStage.MUTE_CITY_Twist_Road,
            FzgxStage.CASINO_PALACE_Split_Oval,
            FzgxStage.SAND_OCEAN_Surface_Slide,
            FzgxStage.LIGHTNING_Loop_Cross,
            FzgxStage.AEROPOLIS_Multiplex,

            // SAPPHIRE CUP
            FzgxStage.BIG_BLUE_Drift_Highway,
            FzgxStage.PORT_TOWN_Aero_Dive,
            FzgxStage.GREEN_PLANT_Mobius_Ring,
            FzgxStage.PORT_TOWN_Long_Pipe,
            FzgxStage.MUTE_CITY_Serial_Gaps,

            // EMERALD CUP
            FzgxStage.FIRE_FIELD_Cylinder_Knot,
            FzgxStage.GREEN_PLANT_Intersection,
            FzgxStage.CASINO_PALACE_Double_Branches,
            FzgxStage.LIGHTNING_Half_Pipe,
            FzgxStage.BIG_BLUE_Ordeal,

            // DIAMOND CUP
            FzgxStage.COSMO_TERMINAL_Trident,
            FzgxStage.SAND_OCEAN_Lateral_Shift,
            FzgxStage.FIRE_FIELD_Undulation,
            FzgxStage.AEROPOLIS_Dragon_Slope,
            FzgxStage.PHANTOM_ROAD_Slim_Line_Slits,

            // AX CUP
            FzgxStage.AEROPOLIS_Screw_Drive,
            FzgxStage.OUTER_SPACE_Meteor_Stream,
            FzgxStage.PORT_TOWN_Cylinder_Wave,
            FzgxStage.LIGHTNING_Thunder_Road,
            FzgxStage.GREEN_PLANT_Spiral,
            FzgxStage.MUTE_CITY_Sonic_Oval,

            // STORY
            FzgxStage.STORY_1,
            FzgxStage.STORY_2,
            FzgxStage.STORY_3,
            FzgxStage.STORY_4,
            FzgxStage.STORY_5,
            FzgxStage.STORY_6,
            FzgxStage.STORY_7,
            FzgxStage.STORY_8,
            FzgxStage.STORY_9,

            // EXTRA
            FzgxStage.EX_Grand_Prix_Podium,
            FzgxStage.EX_Victory_Lap,
        };
    }
}