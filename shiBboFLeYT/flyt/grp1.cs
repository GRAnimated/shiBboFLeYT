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
    class grp1
    {
        public int size;
        public string groupName;
        public int numEntries;
        public List<string> embeds;

        public static grp1 Parse(EndianBinaryReader reader, bool hasEmbeds)
        {

            grp1 grp = new grp1();
            grp.size = reader.ReadInt32();

            grp.groupName = Encoding.ASCII.GetString(reader.ReadBytes(0x18));

            if (hasEmbeds)
            {

                List<string> embed = new List<string>();

                grp.numEntries = reader.ReadInt16();

                reader.ReadBytes(2);

                for (int i = 0; i < grp.numEntries; i++)
                {
                    string embedStr = Encoding.ASCII.GetString(reader.ReadBytes(0x18));
                }
            }
            else
            {
                grp.numEntries = 0;
                grp.embeds = null;
                reader.ReadBytes(4);
            }

            return grp;
        }
    }
}
