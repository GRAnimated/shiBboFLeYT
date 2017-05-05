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
    class pan1
    {
        public int size;
        public int visibility, transmitAlpha;
        public byte alpha, partScale;
        public int parentY, parentX, originY, originX;
        public string paneName;
        byte[] userData;
        float x, y, z, xRot, yRot, zRot, horizScale, vertScale, width, height;

        public static pan1 Parse(EndianBinaryReader reader, bool isEmbed)
        {
            pan1 pan = new pan1();

            if (!isEmbed)
                pan.size = reader.ReadInt32();

            byte start = reader.ReadByte();

            pan.visibility = start & 0x0F;
            pan.transmitAlpha = start >> 4;

            byte originByte = reader.ReadByte();

            pan.parentY = (originByte & 0x3);
            pan.parentX = ((originByte >> 2) & 0x3);
            pan.originY = ((originByte >> 4) & 0x3);
            pan.originX = ((originByte >> 6) & 0x3);

            pan.alpha = reader.ReadByte();
            pan.partScale = reader.ReadByte();
            pan.paneName = Encoding.ASCII.GetString(reader.ReadBytes(0x18)).Replace(" ", "");
            pan.userData = reader.ReadBytes(8);
            pan.x = reader.ReadSingle();
            pan.y = reader.ReadSingle();
            pan.z = reader.ReadSingle();
            pan.xRot = reader.ReadSingle();
            pan.yRot = reader.ReadSingle();
            pan.zRot = reader.ReadSingle();
            pan.horizScale = reader.ReadSingle();
            pan.vertScale = reader.ReadSingle();
            pan.width = reader.ReadSingle();
            pan.height = reader.ReadSingle();

            return pan;

        }
    }
}
