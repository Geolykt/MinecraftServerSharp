﻿using System;
using MinecraftServerSharp.DataTypes;

namespace Tests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TestVarInt32();
            Console.WriteLine(nameof(TestVarInt32) + " passed");
        }

        #region TestVarInt32

        private static void TestVarInt32()
        {
            TestVarInt32(0, 0);
            TestVarInt32(1, 1);
            TestVarInt32(2, 2);
            TestVarInt32(127, 127);
            TestVarInt32(128, 128, 1);
            TestVarInt32(255, 255, 1);
            TestVarInt32(2147483647, 255, 255, 255, 255, 7);
            TestVarInt32(-1, 255, 255, 255, 255, 15);
            TestVarInt32(-2147483648, 128, 128, 128, 128, 8);
        }

        private static void TestVarInt32(int decimalValue, params byte[] bytes)
        {
            Span<byte> tmp = stackalloc byte[VarInt32.MaxEncodedSize];
            int len = new VarInt32(decimalValue).Encode(tmp);
            if (!tmp.Slice(0, len).SequenceEqual(bytes))
                throw new Exception();
        }

        #endregion
    }
}
