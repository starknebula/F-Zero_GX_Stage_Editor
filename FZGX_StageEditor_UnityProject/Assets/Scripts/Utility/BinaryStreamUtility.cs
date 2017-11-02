//// Created by Raphael "Stark" Tetreault 01/06/2016
//// Copyright © 2016 Raphael Tetreault
//// Last updated 24/10/2017

//namespace System.IO
//{
//    /// <summary>
//    /// Utility to serialize any basic C# type to a binary stream.
//    /// </summary>
//    /// <remarks>
//    /// This library was not designed for or intended to be used with large streams. It is
//    /// recommended to keep streams under 5MB.
//    /// 
//    /// Valid value types:
//    /// <list type="bullet">
//    ///   <item>bool</item>
//    ///   <item>char</item>
//    ///   <item>string</item>
//    ///   <item>sbyte</item>
//    ///   <item>byte</item>
//    ///   <item>short</item>
//    ///   <item>ushort</item>
//    ///   <item>int</item>
//    ///   <item>uint</item>
//    ///   <item>long</item>
//    ///   <item>ulong</item>
//    ///   <item>float</item>
//    ///   <item>double</item>
//    ///   <item>decimal</item>
//    ///   <item>enum</item>
//    /// </list>
//    /// </remarks>
//    public static class BinaryStreamUtility
//    {
//        /// <summary>
//        /// Indicates the byte order ("endianness") in which data is written to the stream.
//        /// </summary>
//        /// <remarks>
//        /// This field is set to <c>false</c> by default.
//        /// </remarks>
//        private static bool isLittleEndian = false;
//        /// <summary>
//        /// Indicates the byte order ("endianness") in which data is written to the stream.
//        /// </summary>
//        /// <remarks>
//        /// This field is set to <c>false</c> by default.
//        /// </remarks>
//        public static bool IsLittleEndian
//        {
//            get
//            {
//                return isLittleEndian;
//            }
//            set
//            {
//                isLittleEndian = value;
//            }
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        public static event Action StreamWriteFinish_Callback;

//        /// <summary>
//        /// Writes dynamicData to the end of reference BinaryWriter. If any part of dynamicData is an array
//        /// (multidimensional <c>[ , ]</c> or jagged <c>[ ][ ]</c>) or a list, including lists of the aforementioned types,
//        /// it will recursively break it down to it's most basic data component then serialize it in order presented in array/list.
//        /// </summary>
//        /// <param name="binaryWriter">The <c>BinaryReader</c> to write the <paramref name="dynamicData"/> to.</param>
//        /// <param name="fileName">The name of the file to open or create using the <c>BinaryWriter</c>.</param>
//        /// <param name="dynamicData">The data to write to the <c>BinaryWriter</c>.</param>
//        public static void WriteDynamicDataRecursively(BinaryWriter binaryWriter, string fileName, params dynamic[] dynamicData)
//        {
//            // It is important to know that a dynamic[] is really just a special case object[].
//            // This means that what we are receiving from the method is a boxed variable type.
//            // https://msdn.microsoft.com/en-us/library/yz2be5wk.aspx?f=255&MSPPError=-2147217396
//            //
//            // The thing to note is that what we wish to do in this method is unbox the object
//            // into it's smallest possible value type, such as int, float, etc., so that we may
//            // serialize it in it's binary form with BitConverter. To do so we must unbox the
//            // object[], then if we have an array or list, "unbox" it into its individual
//            // components. To make this truly useful, we would need the method to know how deeply
//            // to unbox the type. That is why this method is recursive; it will detect when it
//            // needs to unbox the item again. However, in doing so we end up reboxing the data into
//            // an object[] once more. This means we must unpack the object[], then unbox the array
//            // or list before passing it to recursive method again, otherwise we would just end up
//            // unboxing the value, checking if it is an array or list, and then immediately
//            // reboxing it.
//            //
//            // We can check the data type by using Type.IsArray for any type of
//            // array (jagged [][] or multidimensional [,]) or Type.IsGenericType for lists. I 
//            // use a foreach loop twice; once to unbox the object[], then again if necessary
//            // to unbox the array or list. At this point we know that the method will handle
//            // any boxed type so we can then proceed to serializing the data, AKA the unboxed
//            // keyword types like int, float, decimal, and char. I added a special case for
//            // strings too, although they technically aren't a System.ValueType, which are
//            // the only types BinaryWriter support.
//            //
//            // At this point you should be able to follow along with the code/comments below.

