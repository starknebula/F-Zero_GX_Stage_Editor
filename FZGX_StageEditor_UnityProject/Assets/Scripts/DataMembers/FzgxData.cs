using System.IO;

namespace FzgxData
{ 
    /// <summary>
    /// 
    /// </summary>
    public abstract class Member
    {
        public abstract uint size { get; }

        public abstract byte[] Serialize();
        public abstract void Deserialize(BinaryReader reader, uint address);
    }
}