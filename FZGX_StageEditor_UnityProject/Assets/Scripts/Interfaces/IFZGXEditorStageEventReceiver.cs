using System.IO;

namespace GameCube
{
    internal interface IFZGXEditorStageEventReceiver
    {
        void StageUnloaded(BinaryReader reader);
        void StageLoaded(BinaryReader reader);
    }
}