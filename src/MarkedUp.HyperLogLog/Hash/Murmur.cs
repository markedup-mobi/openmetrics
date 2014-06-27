/**
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements. See the NOTICE file distributed with this
 * work for additional information regarding copyright ownership. The ASF
 * licenses this file to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */

namespace MarkedUp.HyperLogLog.Hash
{
    /// <summary>
    /// A Murmur3 implementation in .NET that doesn't suck.
    /// 
    /// This is a C# port of the cannonical algorithm in C++, with some helper functions
    /// designed to make it easier to work with POCOs and .NET primitives.
    /// </summary>
    public static class MurmurHash
    {

        #region Constants for X86 / X64 implementations

        private const uint X86_32_C1 = 0xcc9e2d51;

        private const uint X86_32_C2 = 0x1b873593;

        private const ulong X64_128_C1 = 0x87c37b91114253d5L;

        private const ulong X64_128_C2 = 0x4cf5ad432745937fL;

        #endregion

        /// <summary>
        /// Compute a 32-bit Murmur3 hash for an X86 system.
        /// </summary>
        /// <param name="data">The data that needs to be hashed</param>
        /// <param name="length">The length of the data being hashed</param>
        /// <param name="seed">A seed value used to compute the hash</param>
        /// <returns>A computed hash value, as a signed long integer.</returns>
        public static uint Hash_X86_32(byte[] data, int length, long seed)
        {

            var nblocks = length >> 2;
            var h1 = (uint)seed;
            uint k1 = 0;

            for (var i = 0; i < nblocks; i++)
            {
                var i4 = i << 2;
                k1 = GetBlock32(data, i4);
                k1 *= X86_32_C1;
                k1 = RotateLeft32(k1, 15);
                k1 *= X86_32_C2;

                h1 ^= k1;
                h1 = RotateLeft32(h1, 13);
                h1 = h1 + (5 + 0xe6546b64);
            }

            //tail - there's an unprocessed tail of data that we need to hash
            var offset = (nblocks << 2); // nblocks * 2

            switch (length & 3)
            {
                case 3:
                    k1 ^= ((uint)data[offset + 2] << 16);
                    goto case 2; //thanks for the code smell, C#!
                case 2:
                    k1 ^= ((uint)data[offset + 1] << 8);
                    goto case 1;
                case 1:
                    k1 ^= (data[offset]);
                    k1 *= X86_32_C1;
                    k1 = RotateLeft32(k1, 15);
                    k1 *= X86_32_C2;
                    h1 ^= k1;
                    break;
            }

            //finalization
            h1 ^= (uint)length;
            h1 = ForceMix32(h1);
            return h1;
        }

        #region Internal hash functions

        /// <summary>
        /// Read the next 4-byte block (int32) from a block number
        /// </summary>
        /// <param name="blocks">the original byte array</param>
        /// <param name="i">the current block count</param>
        /// <returns>An unsinged 32-bit integer</returns>
        private static uint GetBlock32(byte[] blocks, int i)
        {
            uint k1 = blocks[i];
            k1 |= (uint)blocks[i + 1] << 8;
            k1 |= (uint)blocks[i + 2] << 16;
            k1 |= (uint)blocks[i + 3] << 24;
            return k1;
        }

        /// <summary>
        /// Rotate a 32-bit unsigned integer to the left by <see cref="shift"/> bits
        /// </summary>
        /// <param name="original">Original value</param>
        /// <param name="shift">The shift value</param>
        /// <returns>The rotated 32-bit integer</returns>
        private static uint RotateLeft32(uint original, int shift)
        {
            return (original << shift) | (original >> (32 - shift));
        }

        /// <summary>
        /// Finalization mix - force all bits of a hash block to avalanche.
        /// 
        /// I have no idea what that means but it sound awesome.
        /// </summary>
        private static uint ForceMix32(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;

            return h;
        }

        /// <summary>
        /// Finalization mix - force all bits of a hash block to avalanche.
        /// 
        /// I have no idea what that means but it sound awesome.
        /// </summary>
        private static ulong ForceMix64(ulong k)
        {
            k ^= k >> 33;
            k *= 0xff51afd7ed558ccd;
            k ^= k >> 33;
            k *= 0xc4ceb9fe1a85ec53;
            k ^= k >> 33;

            return k;
        }

        /// <summary>
        /// Rotate a 64-bit unsigned integer to the left by <see cref="shift"/> bits
        /// </summary>
        /// <param name="original">Original value</param>
        /// <param name="shift">The shift value</param>
        /// <returns>The rotated 64-bit integer</returns>
        private static ulong RotateLeft64(ulong original, int shift)
        {
            return (original << shift) | (original >> (64 - shift));
        }

        #endregion
    }


}
