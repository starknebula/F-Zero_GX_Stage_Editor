using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace GX_Data.SplineData
{
    [Serializable]
    public class SplineDataClass : GX
    {
        /// HIERARCHY STRUCTURE
        /// 1. Spline Data
        ///     2. Runtime Data
        ///     2. Editor Data
        ///         3. Library Table
        ///             4. Library Entry

        // Saving space
        public List<EditorData> EditorData { get; private set; }
        public Dictionary<uint, int> EditorDataTable { get; private set; }

        public Base[] Base;

        public SplineDataClass(BinaryReader reader) : base(reader)
        {
            // Initialize GX_Data
            header_const = new GX_Header_Data(0x08, 0x0C, SplineData.Base.size);
            header_pointer = new GX_Pointer(header_const, reader);

            #region Load EditorData & EditorDataTable
            Base[] getAddresses = new Base[header_pointer.count];
            reader.BaseStream.Seek(header_pointer.address, SeekOrigin.Begin);

            //Debug.LogFormat("{0}:{1}\n{2}:{3}", header_const.address, header_instance.address, header_const.count, header_instance.count);

            for (int i = 0; i < getAddresses.Length; i++)
                getAddresses[i] = new Base(reader);

            EditorData = new List<EditorData>();
            EditorDataTable = new Dictionary<uint, int>();
            foreach (Base c in getAddresses)
            {
                // If the index is not yet referenced
                if (!EditorDataTable.ContainsKey(c.EditorDataAddress))
                {
                    // Dredge data from file, add it to list
                    EditorData.Add(new EditorData(reader, c.EditorDataAddress, null));
                    // Add this index into table
                    // Index is EditorDataTable.Count (0, 1, 2...), references the new index made above
                    EditorDataTable.Add(c.EditorDataAddress, EditorDataTable.Count);
                }
            }
            #endregion
            #region Load Base and all sub components
            Base = new Base[header_pointer.count];
            for (uint i = 0; i < (uint)Base.Length; i++)
            {
                Base[i] = new Base(
                    reader,
                    header_pointer.address + i * header_const.size, // i * size indicates the address for the next entry
                    EditorDataTable
                    );
            }
            #endregion
        }
    }

    [Serializable]
    public struct Base
    {
        public const int size = 3 * 4; // 0x0C

        /*/ 0x00 32 /*/ public uint TrackSplitType;
        /*/ 0x04 32 /*/ public uint RuntimeDataAddress;
        /*/ 0x08 32 /*/ public uint EditorDataAddress;

        //
        public RuntimeData RuntimeData { get; private set; }
        public int EditorDataID { get; private set; }

        public Base(BinaryReader reader)
        {
            //reader.BaseStream.Seek(seekAddress, SeekOrigin.Begin);

            // Data Members
            TrackSplitType = reader.GetUInt32();
            RuntimeDataAddress = reader.GetUInt32();
            EditorDataAddress = reader.GetUInt32();

            RuntimeData = new RuntimeData();
            EditorDataID = 0;
        }
        public Base(BinaryReader reader, uint seekAddress, Dictionary<uint, int> database)
        {
            reader.BaseStream.Seek(seekAddress, SeekOrigin.Begin);

            // Data Members
            TrackSplitType = reader.GetUInt32();
            RuntimeDataAddress = reader.GetUInt32();
            EditorDataAddress = reader.GetUInt32();

            // Lead to
            RuntimeData = new RuntimeData(reader, RuntimeDataAddress);
            EditorDataID = database[EditorDataAddress]; // Database is calculated before this constructor is called
        }
    }
    [Serializable]
    public struct RuntimeData
    {
        public const int size = 20 * 4; // 0x50

        /*/ 0x00 32 /*/ public float segmentStartReference;
        /*/ 0x04 32 /*/ public float segmentEndReference;
        /*/ 0x08 32 /*/ public float startingtPositionAlongSpline;
        /*/ 0x0c 96 /*/ public Vector3 startTangent;
        /*/ 0x18 96 /*/ public Vector3 startPosition;
        /*/ 0x24 96 /*/ public float endingPositionAlongSpline;
        /*/ 0x28 96 /*/ public Vector3 endTangent;
        /*/ 0x34 96 /*/ public Vector3 endPosition;
        /*/ 0x40 32 /*/ public float lengthStart;
        /*/ 0x44 32 /*/ public float lengthEnd;
        /*/ 0x48 32 /*/ public float splinePointWidth; // Looks like an editor value, since changing it has no apparent effect
        /*/ 0x4C  8 /*/ public bool mergeFromLast;
        /*/ 0x4D  8 /*/ public bool mergeToNext;
        /*/ 0x4E 16 /*/ //NULL

        public RuntimeData(BinaryReader reader, uint seekAddress)
        {
            reader.BaseStream.Seek(seekAddress, SeekOrigin.Begin);

            segmentStartReference = reader.GetFloat();
            segmentEndReference = reader.GetFloat();
            startingtPositionAlongSpline = reader.GetFloat();
            startTangent = reader.GetVector3Normal();
            startPosition = reader.GetVector3Position();
            endingPositionAlongSpline = reader.GetFloat();
            endTangent = reader.GetVector3Normal();
            endPosition = reader.GetVector3Position();
            lengthStart = reader.GetFloat();
            lengthEnd = reader.GetFloat();
            splinePointWidth = reader.GetFloat();
            mergeFromLast = reader.GetBool();
            mergeToNext = reader.GetBool();
            //reader.SkipBytes(2); // Not needed as constructor call will reset seek position
        }
    }

    [Serializable]
    public class EditorData
    {
        public const int size = 20 * 4; // 0x50

        /*/ 0x00 16 /*/ public byte hierarchyPositionFlag; // 8-bit parent layer
        /*/ 0x02 16 /*/ public byte hasSubEditorData; //Children == [0C00]
        /*/ 0x04 32 /*/ public uint libraryAddress;
        /*/ 0x08 32 /*/ //NULL
        /*/ 0x0C 32 /*/ public uint childEditorDataCount;
        /*/ 0x10 32 /*/ public uint childEditorDataAddress; // Has something to do with spline branches
        /*/ 0x14 96 /*/ public Vector3 localScale;
        /*/ 0x20 96 /*/ public Quaternion localRotation;
        /*/ 0x2C 96 /*/ public Vector3 localPosition;
        /*/ 0x38 32 /*/ public uint unk_id; // Power of Two ID
        /*/ 0x3C 32 /*/ public float unk0x3C; // 0x3C and 0x40 are typically the same
        /*/ 0x40 32 /*/ public float unk0x40;
        /*/ 0x44 96 /*/ //NULL (VECTOR?)

        // INTERNAL STRUCTS
        public LibraryTable LibraryIndexTable { get; private set; }
        public EditorData[] childEditorData;
        public EditorData parent;
        public Vector3 GlobalPosition = new Vector3();
        public Quaternion GlobalRotation = new Quaternion();

        // CONSTRUCTOR
        public EditorData(BinaryReader reader, uint seekAddress, EditorData parent)
        {
            this.parent = parent;
            reader.BaseStream.Seek(seekAddress, SeekOrigin.Begin);

            hierarchyPositionFlag = reader.GetByte(); // 8-bit parent layer
            /*/ NULL 08 /*/ reader.SkipBytes(1);
            hasSubEditorData = reader.GetByte(); // 0x0C indicates children exist
            /*/ NULL 08 /*/ reader.SkipBytes(1);
            libraryAddress = reader.GetUInt32(); // NEW EDITORDATA
            /*/ NULL 32 /*/ reader.SkipBytes(4);
            childEditorDataCount = reader.GetUInt32();
            childEditorDataAddress = reader.GetUInt32();
            localScale = reader.GetVector3Scale();
            localRotation = reader.GetQuaternionRotation();
            localPosition = reader.GetVector3Position();
            unk_id = reader.GetUInt32();
            unk0x3C = reader.GetFloat();
            unk0x40 = reader.GetFloat();
            /*/ NULL 96 /*/
            reader.SkipBytes(12);

            LibraryIndexTable = new LibraryTable(reader, libraryAddress);

            if (parent != null)
            {
                GlobalPosition += parent.GlobalPosition; // not doing this right, do rotation AFTER
                GlobalRotation *= parent.GlobalRotation;
            }
            GlobalPosition += localPosition;
            GlobalRotation *= localRotation;

            // Recursive
            childEditorData = new EditorData[childEditorDataCount];
            for (uint ui = 0; ui < childEditorDataCount; ui++)
                childEditorData[ui] = new EditorData(reader, childEditorDataAddress + ui * size, this);
        }
    }

    [Serializable]
    public struct LibraryTable
    {
        public const uint size = 18 * 4; // 0x48

        /*/ 0x00 32 * 9 /*/ public uint[] count; // Int temp. to fix issues. Not all paths return uint
        /*/ 0x24 32 * 9 /*/ public uint[] address; // Can be 0x0, so you have to only jump when not 0x0!

        // INTERNAL STRUCTS
        public GenericEntry[][] LibraryEntry { get; private set; } // 9x?


        public LibraryTable(BinaryReader reader, uint seekAddress)
        {
            reader.BaseStream.Seek(seekAddress, SeekOrigin.Begin);

            count = new uint[9];
            address = new uint[9];
            for (int i = 0; i < 9; i++) count[i] = reader.GetUInt32();
            for (int i = 0; i < 9; i++) address[i] = reader.GetUInt32();

            #region Initialize LibraryEntry
            // Set constant size to 9
            LibraryEntry = new GenericEntry[9][];

            // Method to read data for LibraryEntry
            Func<BinaryReader, float> BinaryReader_GetFloat = delegate (BinaryReader r) { return r.GetFloat(); };

            for (int i = 0; i < 9; i++)
            {
                // Set the array size of each Entry to the count in the LibraryTable
                LibraryEntry[i] = new GenericEntry[count[i]];

                if (count[i] > 0)
                {
                    // Go to address
                    reader.BaseStream.Seek(address[i], SeekOrigin.Begin);

                    // Read all entries
                    for (int j = 0; j < count[i]; j++)
                        LibraryEntry[i][j] = new GenericEntry(reader);
                }
            }
            #endregion
        }
    }

    public class AiEffects : GX
    {
        public AiEffects(BinaryReader reader) : base(reader)
        {
            header_const = new GX_Header_Data(0x10, 0x14, 5u * 4);
            header_pointer = new GX_Pointer(header_const, reader);
        }
    }

}