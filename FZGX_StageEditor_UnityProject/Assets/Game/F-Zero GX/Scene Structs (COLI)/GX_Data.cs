using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GX_Data
{
    /// <summary>
    /// GENERIC HEADER DATA FOR ALL GX DATA MEMBERS
    /// </summary>
    [Serializable]
    public struct GX_Header_Data
    {
        public uint address { get; private set; }
        public uint count { get; private set; }
        public uint size { get; private set; }

        // CONSTRUCTOR
        public GX_Header_Data(uint count, uint address, uint size)
        {
            this.address = address;
            this.count = count;
            this.size = size;
        }
    }
    [Serializable]
    public struct GX_Pointer
    {
        public uint address;// { get; private set; }
        public uint count;// { get; private set; }

        // CONSTRUCTOR
        public GX_Pointer(GX_Header_Data headerConstants, BinaryReader reader)
        {
            reader.BaseStream.Seek(headerConstants.count, SeekOrigin.Begin);
            this.count = reader.GetUInt32();

            reader.BaseStream.Seek(headerConstants.address, SeekOrigin.Begin);
            this.address = reader.GetUInt32();
        }
    }

    /// <summary>
    /// Generic data member for all GX Data types
    /// </summary>
    [Serializable]
    public class GX
    {
        public GX_Header_Data header_const;
        public GX_Pointer header_pointer;

        public GX(BinaryReader reader) { }

    }

    [Serializable]
    public struct GenericEntry
    {
        public uint ID_1;
        public uint ID_2;
        public float field_1;
        public float field_2;
        public float field_3;

        public GenericEntry(BinaryReader reader)
        {
            ID_1 = reader.GetUInt32();
            ID_2 = reader.GetUInt32();

            field_1 = reader.GetFloat();
            field_2 = reader.GetFloat();
            field_3 = reader.GetFloat();
        }
        new public string ToString()
        {
            return string.Format("{0}\t{1}\t{2}\t{3}\t{4}", ID_1, BitConverter.ToSingle(BitConverter.GetBytes(ID_2), 0), field_1, field_2, field_3);
        }

        public Vector3 AsVector3(bool b)
        {
            return new Vector3(field_1 * ((b) ? -1f : 1f), field_2, field_3);
        }
    }
}