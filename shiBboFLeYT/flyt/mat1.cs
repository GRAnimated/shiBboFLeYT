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
    class mat1
    {
        public int size;
        public short entryCount;

        public static mat1 Parse(EndianBinaryReader reader)
        {
            int start = (int)reader.BaseStream.Position - 4;

            var mat = new mat1();
            mat.size = reader.ReadInt32();
            mat.entryCount = reader.ReadInt16();

            reader.ReadBytes(2);

            List<int> entryOffsets = new List<int>();

            for (int i = 0; i < mat.entryCount; i++)
            {
                int entry = reader.ReadInt32();
                entryOffsets.Add(entry);
            }

            reader.ReadBytes(3);

            List<mat1Entry> material1Entries = new List<mat1Entry>();

            int next = 0;

            for (int i = 0; i < entryOffsets.Count; i++)
            {
                reader.BaseStream.Position = start + entryOffsets[i];

                try
                {
                    next = entryOffsets[i + 1];
                }
                catch
                {
                    next = mat.size - entryOffsets[i];
                }

                mat1Entry matEntry;

                matEntry = mat1Entry.Parse(reader, next);

                material1Entries.Add(matEntry);
            }

            return mat;
        }
    }

    class mat1Entry
    {
        public string name;
        public short unk1, unk2, unk3;
        public int unk4, unk5, unk6, flags;

        public static mat1Entry Parse(EndianBinaryReader reader, int nextOffset)
        {
            int bytesToRead = 0;

            var matEntry = new mat1Entry();
            matEntry.name = Encoding.ASCII.GetString(reader.ReadBytes(0x14));
            matEntry.unk1 = reader.ReadInt16();
            matEntry.unk2 = reader.ReadInt16();
            matEntry.unk3 = reader.ReadInt16();

            matEntry.unk4 = reader.ReadInt32();
            matEntry.unk5 = reader.ReadInt32();
            matEntry.unk6 = reader.ReadInt32();

            matEntry.flags = reader.ReadInt32();

            bytesToRead = nextOffset - 0x2A;

            reader.ReadBytes(bytesToRead);

            return matEntry;
        }
    }
}
