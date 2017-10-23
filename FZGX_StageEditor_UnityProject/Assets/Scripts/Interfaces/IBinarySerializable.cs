// Created by Raphael "Stark" Tetreault 23/10/2017
// Copyright (c) 2017 Raphael Tetreault
// Last updated 23/10/2017

namespace System.IO
{
    public interface IBinarySerializable
    {
        byte[] Serialize();
        void Deserialize(BinaryReader reader, long address);
    }
}