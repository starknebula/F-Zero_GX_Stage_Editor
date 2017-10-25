// Created by Raphael "Stark" Tetreault 04/09/2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 04/09/2017

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCube.COLI
{
    #region ROOT MEMBER
    /// <summary>
    /// COLI.RootMembers are data that own a Count and Offset constants in COLI files.
    /// </summary>
    [Serializable]
    public abstract class RootMember : SerializableMember
    {
        protected abstract uint count { get; }
        protected abstract uint address { get; }

        public uint Count(BinaryReader reader)
        {
            reader.BaseStream.Seek(count, SeekOrigin.Begin);
            return reader.GetUInt32();
        }
        public uint Address(BinaryReader reader)
        {
            reader.BaseStream.Seek(address, SeekOrigin.Begin);
            return reader.GetUInt32();
        }
    }
    #endregion
    #region GENERAL DATA
    [Serializable]
    public struct ArrayPointer
    {
        public uint count;
        public uint address;

        public ArrayPointer(uint count, uint address)
        {
            this.count = count;
            this.address = address;
        }
    }
    [Serializable]
    public struct TriQuad
    {
        public float unk_0x00;
        public Vector3 normal;
        public Vector3[] vertices; // Tri 3, Quad 4
        public Vector3[] dat_vertices; // Tri 3, Quad 4. (unknown data)

        public TriQuad(BinaryReader reader, bool isTriangle)
        {
            unk_0x00 = reader.GetUInt32();
            normal = reader.GetVector3Normal();

            // 3 if triangle, 4 if quad
            int count = isTriangle ? 3 : 4;
            vertices = new Vector3[count];
            dat_vertices = new Vector3[count];

            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = reader.GetVector3Position();
            for (int i = 0; i < vertices.Length; i++)
                dat_vertices[i] = reader.GetVector3Position();
        }
    }
    #endregion
    #region SORT
    [Serializable]
    public sealed class CollisionTable : SerializableMember
    {
        #region CONSTRUCTORS
        public CollisionTable() { }
        public CollisionTable(BinaryReader reader, uint address)
        {
            Deserialize(reader, address);
        }
        #endregion
        #region MEMBERS
        /// <summary>
        /// 0x24
        /// </summary>
        public override uint size
        {
            get
            {
                return 0x24;
            }
        }

        /*/ 0x00 /*/
        private uint unk_0x00;
        /*/ 0x04 /*/
        private uint unk_0x04; // UNKNOWN TYPE
                               /*/ 0x08 /*/
        private uint unk_0x08; // UNKNOWN TYPE
                               /*/ 0x0C /*/
        private uint unk_0x0C; // UNKNOWN TYPE
                               /*/ 0x10 /*/
        private uint unk_0x10; // UNKNOWN TYPE
                               /*/ 0x14 /*/
        private uint triangleCount;
        /*/ 0x18 /*/
        private uint quadCount;
        /*/ 0x18 /*/
        private uint triangleAddress;
        /*/ 0x18 /*/
        private uint quadAddress;
        #endregion
        #region PROPERTIES
        public uint Unk_0x00
        {
            get
            {
                return unk_0x00;
            }
            internal set
            {
                unk_0x00 = value;
            }
        }
        public uint Unk_0x04
        {
            get
            {
                return unk_0x04;
            }
            internal set
            {
                unk_0x04 = value;
            }
        }
        public uint Unk_0x08
        {
            get
            {
                return unk_0x08;
            }
            internal set
            {
                unk_0x08 = value;
            }
        }
        public uint Unk_0x0C
        {
            get
            {
                return unk_0x0C;
            }
            internal set
            {
                unk_0x0C = value;
            }
        }
        public uint Unk_0x10
        {
            get
            {
                return unk_0x10;
            }
            internal set
            {
                unk_0x10 = value;
            }
        }
        public uint TriangleCount
        {
            get
            {
                return triangleCount;
            }
            internal set
            {
                triangleCount = value;
            }
        }
        public uint QuadCount
        {
            get
            {
                return quadCount;
            }
            internal set
            {
                quadCount = value;
            }
        }
        public uint TriangleAddress
        {
            get
            {
                return triangleAddress;
            }
            internal set
            {
                triangleAddress = value;
            }
        }
        public uint QuadAddress
        {
            get
            {
                return quadAddress;
            }
            internal set
            {
                quadAddress = value;
            }
        }
        #endregion
        #region METHODS
        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }
        public override void Deserialize(BinaryReader reader, uint address)
        {
            reader.BaseStream.Seek(address, SeekOrigin.Begin);

            unk_0x00 = reader.GetUInt32();
            unk_0x04 = reader.GetUInt32();
            unk_0x08 = reader.GetUInt32();
            unk_0x0C = reader.GetUInt32();
            unk_0x10 = reader.GetUInt32();
            triangleCount = reader.GetUInt32();
            quadCount = reader.GetUInt32();
            triangleAddress = reader.GetUInt32();
            quadAddress = reader.GetUInt32();
        }
        #endregion
    }
    #endregion

    #region 0x10 - 0x14: AI EFFECT REFERENCE
    /// <summary>
    /// Information fed to the AI to allow them to know the placement of effect collision
    /// along the track.
    /// </summary>
    [Serializable]
    public sealed class AiEffectReference : RootMember
    {
        public AiEffectReference() { }
        public AiEffectReference(BinaryReader reader, uint seekAddress)
        {
            Deserialize(reader, seekAddress);
        }

        /// <summary>
        /// 0x14
        /// </summary>
        public override uint size
        {
            get
            {
                return 0x14;
            }
        }
        /// <summary>
        /// 0x10
        /// </summary>
        protected override uint address
        {
            get
            {
                return 0x10;
            }
        }
        /// <summary>
        /// 0x14
        /// </summary>
        protected override uint count
        {
            get
            {
                return 0x14;
            }
        }

        /*/ 0x00 4 /*/
        private float positionL;
        /*/ 0x04 4 /*/
        private float positionR;
        /*/ 0x08 4 /*/
        private float widthL;
        /*/ 0x0C 4 /*/
        private float widthR;
        /*/ 0x10 1 /*/
        private byte effectCollisionType;
        /*/ 0x11 1 /*/
        private byte trackBranchID;
        /*/ 0x12 2 /*/ /////// NULL - 2 bytes

        public float PositionL
        {
            get
            {
                return positionL;
            }
            internal set
            {
                positionL = value;
            }
        }
        public float PositionR
        {
            get
            {
                return positionR;
            }
            internal set
            {
                positionR = value;
            }
        }
        public float WidthL
        {
            get
            {
                return widthL;
            }
            internal set
            {
                WidthL = value;
            }
        }
        public float WidthR
        {
            get
            {
                return widthR;
            }
            internal set
            {
                widthR = value;
            }
        }
        public CollisionType EffectCollisionType
        {
            get
            {
                return (CollisionType)effectCollisionType;
            }
            internal set
            {
                if ((int)value <= byte.MaxValue)
                    effectCollisionType = (byte)value;
                else
                    throw new OverflowException();
            }
        }
        public int TrackBranchID
        {
            get
            {
                return trackBranchID;
            }
            internal set
            {
                if ((int)value <= byte.MaxValue)
                    trackBranchID = (byte)value;
                else
                    throw new OverflowException();
            }
        }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }
        public override void Deserialize(BinaryReader reader, uint address)
        {
            reader.BaseStream.Seek(address, SeekOrigin.Begin);

            positionL = reader.GetFloat();
            positionR = reader.GetFloat();
            widthL = reader.GetFloat();
            widthR = reader.GetFloat();
            effectCollisionType = reader.GetByte();
            trackBranchID = reader.GetByte();
            //reader.SkipBytes(2);
        }
    }
    #endregion

    namespace TrackData
    {
        #region 0x08 - 0x0C: SPLINE
        [Serializable]
        public sealed class Spline : RootMember
        {
            public Spline() { }
            public Spline(BinaryReader reader)
            {
                Deserialize(reader, Address(reader));
            }

            /// <summary>
            /// 0x0C
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0x0C;
                }
            }
            /// <summary>
            /// 0x0C
            /// </summary>
            protected override uint address
            {
                get
                {
                    return 0x0C;
                }
            }
            /// <summary>
            /// 0x08
            /// </summary>
            protected override uint count
            {
                get
                {
                    return 0x08;
                }
            }

            private SplinePoint[] splinePoints;
            public SplinePoint[] Points
            {
                get
                {
                    return splinePoints;
                }
                internal set
                {
                    splinePoints = value;
                }
            }

            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                splinePoints = new SplinePoint[Count(reader)];
                for (uint i = 0; i < splinePoints.Length; i++)
                    splinePoints[i] = new SplinePoint(reader, address + (size * i));
            }
        }
        [Serializable]
        public sealed class SplinePoint : SerializableMember
        {
            #region CONSTRUCTORS
            public SplinePoint() { }
            public SplinePoint(BinaryReader reader, uint seekAddress)
            {
                Deserialize(reader, seekAddress);
            }
            #endregion
            #region MEMBERS
            /// <summary>
            /// 0x0C
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0x0C;
                }
            }

            /*/ 0x00 4 /*/
            private int trackSplitCount;
            /*/ 0x04 4 /*/
            private uint trackNodeAddress;
            /*/ 0x08 4 /*/
            private uint trackSegmentAddress;

            private SplineNode node;
            private SplineSegment segment;
            #endregion
            #region PROPERTIES
            public int TrackSplitCount
            {
                get
                {
                    return trackSplitCount;
                }
                internal set
                {
                    trackSplitCount = value;
                }
            }
            public uint TrackNodeAddress
            {
                get
                {
                    return trackNodeAddress;
                }
                internal set
                {
                    trackNodeAddress = value;
                }
            }
            public uint TrackSegmentAddress
            {
                get
                {
                    return trackSegmentAddress;
                }
                internal set
                {
                    trackSegmentAddress = value;
                }
            }

            public SplineNode Node
            {
                get
                {
                    return node;
                }
                internal set
                {
                    node = value;
                }
            }
            public SplineSegment Segment
            {
                get
                {
                    return segment;
                }
                internal set
                {
                    segment = value;
                }
            }
            #endregion
            #region METHODS
            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                reader.BaseStream.Seek(address, SeekOrigin.Begin);

                trackSplitCount = reader.GetInt32();
                trackNodeAddress = reader.GetUInt32();
                trackSegmentAddress = reader.GetUInt32();

                node = new SplineNode(reader, trackNodeAddress);
                segment = new SplineSegment(reader, trackSegmentAddress);
            }
            #endregion
        }
        [Serializable]
        public sealed class SplineNode : SerializableMember
        {
            #region CONSTRUCTORS
            public SplineNode() { }
            public SplineNode(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }
            #endregion
            #region MEMBERS
            /// <summary>
            /// 0x50
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0x50;
                }
            }

            /*
            // 0x00  4 //
            // 0x04  4 //
            // 0x08  4 //
            // 0x0c 12 //
            // 0x18 12 //
            // 0x24 12 //
            // 0x28 12 //
            // 0x34 12 //
            // 0x40  4 //
            // 0x44  4 //
            // 0x48  4 //
            // 0x4C  1 //
            // 0x4D  1 //
            // 0x4E  2 // NULL
            /**/

            private float nodeStartReference;
            private float nodeEndReference;
            private float startPositionAlongTrack;
            private Vector3 tangentStart;
            private Vector3 positionStart;
            private float endPositionAlongTrack;
            private Vector3 tangentEnd;
            private Vector3 positionEnd;
            private float lengthStart;
            private float lengthEnd;
            private float trackWidth; // Editor value? since changing it has no apparent effect
            private bool mergeFromLast;
            private bool mergeToNext;
            #endregion
            #region PROPERTIES
            public float NodeStartReference
            {
                get
                {
                    return nodeStartReference;
                }
                internal set
                {
                    nodeStartReference = value;
                }
            }
            public float NodeEndReference
            {
                get
                {
                    return nodeEndReference;
                }
                internal set
                {
                    nodeEndReference = value;
                }
            }
            public float StartPositionAlongTrack
            {
                get
                {
                    return startPositionAlongTrack;
                }
                internal set
                {
                    startPositionAlongTrack = value;
                }
            }
            public Vector3 TangentStart
            {
                get
                {
                    return tangentStart;
                }
                internal set
                {
                    tangentEnd = value;
                }
            }
            public Vector3 PositionStart
            {
                get
                {
                    return positionStart;
                }
                internal set
                {
                    positionStart = value;
                }
            }
            public float EndPositionAlongTrack
            {
                get
                {
                    return endPositionAlongTrack;
                }
                internal set
                {
                    endPositionAlongTrack = value;
                }
            }
            public Vector3 TangentEnd
            {
                get
                {
                    return tangentEnd;
                }
                internal set
                {
                    tangentEnd = value;
                }
            }
            public Vector3 PositionEnd
            {
                get
                {
                    return positionEnd;
                }
                internal set
                {
                    positionEnd = value;
                }
            }
            public float LengthStart
            {
                get
                {
                    return lengthStart;
                }
                internal set
                {
                    lengthStart = value;
                }
            }
            public float LengthEnd
            {
                get
                {
                    return lengthEnd;
                }
                internal set
                {
                    lengthEnd = value;
                }
            }
            public float TrackWidth
            {
                get
                {
                    return trackWidth;
                }
                internal set
                {
                    trackWidth = value;
                }
            }
            public bool MergeFromLast
            {
                get
                {
                    return mergeFromLast;
                }
                internal set
                {
                    mergeFromLast = value;
                }
            }
            public bool MergeToNext
            {
                get
                {
                    return mergeToNext;
                }
                internal set
                {
                    mergeToNext = value;
                }
            }
            #endregion
            #region METHODS
            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                reader.BaseStream.Seek(address, SeekOrigin.Begin);

                nodeStartReference = reader.GetFloat();
                nodeEndReference = reader.GetFloat();
                startPositionAlongTrack = reader.GetFloat();
                tangentStart = reader.GetVector3Normal();
                positionStart = reader.GetVector3Position();
                endPositionAlongTrack = reader.GetFloat();
                tangentEnd = reader.GetVector3Normal();
                positionEnd = reader.GetVector3Position();
                lengthStart = reader.GetFloat();
                lengthEnd = reader.GetFloat();
                trackWidth = reader.GetFloat();
                mergeFromLast = reader.GetBool();
                mergeToNext = reader.GetBool();
            }
            #endregion
        }
        [Serializable]
        public sealed class SplineSegment : SerializableMember
        {
            #region CONSTRUCTORS
            public SplineSegment() { }
            public SplineSegment(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }
            #endregion
            #region MEMBERS
            /// <summary>
            /// 0x50
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0x50;
                }
            }

            /*/ 0x00  2 /*/
            private ushort hierarchyPositionFlag; // Transform hierarchy layer mask
                                                  /*/ 0x02  2 /*/
            private ushort hasChild; // hasChild if equals 0x0C00
                                     /*/ 0x04  4 /*/
            private uint libraryAddress;
            /*/ 0x08  4 /*/ //NULL
                            /*/ 0x0C  4 /*/
            private uint childCount;
            /*/ 0x10  4 /*/
            private uint childAddress; // Has something to do with spline branches
                                       /*/ 0x14 12 /*/
            private Vector3 localScale;
            /*/ 0x20 12 /*/
            private Quaternion localRotation;
            /*/ 0x2C 12 /*/
            private Vector3 localPosition;
            /*/ 0x38  4 /*/
            private uint unk_id; // INT FLAG
                                 /*/ 0x3C  4 /*/
            private float unk0x3C; // 0x3C and 0x40 are typically the same
                                   /*/ 0x40  4 /*/
            private float unk0x40;
            /*/ 0x44 12 /*/ //NULL (VECTOR?)

            private SplineSegment parent;
            public SplineSegment Parent
            {
                get
                {
                    return parent;
                }
                internal set
                {
                    parent = value;
                }
            }
            private SplineSegment[] children;
            public SplineSegment[] Children
            {
                get
                {
                    return children;
                }
                internal set
                {
                    children = value;
                }
            }

            #endregion
            #region PROPERTIES
            public ushort HierarchyPositionFlag
            {
                get
                {
                    return hierarchyPositionFlag;
                }
                internal set
                {
                    hierarchyPositionFlag = value;
                }
            }
            public bool HasChild
            {
                get
                {
                    return hasChild == 0x0C00;
                }
                internal set
                {
                    hasChild = value ? (ushort)0x0C00 : (ushort)0x0000;
                }
            }
            public uint LibraryAddress
            {
                get
                {
                    return libraryAddress;
                }
                internal set
                {
                    libraryAddress = value;
                }
            }
            public uint ChildCount
            {
                get
                {
                    return childCount;
                }
                internal set
                {
                    childCount = value;
                }
            }
            public uint ChildAddress
            {
                get
                {
                    return childAddress;
                }
                internal set
                {
                    childAddress = value;
                }
            }
            public Vector3 LocalScale
            {
                get
                {
                    return localScale;
                }
                internal set
                {
                    localScale = value;
                }
            }
            public Quaternion LocalRotation
            {
                get
                {
                    return localRotation;
                }
                internal set
                {
                    localRotation = value;
                }
            }
            public Vector3 LocalPosition
            {
                get
                {
                    return localPosition;
                }
                internal set
                {
                    localPosition = value;
                }
            }
            public uint Unk_id
            {
                get
                {
                    return unk_id;
                }
                internal set
                {
                    unk_id = value;
                }
            }
            public float Unk0x3C
            {
                get
                {
                    return unk0x3C;
                }
                internal set
                {
                    unk0x3C = value;
                }
            }
            public float Unk0x40
            {
                get
                {
                    return unk0x40;
                }
                internal set
                {
                    unk0x40 = value;
                }
            }

            public Vector3 GlobalPosition()
            {
                Vector3 pos = Vector3.zero;

                if (parent != null)
                {
                    pos += parent.localRotation * localPosition;

                    pos.x *= parent.localScale.x;
                    pos.y *= parent.localScale.y;
                    pos.z *= parent.localScale.z;

                    pos += parent.GlobalPosition();
                }
                else
                    pos += LocalPosition;

                    return pos;
            }
            public Vector3 GlobalScale()
            {
                Vector3 scale = localScale;

                if (parent != null)
                {
                    Vector3 parentScale = parent.GlobalScale();

                    scale.x *= parentScale.x;
                    scale.y *= parentScale.y;
                    scale.z *= parentScale.z;
                }

                return scale;
            }
            public Quaternion GlobalRotation()
            {
                Quaternion rot = localRotation;

                if (parent != null)
                    rot *= parent.GlobalRotation();

                return rot;
            }

            #endregion
            #region METHODS
            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                //this.parent = parent;
                reader.BaseStream.Seek(address, SeekOrigin.Begin);

                hierarchyPositionFlag = reader.GetUInt16();
                hasChild = reader.GetUInt16();
                libraryAddress = reader.GetUInt32();
                reader.SkipBytes(4);
                childCount = reader.GetUInt32();
                childAddress = reader.GetUInt32();
                localScale = reader.GetVector3Scale();
                localRotation = reader.GetQuaternionRotation();
                localPosition = reader.GetVector3Position();
                unk_id = reader.GetUInt32();
                unk0x3C = reader.GetFloat();
                unk0x40 = reader.GetFloat();
                //reader.SkipBytes(12);

                //LibraryIndexTable = new LibraryTable(reader, libraryAddress);

                //if (parent != null)
                //{
                //    GlobalPosition += parent.GlobalPosition; // not doing this right, do rotation AFTER
                //    GlobalRotation *= parent.GlobalRotation;
                //}
                //GlobalPosition += localPosition;
                //GlobalRotation *= localRotation;

                //// Recursive
                //childEditorData = new EditorData[childEditorDataCount];
                //for (uint ui = 0; ui < childEditorDataCount; ui++)
                //    childEditorData[ui] = new EditorData(reader, childEditorDataAddress + ui * size, this);

                children = new SplineSegment[childCount];
                for (uint i = 0; i < childCount; i++)
                {
                    children[i] = new SplineSegment(reader, childAddress + (i * size));
                    children[i].parent = this;
                }
            }
            public int Depth()
            {
                SplineSegment seg = this;
                int value = 0;

                while (seg.parent != null)
                {
                    ++value;
                    seg = seg.parent;
                }

                return value;
            }

            #endregion
        }
        [Serializable]
        public sealed class LibraryTable : SerializableMember
        {
            #region CONSTRUCTORS
            public LibraryTable() { }
            public LibraryTable(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }
            #endregion
            #region MEMBERS
            /// <summary>
            /// 0x48
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0x48;
                }
            }

            /*/ 0x00 4 * 9 /*/
            public uint[] count; // Int temp. to fix issues. Not all paths return uint
                                 /*/ 0x24 4 * 9 /*/
            public uint[] address; // Can be 0x0, so you have to only jump when not 0x0!

            private LibraryEntry[][] libraryEntries;
            #endregion
            #region PROPERTIES
            public LibraryEntry[][] LibraryEntries
            {
                get
                {
                    return libraryEntries;
                }
                internal set
                {
                    value = libraryEntries;
                }
            }
            #endregion
            #region METHODS
            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint seekAddress)
            {
                reader.BaseStream.Seek(seekAddress, SeekOrigin.Begin);

                count = new uint[9];
                address = new uint[9];
                for (int i = 0; i < 9; i++) count[i] = reader.GetUInt32();
                for (int i = 0; i < 9; i++) address[i] = reader.GetUInt32();

                #region Initialize LibraryEntries
                // Set constant size to 9
                libraryEntries = new LibraryEntry[9][];

                for (int i = 0; i < libraryEntries.Length; i++)
                {
                    // Set the array size of each entry to the count in the LibraryTable
                    libraryEntries[i] = new LibraryEntry[count[i]];

                    // If exists, go to address, read all entries
                    if (count[i] > 0)
                    {
                        reader.BaseStream.Seek(address[i], SeekOrigin.Begin);
                        for (int j = 0; j < count[i]; j++)
                            libraryEntries[i][j] = new LibraryEntry(reader, address[i]);
                    }
                }
                #endregion
            }
            #endregion
        }
        [Serializable]
        public sealed class LibraryEntry : SerializableMember
        {
            #region CONSTRUCTORS
            public LibraryEntry() { }
            public LibraryEntry(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }
            #endregion
            #region MEMBERS
            /// <summary>
            /// 0x20
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0x20;
                }
            }

            /*/ 0x00 4 /*/
            private float unk_0x00;
            /*/ 0x04 4 /*/
            private float unk_0x04;
            /*/ 0x08 4 /*/
            private float unk_0x08;
            /*/ 0x0C 4 /*/
            private float unk_0x0C;
            /*/ 0x10 4 /*/
            private float unk_0x10;

            #endregion
            #region PROPERTIES
            public float Unk_0x00
            {
                get
                {
                    return unk_0x00;
                }
                internal set
                {
                    unk_0x00 = value;
                }
            }
            public float Unk_0x04
            {
                get
                {
                    return unk_0x04;
                }
                internal set
                {
                    unk_0x04 = value;
                }
            }
            public float Unk_0x08
            {
                get
                {
                    return unk_0x08;
                }
                internal set
                {
                    unk_0x08 = value;
                }
            }
            public float Unk_0x0C
            {
                get
                {
                    return unk_0x0C;
                }
                internal set
                {
                    unk_0x0C = value;
                }
            }
            public float Unk_0x10
            {
                get
                {
                    return unk_0x10;
                }
                internal set
                {
                    unk_0x10 = value;
                }
            }
            #endregion
            #region METHODS
            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                unk_0x00 = reader.GetFloat();
                unk_0x04 = reader.GetFloat();
                unk_0x08 = reader.GetFloat();
                unk_0x0C = reader.GetFloat();
                unk_0x10 = reader.GetFloat();
            }
            #endregion
        }
        #endregion
    }
    namespace Object
    {
        #region 0x48 - 0x54: OBJECT
        [Serializable]
        public sealed class Object : RootMember
        {
            #region CONSTRUCTORS
            public Object() { }
            public Object(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }
            #endregion
            #region MEMBERS
            /// <summary>
            /// 0x40
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0x40;
                }
            }
            /// <summary>
            /// 0x48
            /// </summary>
            protected override uint address
            {
                get
                {
                    return 0x48;
                }
            }
            /// <summary>
            /// 0x54
            /// </summary>
            protected override uint count
            {
                get
                {
                    return 0x54;
                }
            }

            // This is an object (like a prefab)
            // Using the last index we get to all the instances of it in the scene
            //
            /*/ 0x00 32 /*/
            private uint id1; // ID? - Varies A LOT, but seems to be 4 IDs
                              /*/ 0x04 32 /*/
            private uint id2; // FFFF FFFF or 8000 0000?
                              /*/ 0x08 32 /*/
            private uint lodAndCollisionAddress; // Offset to the collision for this object - part of 0x64 header
                                                 /*/ 0x0C 32 /*/
            private Vector3 position; // Confirmed position x y z
                                      /*/ 0x18 32 /*/
            private string unknown0x18; // ?
                                        /*/ 0x1C 32 /*/
            private string unknown0x1C; // ID?
                                        /*/ 0x20 32 /*/
            private Vector3 scale; // Confirmed scale x y z
                                   /*/ 0x2C 32 /*/ //NULL 32
                                                   /*/ 0x30 32 /*/
            private uint animationAddress;   // Offset 2
                                             /*/ 0x34 32 /*/
            private uint unknownAddress0x34; // Offset 3
                                             /*/ 0x38 32 /*/
            private uint skeletonAddress;    // Offset 4 - SKL models
                                             /*/ 0x3C 32 /*/
            private uint transformAddress;   // Transform: scale, rotation, position

            private SubData subData;
            #endregion
            #region PROPERTIES
            public uint ID1
            {
                get
                {
                    return id1;
                }
                internal set
                {
                    id1 = value;
                }
            }
            public uint ID2
            {
                get
                {
                    return id2;
                }
                internal set
                {
                    id2 = value;
                }
            }
            public uint LodAndCollisionAddress
            {
                get
                {
                    return lodAndCollisionAddress;
                }
                internal set
                {
                    lodAndCollisionAddress = value;
                }
            }
            public Vector3 Position
            {
                get
                {
                    return position;
                }
                internal set
                {
                    position = value;
                }
            }
            public string Unknown0x18
            {
                get
                {
                    return unknown0x18;
                }
                internal set
                {
                    unknown0x18 = value;
                }
            }
            public string Unknown0x1C
            {
                get
                {
                    return unknown0x1C;
                }
                internal set
                {
                    unknown0x1C = value;
                }
            }
            public Vector3 Scale
            {
                get
                {
                    return scale;
                }
                internal set
                {
                    scale = value;
                }
            }
            public uint AnimationAddress
            {
                get
                {
                    return animationAddress;
                }
                internal set
                {
                    animationAddress = value;
                }
            }
            public uint UnknownAddress0x34
            {
                get
                {
                    return unknownAddress0x34;
                }
                internal set
                {
                    unknownAddress0x34 = value;
                }
            }
            public uint SkeletonAddress
            {
                get
                {
                    return skeletonAddress;
                }
                internal set
                {
                    skeletonAddress = value;
                }
            }
            public uint TransformAddress
            {
                get
                {
                    return transformAddress;
                }
                internal set
                {
                    transformAddress = value;
                }
            }

            public string ObjectName { get { return subData.NameData.Name; } }
            #endregion
            #region METHODS
            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                reader.BaseStream.Seek(address, SeekOrigin.Begin);

                id1 = reader.GetUInt32();
                id2 = reader.GetUInt32();
                lodAndCollisionAddress = reader.GetUInt32();
                position = reader.GetVector3Position();
                unknown0x18 = reader.GetString(4);
                unknown0x1C = reader.GetString(4);
                scale = reader.GetVector3Scale();
                reader.SkipBytes(4); // SKIP
                animationAddress = reader.GetUInt32();
                unknownAddress0x34 = reader.GetUInt32();
                skeletonAddress = reader.GetUInt32();
                transformAddress = reader.GetUInt32();

                //sectionA = new AdditionalObjectInfo_A(reader, lodAndCollisionAddress);
                //transform = new ObjectTransform(reader, transformAddress);
                //animation = new ObjectAnimation(reader, animationAddress);
            }
            #endregion
        }
        /// <summary>
        /// Formerly COLLISION MESH OFFSET
        /// </summary>
        [Serializable]
        public sealed class SubData : SerializableMember
        {
            #region CONSTRUCTORS
            public SubData() { }
            public SubData(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }
            #endregion
            #region MEMBERS
            /// <summary>
            /// 0x10
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0x10;
                }
            }

            private uint unk_0x00;
            private uint unk_0x04;
            private uint objectNameDataAddress;
            private uint collisionDataAddress;

            private NameData nameData;
            #endregion
            #region
            public NameData NameData
            {
                get
                {
                    return nameData;
                }
            }
            #endregion
            #region METHODS
            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                reader.BaseStream.Seek(address, SeekOrigin.Begin);

                unk_0x00 = reader.GetUInt32();
                unk_0x04 = reader.GetUInt32();
                objectNameDataAddress = reader.GetUInt32();
                collisionDataAddress = reader.GetUInt32();

                //reader.BaseStream.Seek(bInfoAddress, SeekOrigin.Begin);
                //infoB = new AdditionalObjectInfo_B(reader);
                //collision = new ObjectCollision(reader, collisionAddress);
            }
            #endregion
        }
        /// <summary>
        /// Formerly SUPLIMENTARY INFO OFFSET
        /// </summary>
        [Serializable]
        public sealed class NameData : SerializableMember
        {
            #region CONSTRUCTORS
            public NameData() { }
            public NameData(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }
            #endregion
            #region MEMBERS
            /// <summary>
            /// 0x10
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0x10;
                }
            }

            /*/ 0x00 4 /*/ // NULL
                           /*/ 0x04 4 /*/
            private uint nameAddress;
            /*/ 0x08 4 /*/ // NULL
                           /*/ 0x0C 4 /*/
            private float unk_0x0C; // LOD related?

            /// <summary>
            /// Name of this object (stored in COLI footer)
            /// </summary>
            private string name;
            #endregion
            #region PROPERTIES
            /// <summary>
            /// Returns the name of this object (stored in COLI footer)
            /// </summary>
            public string Name
            {
                get
                {
                    return name;
                }
            }
            #endregion
            #region METHODS
            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                reader.BaseStream.Seek(address, SeekOrigin.Begin);

                reader.SkipBytes(4);
                nameAddress = reader.GetUInt32();
                reader.SkipBytes(4);
                unk_0x0C = reader.GetUInt32();

                name = GetName(reader, nameAddress);
            }

            /// <summary>
            /// Retrieves the name of this object from the COLI footer
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="nameAddress"></param>
            /// <returns></returns>
            private static string GetName(BinaryReader reader, uint nameAddress)
            {
                reader.BaseStream.Seek(nameAddress, SeekOrigin.Begin);
                string name = string.Empty;
                char c;

                // read char and assign
                while ((c = reader.GetChar()) != char.MinValue)
                {
                    name += c;
                }

                return name;
            }
            #endregion
        }
        [Serializable]
        public sealed class Animation : SerializableMember
        {
            #region CONSTRUCTORS
            public Animation() { }
            public Animation(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }
            #endregion
            #region MEMBERS
            /// <summary>
            /// 0x10
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0x10;
                }
            }

            private const int animationAddressCount = 6;
            private float unk_0x00;
            private float unk_0x04;
            private uint layer_16bit;

            // Always [6]
            private ArrayPointer[] pointers;

            // Always [6][X]
            public Entry[][] animationEntry;
            #endregion
            #region PROPERTIES
            public float Unk_0x00
            {
                get
                {
                    return unk_0x00;
                }
                internal set
                {
                    unk_0x00 = value;
                }
            }
            public float Unk_0x04
            {
                get
                {
                    return unk_0x04;
                }
                internal set
                {
                    unk_0x04 = value;
                }
            }
            public ushort Unk_Layer
            {
                get
                {
                    return (ushort)(layer_16bit >> 16);
                }
                internal set
                {
                    layer_16bit = (ushort)(value << 16);
                }
            }
            #endregion
            #region METHODS
            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                unk_0x00 = 0;
                unk_0x04 = 0;
                layer_16bit = 0;

                // Prevent false seeks
                if (address > 0)
                {
                    reader.BaseStream.Seek(address, SeekOrigin.Begin);

                    // HEADER //
                    unk_0x00 = reader.GetFloat();
                    unk_0x04 = reader.GetFloat();
                    reader.SkipBytes(4 * 4);
                    layer_16bit = reader.GetUInt32();
                    // HEADER //

                    pointers = new ArrayPointer[animationAddressCount];
                    // Read all animation addresses
                    for (int i = 0; i < animationAddressCount; i++)
                    {
                        reader.SkipBytes(4 * 4);
                        animationEntry[i] = new Entry[pointers[i].count];
                    }

                    // Read all animation entries at addresses previously read
                    for (int i = 0; i < animationAddressCount; i++)
                    {
                        reader.BaseStream.Seek(pointers[i].address, SeekOrigin.Begin);
                        for (int j = 0; j < animationEntry[i].Length; j++)
                            animationEntry[i][j] = new Entry(reader);
                    }
                }
                else // 
                {
                    for (int i = 0; i < animationEntry.Length; i++)
                        animationEntry[i] = new Entry[0];
                }
            }
            #endregion

            #region NESTED TYPES
            [Serializable]
            public struct Entry
            {
                public uint unk_0x00;  // Theory?: 1 = no, 2 = yes, 3 = yes!?
                public float keyTime;     // Keyframe time
                public Vector3 vector; // Can be pos, rot, scale?

                public Entry(BinaryReader reader)
                {
                    unk_0x00 = reader.GetUInt32();
                    keyTime = reader.GetFloat();
                    vector = reader.GetVector3Position();
                }
            }
            #endregion
        }
        [Serializable]
        public sealed class Transform : SerializableMember
        {
            #region CONSTRUCTOR 
            public Transform() { }
            public Transform(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }
            #endregion
            #region MEMBERS
            /// <summary>
            /// 0x30
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0x30;
                }
            }

            private Vector3 normalX;
            private Vector3 normalY;
            private Vector3 normalZ;
            private Vector3 position;

            // We can infer the rest of the transform
            private Vector3 scale;
            private Quaternion rotation;
            #endregion
            #region PROPERTIES
            public Vector3 Position
            {
                get
                {
                    return position;
                }
            }
            public Vector3 Rotation
            {
                get
                {
                    return rotation.eulerAngles;
                }
            }
            public Vector3 Scale
            {
                get
                {
                    return scale;
                }
            }
            #endregion
            #region METHODS
            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                reader.BaseStream.Seek(address, SeekOrigin.Begin);

                //Vector3 position = new Vector3();

                normalX = reader.GetVector3Normal();
                position.x = reader.GetFloat() * ((StageManager.doInverseWindingPositionX) ? -1f : 1f);
                normalY = reader.GetVector3Normal();
                position.y = reader.GetFloat();
                normalZ = reader.GetVector3Normal();
                position.z = reader.GetFloat();

                //position = position;

                // Use normal length to know scale for this axis
                scale = new Vector3(normalX.magnitude, normalY.magnitude, normalZ.magnitude);
                // Compare normal to that of world-space to get rotation
                rotation = Quaternion.Euler(Vector3.Angle(normalX, Vector3.right), Vector3.Angle(normalY, Vector3.up), Vector3.Angle(normalZ, Vector3.forward));
            }
            #endregion
        }
        [Serializable]
        public sealed class Collision : SerializableMember
        {
            #region CONSTRUCTORS
            public Collision() { }
            public Collision(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }
            #endregion
            #region MEMBERS
            /// <summary>
            /// 0x24
            /// </summary>
            public override uint size
            {
                get
                {
                    return 0X24;
                }
            }

            private uint id; // layer
            private uint unk_Tri_0x04;
            private uint unk_Quad_0x08;
            private uint unk_Tri_0x0C;
            private uint unk_Quad_0x10;
            private uint triCount;
            private uint quadCount;
            private uint triAddress;
            private uint quadAddress;

            private TriQuad[] triangles;
            private TriQuad[] quads;
            #endregion
            #region PROPERTIES
            public uint ID
            {
                get
                {
                    return id;
                }
                internal set
                {
                    id = value;
                }
            }
            public uint Unk_Tri_0x04
            {
                get
                {
                    return unk_Tri_0x04;
                }
                internal set
                {
                    unk_Tri_0x04 = value;
                }
            }
            public uint Unk_Quad_0x08
            {
                get
                {
                    return unk_Quad_0x08;
                }
                internal set
                {
                    unk_Quad_0x08 = value;
                }
            }
            public uint Unk_Tri_0x0C
            {
                get
                {
                    return unk_Tri_0x0C;
                }
                internal set
                {
                    unk_Tri_0x0C = value;
                }
            }
            public uint Unk_Quad_0x10
            {
                get
                {
                    return unk_Quad_0x10;
                }
                internal set
                {
                    unk_Quad_0x10 = value;
                }
            }
            public uint TriCount
            {
                get
                {
                    return triCount;
                }
                internal set
                {
                    triCount = value;
                }
            }
            public uint QuadCount
            {
                get
                {
                    return quadCount;
                }
                internal set
                {
                    quadCount = value;
                }
            }
            public uint TriAddress
            {
                get
                {
                    return triAddress;
                }
                internal set
                {
                    triAddress = value;
                }
            }
            public uint QuadAddress
            {
                get
                {
                    return quadAddress;
                }
                internal set
                {
                    quadAddress = value;
                }
            }

            public TriQuad[] Triangles
            {
                get
                {
                    return triangles;
                }
                internal set
                {
                    triangles = value;
                }
            }
            public TriQuad[] Quads
            {
                get
                {
                    return quads;
                }
                internal set
                {
                    quads = value;
                }
            }
            #endregion
            #region METHODS
            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                if (address > 0)
                {
                    reader.BaseStream.Seek(address, SeekOrigin.Begin);

                    id = reader.GetUInt32();

                    unk_Tri_0x04 = reader.GetUInt32();
                    unk_Quad_0x08 = reader.GetUInt32();
                    unk_Tri_0x0C = reader.GetUInt32();
                    unk_Quad_0x10 = reader.GetUInt32();

                    triCount = reader.GetUInt32();
                    quadCount = reader.GetUInt32();
                    triAddress = reader.GetUInt32();
                    quadAddress = reader.GetUInt32();

                    //Debug.LogFormat("Tri:{0} Quad:{1}", triCount, quadCount);

                    triangles = InitializeTriQuad(reader, triAddress, triCount, true);
                    quads = InitializeTriQuad(reader, quadAddress, quadCount, false);
                }
            }

            public TriQuad[] InitializeTriQuad(BinaryReader reader, uint address, uint count, bool isTriangle)
            {
                TriQuad[] triQuad = new TriQuad[count];

                if (count > 0)
                {
                    reader.BaseStream.Seek(address, SeekOrigin.Begin);
                    triQuad = new TriQuad[count];
                    for (int i = 0; i < count; i++)
                        triQuad[i] = new TriQuad(reader, isTriangle);
                }

                return triQuad;
            }
            #endregion
        }
        #endregion
    }

    #region 0x1C
    [Serializable]
    public sealed class Collision : RootMember
    {
        public Collision() { }
        public Collision(BinaryReader reader)
        {
            // Read offset into deserializer
            reader.BaseStream.Seek(address, SeekOrigin.Begin);
            Deserialize(reader, reader.GetUInt32());
        }

        /// <summary>
        /// 0xCC
        /// </summary>
        public override uint size
        {
            get
            {
                return 0xCC;
            }
        }
        /// <summary>
        /// 0x1C
        /// </summary>
        protected override uint address
        {
            get
            {
                return 0x1C;
            }
        }
        /// <summary>
        /// 0x00 - THIS PORTION DOES NOT HAVE A REGULAR COUNT
        /// </summary>
        protected override uint count
        {
            get
            {
                return 0x00;
            }
        }

        private const int blockAddressCount = 14;
        private uint triPoolAddress;
        private uint quadPoolAddress;
        private uint[] triBlockAddresses;
        private uint[] quadBlockAddresses;

        private TriQuad[] tris;
        private TriQuad[] quads;

        private IndexOffsetBlock[] indexOffsetBlockTri;
        private IndexOffsetBlock[] indexOffsetBlockQuad;

        public TriQuad[] Tris
        {
            get
            {
                return tris;
            }
        }
        public TriQuad[] Quads
        {
            get
            {
                return quads;
            }
        }
        public IndexOffsetBlock[] IndexOffsetBlocksTri
        {
            get
            {
                return indexOffsetBlockTri;
            }
            internal set
            {
                indexOffsetBlockTri = value;
            }
        }
        public IndexOffsetBlock[] IndexOffsetBlocksQuad
        {
            get
            {
                return indexOffsetBlockQuad;
            }
            internal set
            {
                indexOffsetBlockQuad = value;
            }
        }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }
        public override void Deserialize(BinaryReader reader, uint address)
        {
            reader.BaseStream.Seek(address, SeekOrigin.Begin);

            reader.SkipBytes(4 * 9);

            triPoolAddress = reader.GetUInt32();
            triBlockAddresses = new uint[blockAddressCount];
            for (int i = 0; i < triBlockAddresses.Length; i++)
                triBlockAddresses[i] = reader.GetUInt32();

            //
            reader.SkipBytes(4 * 6);

            quadPoolAddress = reader.GetUInt32();
            quadBlockAddresses = new uint[blockAddressCount];
            for (int i = 0; i < quadBlockAddresses.Length; i++)
                quadBlockAddresses[i] = reader.GetUInt32();

            //
            reader.SkipBytes(4 * 6);

            // Load TRI indices
            indexOffsetBlockTri = new IndexOffsetBlock[blockAddressCount];
            for (int i = 0; i < indexOffsetBlockTri.Length; i++)
                indexOffsetBlockTri[i] = new IndexOffsetBlock(reader, triBlockAddresses[i]);
            // Load QUAD indices
            indexOffsetBlockQuad = new IndexOffsetBlock[blockAddressCount];
            for (int i = 0; i < indexOffsetBlockQuad.Length; i++)
                indexOffsetBlockQuad[i] = new IndexOffsetBlock(reader, quadBlockAddresses[i]);

            tris = new TriQuad[GetLargestIndex(indexOffsetBlockTri)];
            reader.BaseStream.Seek(triPoolAddress, SeekOrigin.Begin);
            for (int i = 0; i < tris.Length; i++)
                tris[i] = new TriQuad(reader, true);

            quads = new TriQuad[GetLargestIndex(indexOffsetBlockQuad)];
            reader.BaseStream.Seek(quadPoolAddress, SeekOrigin.Begin);
            for (int i = 0; i < quads.Length; i++)
                quads[i] = new TriQuad(reader, false);
        }

        private ushort GetLargestIndex(IndexOffsetBlock[] indexOffsetBlock)
        {
            // Find the largest known index to use as tri/quad array size
            // The game probably just reads indices dynamically using address + index * tri/quad size
            ushort largestIndex = 0;

            foreach (IndexOffsetBlock block in indexOffsetBlock)
            {
                if (block.Sequences != null)
                {
                    foreach (IndexSequence sequence in block.Sequences)
                    {
                        if (sequence != null)
                        {
                            foreach (ushort index in sequence.Indices)
                            {
                                if (index > largestIndex)
                                {
                                    largestIndex = index;
                                }
                            }
                        }
                    }
                }
            }

            // Indices are n-1, so to compentsate ++
            return ++largestIndex;
        }

        #region NESTED TYPES
        [Serializable]
        public sealed class IndexOffsetBlock : SerializableMember
        {
            public IndexOffsetBlock() { }
            public IndexOffsetBlock(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }

            /// <summary>
            /// 256
            /// </summary>
            public override uint size
            {
                get
                {
                    return 256;
                }
            }

            private uint[] addresses;
            private IndexSequence[] sequences;

            public IndexSequence[] Sequences
            {
                get
                {
                    return sequences;
                }
                internal set
                {
                    sequences = value;
                }
            }

            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                reader.BaseStream.Seek(address, SeekOrigin.Begin);

                addresses = new uint[size];
                for (int i = 0; i < addresses.Length; i++)
                    addresses[i] = reader.GetUInt32();

                sequences = new IndexSequence[size];
                for (int i = 0; i < addresses.Length; i++)
                {
                    if (addresses[i] != 0x00)
                    {
                        sequences[i] = new IndexSequence(reader, addresses[i]);
                    }
                }
            }
        }
        [Serializable]
        public sealed class IndexSequence
        {
            public IndexSequence(BinaryReader reader, uint address)
            {
                reader.BaseStream.Seek(address, SeekOrigin.Begin);
                indices = ReadVertexIndices(reader);
            }

            private const ushort terminatingCharacter = ushort.MaxValue;
            private ushort[] indices;

            public ushort[] Indices
            {
                get
                {
                    return indices;
                }
                internal set
                {
                    indices = value;
                }
            }
            public TriQuad[] GetVertices(TriQuad[] vertsIn)
            {
                TriQuad[] vertsOut = new TriQuad[indices.Length];

                for (int i = 0; i < vertsOut.Length; i++)
                {
                    int index = indices[i];
                    vertsOut[i] = vertsIn[index];
                }

                return vertsOut;
            }

            private ushort[] ReadVertexIndices(BinaryReader reader)
            {
                ushort value;
                List<ushort> indices = new List<ushort>();

                while ((value = reader.GetUInt16()) != terminatingCharacter)
                {
                    indices.Add(value);
                }

                return indices.ToArray();
            }
        }
        #endregion
    }
    #endregion
    #region 0xAC - 0xB0: Object Path
    public sealed class ObjectPath : RootMember
    {
        public ObjectPath() { }
        public ObjectPath(BinaryReader reader)
        {
            Deserialize(reader, Address(reader));
        }

        /// <summary>
        /// 0x24
        /// </summary>
        public override uint size
        {
            get
            {
                return 0x24;
            }
        }
        /// <summary>
        /// 0x
        /// </summary>
        protected override uint count
        {
            get
            {
                return 0xA4;
            }
        }
        /// <summary>
        /// 0x
        /// </summary>
        protected override uint address
        {
            get
            {
                return 0xA8;
            }
        }

        private Path[] paths;

        public Path[] Paths
        {
            get
            {
                return paths;
            }
            internal set
            {
                paths = value;
            }
        }

        public override byte[] Serialize()
        {
            throw new NotImplementedException();
        }
        public override void Deserialize(BinaryReader reader, uint address)
        {
            //reader.BaseStream.Seek(address, SeekOrigin.Begin);

            paths = new Path[Count(reader)];
            for (uint i = 0; i < paths.Length; i++)
                paths[i] = new Path(reader, address + (size * i));
        }

        public sealed class Path : SerializableMember
        {
            public Path() { }
            public Path(BinaryReader reader, uint address)
            {
                Deserialize(reader, address);
            }

            public override uint size
            {
                get
                {
                    return 0x24;
                }
            }

            private Vector3 positionStart;
            private uint unk_0x0C;
            private uint unk_0x10;
            private Vector3 positionEnd;
            private uint unk_0x20;

            public Vector3 PositionStart
            {
                get
                {
                    return positionStart;
                }
                internal set
                {
                    positionStart = value;
                }
            }
            public uint Unk_0x0C
            {
                get
                {
                    return unk_0x0C;
                }
                internal set
                {
                    unk_0x0C = value;
                }
            }
            public uint Unk_0x10
            {
                get
                {
                    return unk_0x10;
                }
                internal set
                {
                    unk_0x10 = value;
                }
            }
            public Vector3 PositionEnd
            {
                get
                {
                    return positionEnd;
                }
                internal set
                {
                    positionEnd = value;
                }
            }
            public uint Unk_0x20
            {
                get
                {
                    return unk_0x20;
                }
                internal set
                {
                    unk_0x20 = value;
                }
            }

            public override byte[] Serialize()
            {
                throw new NotImplementedException();
            }
            public override void Deserialize(BinaryReader reader, uint address)
            {
                reader.BaseStream.Seek(address, SeekOrigin.Begin);

                positionStart = reader.GetVector3Position();
                unk_0x0C = reader.GetUInt32();
                unk_0x10 = reader.GetUInt32();
                positionEnd = reader.GetVector3Position();
                unk_0x20 = reader.GetUInt32();
            }
        }


    }
    #endregion


    #region ENUMERATIONS
    // https://docs.google.com/spreadsheets/d/1ZeB0_k8blhKfIxIEcTX6zvPm_ioqe4ukpeFtdo9AOMU/edit#gid=0
    public enum CollisionType
    {
        Driveable = 0,
        Recover,
        unk_HardCollision,
        Boost,
        Jump,
        Ice,
        Dirt,
        Lava,
        OutOfBounds,
        Kill_1,
        Kill_2,
        unk_Kill_3,
        Kill_4
    }
    #endregion
}