//            using (binaryWriter = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)))
//            {
//                // Foreach object in object[]
//                foreach (dynamic value in dynamicData)
//                {
//                    // Store value's type, will be used often
//                    Type valueType = ((object)value).GetType();

//                    // If the unboxed type is an array
//                    if (valueType.IsArray)
//                        // Unbox the array further
//                        foreach (dynamic subValue in value)
//                            // Pass each unboxed item back to this method
//                            WriteDynamicDataRecursively(binaryWriter, fileName, subValue);
//                    // If the unboxed type is a list
//                    else if (valueType.IsGenericType)
//                        // Unbox the list further
//                        foreach (dynamic subValue in value)
//                            // Pass each unboxed item back to this method
//                            WriteDynamicDataRecursively(binaryWriter, fileName, subValue);

//                    // Prevent any array or list from proceeding as we cannot handle it. Though
//                    // We broke it down, the array is still working it's way through this method.
//                    // We skip the array to the end of the loop so it bypasses the oncoming checks
//                    // as it's sub-components have been written to the stream.
//                    if (valueType.IsArray | valueType.IsGenericType)
//                        continue;

//                    // Set stream position to end of file
//                    binaryWriter.BaseStream.Seek(binaryWriter.BaseStream.Length, SeekOrigin.Begin);

//                    // Handle special case types
//                    //
//                    // STRING
//                    // BinaryWriter does not support strings. However we can pass in a char[].
//                    if (valueType == typeof(string))
//                        binaryWriter.Write(((string)value).ToCharArray());
//                    //
//                    // ENUMERATION
//                    // BitConverter doesn't support enums, but we cast them to uint (32 bit)
//                    else if (value is Enum)
//                    {
//                        // Get bytes of ENUM cast as uint
//                        byte[] data = BitConverter.GetBytes((uint)value);

//                        if (BitConverter.IsLittleEndian ^ isLittleEndian)
//                            Array.Reverse(data);

//                        binaryWriter.Write(data);
//                    }
//                    //
//                    // BYTE - SBYTE
//                    // If we let this fall into BitConverter, bytes/sbytes get casted into shorts!
//                    // Instead we can just write the byte/sbyte to stream as endianness doesn't
//                    // affect writing single bytes/sbytes.
//                    else if (valueType == typeof(byte) | valueType == typeof(sbyte))
//                        binaryWriter.Write(value);
//                    //
//                    // DECIMAL
//                    // BitConverter does not support decimals. We can get around this with by using
//                    // a BinaryWriter. The issue then is supporting endianness as a BinaryWriter will
//                    // write in Little-Endian by default. To support Big-Endian, we can write it to a
//                    // temporary stream, get the bytes from it and reverse them, then write the reversed
//                    // array to our target stream. Serializing this value isn't cheap (compared to others),
//                    // but decimals are less used than other types so we proceed regardless.
//                    else if (value is decimal)
//                    {
//                        // Get decimal into byte[]
//                        MemoryStream tempStream = new MemoryStream(16);
//                        BinaryWriter tempWriter = new BinaryWriter(tempStream);
//                        tempWriter.Write(value);
//                        byte[] data = tempStream.ToArray();

//                        // Reverse only if requesting Little-Endian
//                        if (BitConverter.IsLittleEndian ^ isLittleEndian)
//                            Array.Reverse(data);

