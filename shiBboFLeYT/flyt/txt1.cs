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
    class txt1
    {
        public int size;
        public pan1 panel;
        public short length, restrictLength, materialIndex, fontIndex;
        public byte unk1, unk2, activeShadows;
        public float unk3;
        public int strOffset, color1, color2;
        public float horizontalSize, verticalSize, charSize, lineSize;
        public int textBoxNameOffset;
        public float shadowX, shadowY, shadowHorizScale, shadowVertScale, shadowItalicTilt;
        public byte shadowTopR, shadowTopG, shadowTopB, shadowTopA;
        public byte shadowBottomR, shadowBottomG, shadowBottomB, shadowBottomA;

        // these next ones are not a part of the structure, but are defined so it makes things easier
        public string fontUsed;

        public static txt1 Parse(EndianBinaryReader reader)
        {
            int start = (int)reader.BaseStream.Position - 4;

            txt1 txt = new txt1();
            txt.size = reader.ReadInt32();
            txt.panel = pan1.Parse(reader, true);
            txt.length = reader.ReadInt16();
            txt.restrictLength = reader.ReadInt16();
            txt.materialIndex = reader.ReadInt16();
            txt.fontIndex = reader.ReadInt16();

            txt.fontUsed = fnl1.returnFontName(txt.fontIndex);

            txt.unk1 = reader.ReadByte();
            txt.unk2 = reader.ReadByte();
            txt.activeShadows = reader.ReadByte();

            reader.ReadByte();

            txt.unk3 = reader.ReadSingle();
            txt.strOffset = reader.ReadInt32();
            txt.color1 = reader.ReadInt32();
            txt.color2 = reader.ReadInt32();
            txt.horizontalSize = reader.ReadSingle();
            txt.verticalSize = reader.ReadSingle();
            txt.charSize = reader.ReadSingle();
            txt.lineSize = reader.ReadSingle();
            txt.textBoxNameOffset = reader.ReadInt32();
            txt.shadowX = reader.ReadSingle();
            txt.shadowY = reader.ReadSingle();
            txt.shadowHorizScale = reader.ReadSingle();
            txt.shadowVertScale = reader.ReadSingle();

            txt.shadowTopR = reader.ReadByte();
            txt.shadowTopG = reader.ReadByte();
            txt.shadowTopB = reader.ReadByte();
            txt.shadowTopA = reader.ReadByte();

            txt.shadowBottomR = reader.ReadByte();
            txt.shadowBottomG = reader.ReadByte();
            txt.shadowBottomB = reader.ReadByte();
            txt.shadowBottomA = reader.ReadByte();

            txt.shadowItalicTilt = reader.ReadSingle();

            // now we just need to figure out where we're at in the file 
            // it varies in length, so we need to see how much farther we have in the section
            // still have yet to figure out how properly see where the strings are truly located
            int whereWeAt = (int)reader.BaseStream.Position - start;

            int bytesToRead = txt.size - whereWeAt;

            reader.ReadBytes(bytesToRead);

            return txt;
        }

    }
}
