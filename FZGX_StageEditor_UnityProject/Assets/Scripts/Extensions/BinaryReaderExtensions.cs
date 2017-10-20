// Created by Raphael "Stark" Tetreault 07/06/2015
// Copyright © 2016 Raphael Tetreault
// Last updated 29/10/2016

namespace System.IO
{
    /// <summary>
    /// A suite of extensions for <c>System.IO.BinaryReader</c> which enables reading a Stream in either Big-Endian and Little-Endian ('endianess').
    /// </summary>
    public static partial class BinaryReaderExtensions
    {
        /// <summary>
        /// Indicates the byte order ("endianness") in which data is read from the stream.
        /// </summary>
        /// <remarks>
        /// This field is set to <c>true</c> by default.
        /// </remarks>
        public static bool isLittleEndian = true;

        // To be implemented
        /*/
        /// <summary>
        /// When <c>true</c>, forces BinaryReaderExtensions.encoding upon the BinaryReader.
        /// </summary>
        public static bool forceEncoding = false;
        /// <summary>
        /// The encoding that is forced upon the Binary reader when BinaryReaderExtensions.forceEncoding is <c>true</c>.
        /// </summary>
        public static Text.Encoding encoding = Text.Encoding.UTF8;
        //*/

        /// <summary>
        /// Reads the <paramref name="bitCount"/> amount of bits at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c></param>
        /// <param name="index">the bit to start reading from the byte.</param>
        /// <param name="bitCount">The amount of bits to read.</param>
        public static byte GetBits(this BinaryReader binaryReader, int index, int bitCount)
        {
            if (index    > 7)
                throw new IndexOutOfRangeException("The index specified for BinaryReaderExceptions.GetBits is greater than 7 which is out of range!");
            if (bitCount > 7)
                throw new IndexOutOfRangeException("BinaryReaderExceptions.GetBits cannot read more than 8 bits!");

            byte b = binaryReader.ReadByte();
            b = (byte)(b >> index);
            b = (byte)(b & ((1 << bitCount) - 1));

            return b;
        }

        /// <summary>
        /// Reads the next <c>bool</c> from the current stream and advances the current position of the stream by 1 byte.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static bool GetBool(this BinaryReader binaryReader)
        {
            return binaryReader.ReadBoolean();
        }
        /// <summary>
        /// Skips <paramref name="count"/> amount of bytes, reads the next <c>bool</c> from the current stream and advances the current position of the stream by 1 byte.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        /// <param name="count">The amount of bytes to skip before reading a boolean.</param>
        /// <remarks>
        /// This method is more specifically tuned towards reading data made by other sources.
        /// It isn't uncommon for booleans to be save out as 16-bit or 32-bit entries, thusly
        /// being padded by bytes beforehand. This method allows you to skip the padding
        /// and read the boolean. 16-bit:[<c>00 01</c>], 32-bit:[<c>00 00 00 01</c>]
        /// </remarks>
        public static bool GetBool(this BinaryReader binaryReader, int count)
        {
            binaryReader.BaseStream.Position += count;
            return binaryReader.ReadBoolean();
        }

        /// <summary>
        /// Reads the next <c>byte</c> from the current stream and advances the current position of the stream by 1 byte.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static byte GetByte(this BinaryReader binaryReader)
        {
            return binaryReader.ReadByte();
        }
        /// <summary>
        /// Reads the next <paramref name="count"/> bytes from the current stream and advances the current position of the stream by <paramref name="count"/> bytes.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        /// <param name="count">The amount of sbytes to read from the current BinaryStream.</param>
        public static byte[] GetBytes(this BinaryReader binaryReader, int count)
        {
            return binaryReader.ReadBytes(count);
        }