//                        binaryWriter.Write(data);
//                        tempWriter.Dispose();
//                        tempStream.Dispose();
//                    }
//                    //
//                    // EVERYTHING ELSE
//                    // bool, char, short, ushort, int, uint, long, ulong, float, double
//                    else
//                    {
//                        try
//                        {
//                            // Get data as bytes. BitConverter will accept the remaining types
//                            // assuming they are not structs, classes, or interfaces. If so, it
//                            // will throw an error that will get caught.
//                            byte[] data = BitConverter.GetBytes(value);

//                            // Write in the endianness desired. We only want to reverse the
//                            // array when the BitConverter's endianness does not match the
//                            // desired endianness. Thus we XOR (^) the bools. When these two
//                            // endiannesses don't match, XOR returns true as it means we need
//                            // to correct the written endianness.
//                            // Want Big-Endian but BitConverter is Little-Endian? Reverse.
//                            // Want Little-Endian but BitConverter is Big-Endian? Reverse.
//                            if (BitConverter.IsLittleEndian ^ isLittleEndian)
//                                Array.Reverse(data);

//                            binaryWriter.Write(data);
//                        }
//                        catch
//                        {
//                            // Catch any type that got passed that is not a Value Type or String, such
//                            // as structs, classes, interfaces or other. Throw error with value name.
//                            throw new IOException(string.Format("The type {0} is not supported!", valueType));
//                        } // End of try/catch
//                    } // End of else (everything else)
//                } // End of foreach
//            } // Using BinaryReader
//            StreamWriteFinish_Callback?.Invoke();
//        } // End of method
//    }

//    /// <summary>
//    /// Extension methods for BinaryStreamUtility to use directly from a BinaryWriter
//    /// </summary>
//    public static class BinaryStreamUtilityExtensions
//    {
//        /// <summary>
//        /// Writes dynamicData to the end of reference BinaryWriter. If any part of dynamicData is an array
//        /// (multidimensional <c>[ , ]</c> or jagged <c>[ ][ ]</c>) or a list, including lists of the aforementioned types,
//        /// it will recursively break it down to it's most basic data component then serialize it in order presented in array/list.
//        /// </summary>
//        /// <param name="binaryWriter">The <c>BinaryReader</c> to write the <paramref name="dynamicData"/> to.</param>
//        /// <param name="fileName">The name of the file to open or create using the <c>BinaryWriter</c>.</param>
//        /// <param name "isLittleEndian">Writes the stream in Little-Endian? (This does not change the value in BinaryStreamUtility.)</param>
//        /// <param name="dynamicData">The data to write to the <c>BinaryWriter</c>.</param>
//        public static void WriteDynamicDataRecursively(this BinaryWriter binaryWriter, string fileName, bool isLittleEndian, params dynamic[] dynamicData)
//        {
//            // Record BinaryStreamUtility.IsLittleEndian's state
//            bool recordState = BinaryStreamUtility.IsLittleEndian;
//            BinaryStreamUtility.IsLittleEndian = isLittleEndian;

//            WriteDynamicDataRecursively(binaryWriter, fileName, dynamicData);

//            // Reapply state after method call
//            BinaryStreamUtility.IsLittleEndian = recordState;
//        }
//        /// <summary>
//        /// Writes dynamicData to the end of reference BinaryWriter. If any part of dynamicData is an array
//        /// (multidimensional <c>[ , ]</c> or jagged <c>[ ][ ]</c>) or a list, including lists of the aforementioned types,
//        /// it will recursively break it down to it's most basic data component then serialize it in order presented in array/list.
//        /// The Endianness is that of <BinaryStreamUtility.IsLittleEndian.
//        /// </summary>
//        /// <param name="writer">The <c>BinaryReader</c> to write the <paramref name="dynamicData"/> to.</param>
//        /// <param name="binaryWriter">The name of the file to open or create using the <c>BinaryWriter</c>.</param>
//        /// <param name="dynamicData">The data to write to the <c>BinaryWriter</c>.</param>
//        public static void WriteDynamicDataRecursively(this BinaryWriter writer, string binaryWriter, params dynamic[] dynamicData)
//        {
//            WriteDynamicDataRecursively(writer, binaryWriter, dynamicData);
//        }
//    }
//}