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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using shiBboFLeYT.flyt;

namespace shiBboFLeYT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                doShit(dialog.FileName);
            }
        }

        public void doShit(string path)
        {
            // first we parse the FLYT and LYT1 sections because they're always in this order
            using (EndianBinaryReader reader = new EndianBinaryReader(File.Open(path, FileMode.Open)))
            {
                Console.WriteLine("Parsing FLYT header...");
                FLYT flyt = FLYT.Parse(reader);

                Console.WriteLine("Parsing LYT1 section...");
                lyt1 lyt = lyt1.Parse(reader);

                string prevMagic = "";

                for (int i = 1; i < flyt.numSections; i++)
                {
                    string magic = Encoding.ASCII.GetString(reader.ReadBytes(4));
                    int size;

                    switch (magic)
                    {
                        case "txl1":
                            Console.WriteLine("TXL1 found");
                            txl1.Parse(reader);
                            break;
                        case "fnl1":
                            Console.WriteLine("FNL1 found");
                            fnl1.Parse(reader);
                            break;
                        case "grp1":
                            Console.WriteLine("GRP1 found");
                            if (prevMagic == "grs1" || prevMagic == "grp1")
                                grp1.Parse(reader, true);
                            else
                                grp1.Parse(reader, false);
                            break;
                        case "mat1":
                            Console.WriteLine("MAT1 found");
                            mat1.Parse(reader);
                            break;
                        case "pan1":
                            Console.WriteLine("PAN1 found");
                            pan1.Parse(reader, false);
                            break;
                        case "pas1":
                            Console.WriteLine("PAS1 found");
                            size = reader.ReadInt32();
                            reader.ReadBytes(size - 8);
                            break;
                        case "pic1":
                            Console.WriteLine("PIC1 found");
                            size = reader.ReadInt32();
                            reader.ReadBytes(size - 8);
                            break;
                        case "txt1":
                            Console.WriteLine("TXT1 found");
                            txt1.Parse(reader);
                            break;
                        case "pae1":
                            Console.WriteLine("PAE1 found");
                            reader.ReadBytes(4);
                            break;
                        case "grs1":
                            Console.WriteLine("GRS1 found");
                            reader.ReadBytes(4);
                            break;
                        case "prt1":
                            Console.WriteLine("PRT1 found");
                            size = reader.ReadInt32();
                            reader.ReadBytes(size - 8);
                            break;
                        case "wnd1":
                            Console.WriteLine("WND1 found");
                            size = reader.ReadInt32();
                            reader.ReadBytes(size - 8);
                            break;
                        case "gre1":
                            Console.WriteLine("GRE1 found");
                            reader.ReadBytes(4);
                            break;
                    }

                    prevMagic = magic;
                }

            }
        }
    }
}
