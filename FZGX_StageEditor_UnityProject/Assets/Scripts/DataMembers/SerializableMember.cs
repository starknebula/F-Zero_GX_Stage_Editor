using System.IO;

namespace GameCube
{ 
    /// <summary>
    /// Base class to inherit to facilitate loading and saving members from proprietary GC files.
    /// </summary>
    public abstract class SerializableMember
    {
        public abstract uint size { get; }

        public abstract byte[] Serialize();
        public abstract void Deserialize(BinaryReader reader, uint address);
    }
}