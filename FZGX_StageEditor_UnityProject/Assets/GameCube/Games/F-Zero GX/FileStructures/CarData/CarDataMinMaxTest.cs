using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameCube.Games.FZeroGX.FileStructures.CarData
{
    public class CarDataMinMaxTest : MonoBehaviour
    {
        [SerializeField]
        protected CarDataSobj carData;

        void Start()
        {
            // Machines
            RunMinMax(carData.CarStats.ToArray(), "Machine");
            RunMinMax(carData.CustomPartStats.GetRange(0, 25).ToArray(), "Body");
            RunMinMax(carData.CustomPartStats.GetRange(25, 25).ToArray(), "Cockpit");
            RunMinMax(carData.CustomPartStats.GetRange(50, 25).ToArray(), "Booster");
            QuickTextWriter.Save("StatsCarData", "tsv");
        }

        private void RunMinMax(CarStatsSobj[] stats, string title)
        {
            CarStatsSobj min = new CarStatsSobj();
            CarStatsSobj max = new CarStatsSobj();

            // mins
            min.acceleration = stats.Min(x => x.acceleration);
            min.body = stats.Min(x => x.body);
            min.boostDuration = stats.Min(x => x.boostDuration);
            min.boostStrength = stats.Min(x => x.boostStrength);
            min.cameraReorientation = stats.Min(x => x.cameraReorientation);
            min.cameraRepositioning = stats.Min(x => x.cameraRepositioning);
            min.drag = stats.Min(x => x.drag);
            min.driftAcceleration = stats.Min(x => x.driftAcceleration);
            min.grip1 = stats.Min(x => x.grip1);
            min.grip2 = stats.Min(x => x.grip2);
            min.grip3 = stats.Min(x => x.grip3);
            min.maxSpeed = stats.Min(x => x.maxSpeed);
            min.strafe = stats.Min(x => x.strafe);
            min.strafeTurn = stats.Min(x => x.strafeTurn);
            // Tilts
            min.turnDeceleration = stats.Min(x => x.turnDeceleration);
            min.turnMovement = stats.Min(x => x.turnMovement);
            min.turnReaction = stats.Min(x => x.turnReaction);
            min.turnTension = stats.Min(x => x.turnTension);
            min.unused_0x00 = stats.Min(x => x.unused_0x00);
            min.unknownEnumFlags_0x48 = stats.Min(x => x.unknownEnumFlags_0x48);
            min.unknownEnumFlags_0x49 = stats.Min(x => x.unknownEnumFlags_0x49);
            //min.unknownBool_0x49 = stats.Min(x => x.unknownBool_0x49);
            // Wall collisions
            min.weight = stats.Min(x => x.weight);

            // mins
            max.acceleration = stats.Max(x => x.acceleration);
            max.body = stats.Max(x => x.body);
            max.boostDuration = stats.Max(x => x.boostDuration);
            max.boostStrength = stats.Max(x => x.boostStrength);
            max.cameraReorientation = stats.Max(x => x.cameraReorientation);
            max.cameraRepositioning = stats.Max(x => x.cameraRepositioning);
            max.drag = stats.Max(x => x.drag);
            max.driftAcceleration = stats.Max(x => x.driftAcceleration);
            max.grip1 = stats.Max(x => x.grip1);
            max.grip2 = stats.Max(x => x.grip2);
            max.grip3 = stats.Max(x => x.grip3);
            max.maxSpeed = stats.Max(x => x.maxSpeed);
            max.strafe = stats.Max(x => x.strafe);
            max.strafeTurn = stats.Max(x => x.strafeTurn);
            // Tilts
            max.turnDeceleration = stats.Max(x => x.turnDeceleration);
            max.turnMovement = stats.Max(x => x.turnMovement);
            max.turnReaction = stats.Max(x => x.turnReaction);
            max.turnTension = stats.Max(x => x.turnTension);
            max.unused_0x00 = stats.Max(x => x.unused_0x00);
            max.unknownEnumFlags_0x48 = stats.Max(x => x.unknownEnumFlags_0x48);
            max.unknownEnumFlags_0x49 = stats.Max(x => x.unknownEnumFlags_0x49);
            //max.unknownBool_0x49 = stats.Max(x => x.unknownBool_0x49);
            // Wall collisions
            max.weight = stats.Max(x => x.weight);

            QuickTextWriter.WriteTsvLineEndToBuffer(title, "Min", "Max");
            QuickTextWriter.WriteTsvLineEndToBuffer("acceleration", $"{min.acceleration}", $"{max.acceleration}");
            QuickTextWriter.WriteTsvLineEndToBuffer("body", $"{min.body}", $"{max.body}");
            QuickTextWriter.WriteTsvLineEndToBuffer("boostDuration", $"{min.boostDuration}", $"{max.boostDuration}");
            QuickTextWriter.WriteTsvLineEndToBuffer("boostStrength", $"{min.boostStrength}", $"{max.boostStrength}");
            QuickTextWriter.WriteTsvLineEndToBuffer("cameraReorientation", $"{min.cameraReorientation}", $"{max.cameraReorientation}");
            QuickTextWriter.WriteTsvLineEndToBuffer("cameraRepositioning", $"{min.cameraRepositioning}", $"{max.cameraRepositioning}");
            QuickTextWriter.WriteTsvLineEndToBuffer("drag", $"{min.drag}", $"{max.drag}");
            QuickTextWriter.WriteTsvLineEndToBuffer("driftAcceleration", $"{min.driftAcceleration}", $"{max.driftAcceleration}");
            QuickTextWriter.WriteTsvLineEndToBuffer("grip1", $"{min.grip1}", $"{max.grip1}");
            QuickTextWriter.WriteTsvLineEndToBuffer("grip2", $"{min.grip2}", $"{max.grip2}");
            QuickTextWriter.WriteTsvLineEndToBuffer("grip3", $"{min.grip3}", $"{max.grip3}");
            QuickTextWriter.WriteTsvLineEndToBuffer("maxSpeed", $"{min.maxSpeed}", $"{max.maxSpeed}");
            QuickTextWriter.WriteTsvLineEndToBuffer("strafe", $"{min.strafe}", $"{max.strafe}");
            QuickTextWriter.WriteTsvLineEndToBuffer("strafeTurn", $"{min.strafeTurn}", $"{max.strafeTurn}");
            QuickTextWriter.WriteTsvLineEndToBuffer("turnDeceleration", $"{min.turnDeceleration}", $"{max.turnDeceleration}");
            QuickTextWriter.WriteTsvLineEndToBuffer("turnMovement", $"{min.turnMovement}", $"{max.turnMovement}");
            QuickTextWriter.WriteTsvLineEndToBuffer("turnReaction", $"{min.turnReaction}", $"{max.turnReaction}");
            QuickTextWriter.WriteTsvLineEndToBuffer("turnTension", $"{min.turnTension}", $"{max.turnTension}");
            QuickTextWriter.WriteTsvLineEndToBuffer("unk0x00", $"{min.unused_0x00}", $"{max.unused_0x00}");
            QuickTextWriter.WriteTsvLineEndToBuffer("unk1", $"{min.unknownEnumFlags_0x48}", $"{max.unknownEnumFlags_0x48}");
            QuickTextWriter.WriteTsvLineEndToBuffer("unk2", $"{min.unknownEnumFlags_0x49}", $"{max.unknownEnumFlags_0x49}");
            QuickTextWriter.WriteTsvLineEndToBuffer("unk3", $"{min.unused_0x4A}", $"{max.unused_0x4A}");
            QuickTextWriter.WriteTsvLineEndToBuffer("weight", $"{min.weight}", $"{max.weight}");
            QuickTextWriter.WriteTsvLineEndToBuffer();
        }
    }
}