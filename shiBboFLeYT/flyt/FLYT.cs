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
    class FLYT
    {
        int magic;
        short endianness, sectionOffs;
        int versionNum, fileSize;
        public short numSections;

        public static FLYT Parse(EndianBinaryReader reader)
        {
            FLYT flyt = new FLYT();
            flyt.magic = reader.ReadInt32();

            if (flyt.magic != 0x464C5954) // "FLYT"
            {
                Console.WriteLine("Invalid magic, breaking.");
                return null;
            }

            flyt.endianness = reader.ReadInt16(); // should be 0xFFEF for BE (Big Endian)
            flyt.sectionOffs = reader.ReadInt16(); // First section offset
            flyt.versionNum = reader.ReadInt32(); // 0x02020000 in NSMBU, 0x05020000 in Wolly World

            if (flyt.versionNum == 0x02020000)
                Console.WriteLine("NSMBU FLYT Detected");
            else if (flyt.versionNum == 0x05020000)
                Console.WriteLine("Splatoon / Wolly World FLYT Detected");

            flyt.fileSize = reader.ReadInt32(); // entire filesize
            flyt.numSections = reader.ReadInt16(); // number of sections to read

            reader.ReadBytes(2); // padding

            return flyt;

        }
    }
}
