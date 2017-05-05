/*
    Copyright 2017 shibboleet
    This file is part of shiBboFLeYT.
    shiBboFLeYT is free software: you can redistribute it and/or modify it under
    the terms of the GNU General Public License as published by the Free
    Software Foundation, either version 3 of the License, or (at your option)
    any later version.
    shiBboFLeYT is distributed in the hope that it will be useful, but WITHOUT ANY
    WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
    FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
    You should have received a copy of the GNU General Public License along
    with shiBboFLeYT. If not, see http://www.gnu.org/licenses/.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace shiBboFLeYT.flyt
{
    class txl1
    {
        public int size;
        public short numFiles;
        public List<uint> strOffsets;
        public List<string> filesNeeded;

        public static txl1 Parse(EndianBinaryReader reader)
        {

            uint start = (uint)reader.BaseStream.Position;

            txl1 txl = new txl1();
            txl.size = reader.ReadInt32();
            txl.numFiles = reader.ReadInt16();

            uint baseOffset = (uint)reader.BaseStream.Position;

            reader.ReadBytes(2); // possible padding

            List<uint> offsets = new List<uint>();

            for (int i = 0; i < txl.numFiles; i++)
            {
                uint offset = reader.ReadUInt32();

                offsets.Add(offset);
            }

            txl.strOffsets = offsets;

            List<string> strings = new List<string>();

            foreach (uint offset in offsets)
            {
                string fileName = "";
                uint offsetToRead = offset + baseOffset;
                reader.BaseStream.Position = offsetToRead + 2;

                while(true)
                {
                    byte str = reader.ReadByte();

                    if (str != 0x00)
                    {
                        fileName += str.ToString("X");
                    }
                    else
                    {
                        string newFileName = HexStringToString(fileName).Replace(" ", "");
                        strings.Add(newFileName);
                        break;
                    }
                }

                txl.filesNeeded = strings;
            }

            int butts = (int)reader.BaseStream.Position - (int)start;

            int remainingNulls = txl.size - butts - 4;

            reader.ReadBytes(remainingNulls);

            return txl;
            
        }

        public static string HexStringToString(string hexString)
        {
            if (hexString == null || (hexString.Length & 1) == 1)
            {
                throw new ArgumentException();
            }
            var sb = new StringBuilder();
            for (var i = 0; i < hexString.Length; i += 2)
            {
                var hexChar = hexString.Substring(i, 2);
                sb.Append((char)Convert.ToByte(hexChar, 16));
            }
            return sb.ToString();
        }
    }
}