        /// <summary>
        /// Reads the next <c>sbyte</c> from the current stream and advances the current position of the stream by 1 byte.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static sbyte GetSByte(this BinaryReader binaryReader)
        {
            return binaryReader.ReadSByte();
        }
        /// <summary>
        /// Reads the next <paramref name="count"/> sbytes from the current stream and advances the current position of the stream by <paramref name="count"/> bytes.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        /// <param name="count">The amount of sbytes to read from the current BinaryStream.</param>
        public static sbyte[] GetSBytes(this BinaryReader binaryReader, int count)
        {
            sbyte[] bytes = new sbyte[count];

            for (int i = 0; i < count; i++)
                bytes[i] = binaryReader.ReadSByte();

            return bytes;
        }

        /// <summary>
        /// Reads the next <c>short</c> from the current stream and advances the current position of the stream by 2 bytes.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static short GetInt16(this BinaryReader binaryReader)
        {
            byte[] bytes = binaryReader.ReadBytes(2);
            if (isLittleEndian) Array.Reverse(bytes);

            return BitConverter.ToInt16(bytes, 0);
        }
        /// <summary>
        /// Reads the next <c>ushort</c> from the current stream and advances the current position of the stream by 2 bytes.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static ushort GetUInt16(this BinaryReader binaryReader)
        {
            byte[] bytes = binaryReader.ReadBytes(2);
            if (isLittleEndian) Array.Reverse(bytes);

            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// Reads the next <c>int</c> from the current stream and advances the current position of the stream by 4 bytes.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static int GetInt32(this BinaryReader binaryReader)
        {
            byte[] bytes = binaryReader.ReadBytes(4);
            if (isLittleEndian) Array.Reverse(bytes);

            return BitConverter.ToInt32(bytes, 0);
        }
        /// <summary>
        /// Reads the next <c>uint</c> from the current stream and advances the current position of the stream by 4 bytes.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static uint GetUInt32(this BinaryReader binaryReader)
        {
            byte[] bytes = binaryReader.ReadBytes(4);
            if (isLittleEndian) Array.Reverse(bytes);

            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Reads the next <c>long</c> from the current stream and advances the current position of the stream by 8 bytes.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static long GetInt64(this BinaryReader binaryReader)
        {
            byte[] bytes = binaryReader.ReadBytes(8);
            if (isLittleEndian) Array.Reverse(bytes);

            return BitConverter.ToInt64(bytes, 0);
        }
        /// <summary>
        /// Reads the next <c>ulong</c> from the current stream and advances the current position of the stream by 8 bytes.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static ulong GetUInt64(this BinaryReader binaryReader)
        {
            byte[] bytes = binaryReader.ReadBytes(8);
            if (isLittleEndian) Array.Reverse(bytes);

            return BitConverter.ToUInt64(bytes, 0);
        }

        /// <summary>
        /// Reads the next <c>float</c> from the current stream and advances the current position of the stream by 4 bytes.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static float GetFloat(this BinaryReader binaryReader)
        {
            byte[] bytes = binaryReader.ReadBytes(4);
            if (isLittleEndian) Array.Reverse(bytes);

            return BitConverter.ToSingle(bytes, 0);
        }
        /// <summary>
        /// Reads the next <c>double</c> from the current stream and advances the current position of the stream by 8 bytes.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static double GetDouble(this BinaryReader binaryReader)
        {
            byte[] bytes = binaryReader.ReadBytes(8);
            if (isLittleEndian) Array.Reverse(bytes);

            return BitConverter.ToDouble(bytes, 0);
        }
        /// <summary>
        /// Reads the next <c>decimal</c> from the current stream and advances the current position of the stream by 16 bytes.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static decimal GetDecimal(this BinaryReader binaryReader)
        {
            byte[] bytes = binaryReader.ReadBytes(16);
            if (isLittleEndian) Array.Reverse(bytes);

            // Merge 4 bytes into 1 int, then 4 ints into 1 decimal
            return new decimal(new int[]
            {
                BitConverter.ToInt32(bytes, 0),
                BitConverter.ToInt32(bytes, 4),
                BitConverter.ToInt32(bytes, 8),
                BitConverter.ToInt32(bytes, 12),
            });
        }

        /// <summary>
        /// Returns a <c>string</c> (4 bytes) formated as <c>1122 3344</c>.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static string GetHexString(this BinaryReader binaryReader)
        {
            return binaryReader.GetHexString(4, 2);
        }
        /// <summary>
        /// Returns a <c>string</c> of bytes of <paramref name="size"/> formated as <c>00</c> with a space padding every other byte.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        /// <param name="size">The amount of bytes to read from the stream.</param>
        public static string GetHexString(this BinaryReader binaryReader, int size)
        {
            return binaryReader.GetHexString(size, 2);
        }
        /// <summary>
        /// Returns a <c>string</c> of bytes of <paramref name="size"/> in the specified group size.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        /// <param name="size">The amount of bytes to read from the stream.</param>
        /// <param name="bytesPerGroup">The amount of bytes to cluster before separating them with a space character.</param>
        public static string GetHexString(this BinaryReader binaryReader, int size, int bytesPerGroup)
        {
            byte[] bytes = binaryReader.ReadBytes(size);
            string outputString = null;

            for (int i = 0; i < bytes.Length; i++)
            {
                outputString += bytes[i].ToString("X2");

                if (i != bytes.Length)
                    if ((i + 1) % bytesPerGroup == 0)
                        outputString += " ";
            }

            return outputString;
        }

        /// <summary>
        /// Reads the next <c>char</c> from the current stream and advances the current position of the stream in
        /// accordance with the Encoding used and the specific character being read from the stream.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        public static char GetChar(this BinaryReader binaryReader)
        {
            return binaryReader.ReadChar();
        }
        /// <summary>
        /// Reads the next <paramref name="count"/> amount of <c>char</c> from the current stream and advances the current position of the stream in
        /// accordance with the Encoding used and the specific character being read from the stream.        
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        /// <param name="count">The amount of char characters to read from the current stream.</param>
        public static char[] GetChars(this BinaryReader binaryReader, int count)
        {
            return binaryReader.ReadChars(count);
        }

        /// <summary>
        /// Returns <paramref name="size"/> amount of bytes as <c>string</c>.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        /// <param name="size">The amount of bytes to read from the current stream.</param>
        public static string GetString(this BinaryReader binaryReader, int size)
        {
            return binaryReader.GetString(size, null);
        }
        /// <summary>
        /// Returns <paramref name="size"/> amount of bytes as <c>string</c> in the specified <c>ToString</c> format.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        /// <param name="size">The amount of bytes to read from the current stream.</param>
        /// <param name="format">The IFormatProvider for ToString().</param>
        public static string GetString(this BinaryReader binaryReader, int size, string format)
        {
            byte[] bytes = binaryReader.ReadBytes(size);
            string outputString = null;

            foreach (byte b in bytes)
                outputString += b.ToString(format);

            return outputString;
        }

        /// <summary>
        /// Continuously reads characters out of stream until it reaches the specified terminating character.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        /// <param name="terminator">The character which terminates reading from the stream.</param>
        public static string GetVariableLengthString(this BinaryReader binaryReader, char terminator)
        {
            string outputString = null;

            while (binaryReader.PeekChar() != terminator)
                outputString += binaryReader.ReadChar();

            return outputString;
        }
        /// <summary>
        /// Continuously reads characters out of stream until it reaches any of the specified terminating characters.
        /// </summary>
        /// <param name="binaryReader">The current <c>BinaryReader</c>.</param>
        /// <param name="terminators">The characters which terminate reading from the stream.</param>
        public static string GetVariableLengthString(this BinaryReader binaryReader, params char[] terminators)
        {
            string outputString = null;
            char currentChar = new char();
            bool breakLoop = false;

            while (true)
            {
                currentChar = binaryReader.ReadChar();

                foreach (char terminator in terminators)
                    if (currentChar == terminator)
                        breakLoop = true;

                if (breakLoop)
                    break;

                outputString += currentChar;
            }

            return outputString;
        }
    }
}