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
    // LYT1
    // defines stuff like sizes, and drawing modes
    class lyt1
    {
        uint magic, size;
        byte drawMiddle;
        float layoutHeight, layoutWidth, maxPartWidth, maxPartHeight;
        string layoutName;

        public static lyt1 Parse(EndianBinaryReader reader)
        {
            long position = reader.BaseStream.Position; // should be 0x14 to be honest

            lyt1 lyt = new lyt1();

            lyt.magic = reader.ReadUInt32();

            if (lyt.magic != 0x6C797431) // "lyt1"
            {
                Console.WriteLine("Invalid LYT1 magic.");
                return null;
            }

            lyt.size = reader.ReadUInt32();

            uint stringLength =  lyt.size - (uint)position - 8; // this is the size before trimming the null-terminators

            lyt.drawMiddle = reader.ReadByte();
            reader.ReadBytes(3); // most likely padding
            lyt.layoutWidth = reader.ReadSingle();
            lyt.layoutHeight = reader.ReadSingle();
            lyt.maxPartHeight = reader.ReadSingle();
            lyt.maxPartWidth = reader.ReadSingle();

            // why does a arg of a ReadBytes() need a int? < 0 bytes?
            // Replace() removed null terminators
            lyt.layoutName = Encoding.ASCII.GetString(reader.ReadBytes((int)stringLength)).Replace(" ", "");

            return lyt;
        }
    }
}
