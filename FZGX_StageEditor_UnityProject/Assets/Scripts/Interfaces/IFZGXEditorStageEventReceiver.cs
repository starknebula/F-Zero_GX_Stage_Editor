using System.IO;

namespace FzgxData
{
    internal interface IFZGXEditorStageEventReceiver
    {
        void StageUnloaded(BinaryReader reader);
        void StageLoaded(BinaryReader reader);
    }
}