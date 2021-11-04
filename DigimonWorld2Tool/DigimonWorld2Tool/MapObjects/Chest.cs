using DigimonWorld2Tool.Domains;
using DigimonWorld2Tool.Interfaces;
using DigimonWorld2Tool.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DigimonWorld2Tool.MapObjects
{
    public class Chest : IFloorLayoutObject
    {
        private static readonly Dictionary<byte, string> TreasureItemsLookupTable = new Dictionary<byte, string>
        {
            {0x00, "Empty" },
            {0x01, "Wolf Eg-1"},
            {0x02, "Wolf Eg-2"},
            {0x03, "Wolf Eg-3"},
            {0x04, "Wolf Eg-4"},
            {0x05, "Wolf Eg-5"},
            {0x06, "Lion Eg-1"},
            {0x07, "Lion Eg-2"},
            {0x08, "Lion Eg-3"},
            {0x09, "Lion Eg-4"},
            {0x0A, "Lion Eg-5"},
            {0x0B, "Tiger Eg-1"},
            {0x0C, "Tiger Eg-2"},
            {0x0D, "Tiger Eg-3"},
            {0x0E, "Tiger Eg-4"},
            {0x0F, "Tiger Eg-5"},
            {0x10, "Pegasus Eg-1"},
            {0x11, "Pegasus Eg-2"},
            {0x12, "Pegasus Eg-3"},
            {0x13, "Pegasus Eg-4"},
            {0x14, "Pegasus Eg-5"},
            {0x15, "Griffin Eg-1"},
            {0x16, "Griffin Eg-2"},
            {0x17, "Griffin Eg-3"},
            {0x18, "Griffin Eg-4"},
            {0x19, "Griffin Eg-5"},
            {0x1A, "Wyvern Eg-1"},
            {0x1B, "Wyvern Eg-2"},
            {0x1C, "Wyvern Eg-3"},
            {0x1D, "Wyvern Eg-4"},
            {0x1E, "Wyvern Eg-5"},
            {0x1F, "Dragon Eg-1"},
            {0x20, "Dragon Eg-2"},
            {0x21, "Dragon Eg-3"},
            {0x22, "Dragon Eg-4"},
            {0x23, "Dragon Eg-5"},
            {0x24, "Giant Eg-1"},
            {0x25, "Giant Eg-2"},
            {0x26, "Giant Eg-3"},
            {0x27, "Giant Eg-4"},
            {0x28, "Giant Eg-5"},
            {0x29, "Mammoth Eg-1"},
            {0x2A, "Mammoth Eg-2"},
            {0x2B, "Mammoth Eg-3"},
            {0x2C, "Mammoth Eg-4"},
            {0x2D, "Mammoth Eg-5"},
            {0x2E, "Maximus EG"},
            {0x2F, "Ant Ram"},
            {0x30, "Wasp Ram"},
            {0x31, "Spider Ram"},
            {0x32, "Mantis Ram"},
            {0x33, "Hornet Ram"},
            {0x34, "Beetle Ram"},
            {0x35, "Crab Bat-1"},
            {0x36, "Crab Bat-2"},
            {0x37, "Crab Bat-3"},
            {0x38, "Crab Bat-4"},
            {0x39, "Crab Bat-5"},
            {0x3A, "Turtle Bat-1"},
            {0x3B, "Turtle Bat-2"},
            {0x3C, "Turtle Bat-3"},
            {0x3D, "Turtle Bat-4"},
            {0x3E, "Turtle Bat-5"},
            {0x3F, "Shark Bat-1"},
            {0x40, "Shark Bat-2"},
            {0x41, "Shark Bat-3"},
            {0x42, "Shark Bat-4"},
            {0x43, "Shark Bat-5"},
            {0x44, "Orca Bat-1"},
            {0x45, "Orca Bat-2"},
            {0x46, "Orca Bat-3"},
            {0x47, "Orca Bat-4"},
            {0x48, "Orca Bat-5"},
            {0x49, "Whale Bat"},
            {0x4A, "Dodo Box"},
            {0x4B, "Crow Box"},
            {0x4C, "Crane Box"},
            {0x4D, "Stork Box"},
            {0x4E, "Hawk Box"},
            {0x4F, "Eagle Box"},
            {0x50, "Ring Tires"},
            {0x51, "Chain Tires"},
            {0x52, "Plate Tires"},
            {0x53, "Aero Tires"},
            {0x54, "Gravi Tires"},
            {0x55, "Shovel Arm"},
            {0x56, "Drill Arm"},
            {0x57, "Jet Arm"},
            {0x58, "Laser Arm"},
            {0x59, "Magnum Arm"},
            {0x5A, "Mech Hand"},
            {0x5B, "Magnet Hand"},
            {0x5C, "Fantom Hand"},
            {0x5D, "Super Hand"},
            {0x5E, "Ultra Hand"},
            {0x5F, "Shooter Gun"},
            {0x60, "Z Cannon-1"},
            {0x61, "Z Cannon-2"},
            {0x62, "Z Cannon-3"},
            {0x63, "R Cannon-1"},
            {0x64, "R Cannon-2"},
            {0x65, "R Cannon-3"},
            {0x66, "Missle Gun"},
            {0x67, "Bug Zapper"},
            {0x68, "Mine Sweep1"},
            {0x69, "Mine Sweep2"},
            {0x6A, "Mine Sweep3"},
            {0x6B, "Mine Sweep4"},
            {0x6C, "Mine Sweep5"},
            {0x6D, "Bug Sweep1"},
            {0x6E, "Bug Sweep2"},
            {0x6F, "Bug Sweep3"},
            {0x70, "Bug Swp-4"},
            {0x71, "Bug Swp-5"},
            {0x72, "DM Transfer"},
            {0x73, "Auto Pilot"},
            {0x74, "Power Pilot"},
            {0x75, "Radar"},
            {0x76, "Power Radar"},
            {0x77, "Map Radar"},
            {0x78, "HP Disk-1"},
            {0x79, "HP Disk-2"},
            {0x7A, "HP Disk-3"},
            {0x7B, "MP Disk-1"},
            {0x7C, "MP Disk-2"},
            {0x7D, "MP Disk-3"},
            {0x7E, "Anti-Dote"},
            {0x7F, "Anti-Freeze"},
            {0x80, "Anti-Mixup"},
            {0x81, "Power Disk"},
            {0x82, "Mech Fix"},
            {0x83, "Mech Fix-EX"},
            {0x84, "Parts Fix"},
            {0x85, "EP Pack-1"},
            {0x86, "EP Pack-2"},
            {0x87, "EP Pack-3"},
            {0x88, "Mag.Miss-1"},
            {0x89, "Mag.Miss-2"},
            {0x8A, "Mag.Miss-3"},
            {0x8B, "Mag.Miss-4"},
            {0x8C, "Mag.Miss-5"},
            {0x8D, "DrillMiss1"},
            {0x8E, "DrillMiss2"},
            {0x8F, "DrillMiss3"},
            {0x90, "DrillMiss4"},
            {0x91, "DrillMiss5"},
            {0x92, "WaveMiss-1"},
            {0x93, "WaveMiss-2"},
            {0x94, "WaveMiss-3"},
            {0x95, "WaveMiss-4"},
            {0x96, "WaveMiss-5"},
            {0x97, "BitBugZap1"},
            {0x98, "BitBugZap2"},
            {0x99, "BitBugZap3"},
            {0x9A, "EP-BugZap1"},
            {0x9B, "EP-BugZap2"},
            {0x9C, "EP-BugZap3"},
            {0x9D, "RetBugZap1"},
            {0x9E, "RetBugZap2"},
            {0x9F, "RetBugZap3"},
            {0xA0, "MemBugZap1"},
            {0xA1, "MemBugZap2"},
            {0xA2, "MemBugZap3"},
            {0xA3, "SupBugZap1"},
            {0xA4, "SupBugZap2"},
            {0xA5, "SupBugZap3"},
            {0xA6, "Fire Blast"},
            {0xA7, "WaterArrow"},
            {0xA8, "Flash Bolt"},
            {0xA9, "Iron Fist"},
            {0xAA, "Dark Fear"},
            {0xAB, "Inferno"},
            {0xAC, "Blizzard"},
            {0xAD, "Hurricane"},
            {0xAE, "RustStorm"},
            {0xAF, "Black Hole"},
            {0xB0, "Data Steel"},
            {0xB1, "Data Candy"},
            {0xB2, "Data Macho"},
            {0xB3, "Data Weak"},
            {0xB4, "Vac. Steel"},
            {0xB5, "Vac. Candy"},
            {0xB6, "Vac. Macho"},
            {0xB7, "Vac. Weak"},
            {0xB8, "Vir. Steel"},
            {0xB9, "Vir. Candy"},
            {0xBA, "Vir. Macho"},
            {0xBB, "Vir. Weak"},
            {0xBC, "Toy Car"},
            {0xBD, "Toy Truck"},
            {0xBE, "Toy Tank"},
            {0xBF, "Toy Boat"},
            {0xC0, "Toy Plane"},
            {0xC1, "Card Game"},
            {0xC2, "Digivice"},
            {0xC3, "CD Game"},
            {0xC4, "DVD Game"},
            {0xC5, "Laptop"},
            {0xC6, "WristWatch"},
            {0xC7, "CD Player"},
            {0xC8, "Cell Phone"},
            {0xC9, "DigiCamera"},
            {0xCA, "DVD Player"},
            {0xCB, "Kickboard"},
            {0xCC, "Skateboard"},
            {0xCD, "Skies"},
            {0xCE, "Snowboard"},
            {0xCF, "Surfboard"},
            {0xD0, "HP Chip"},
            {0xD1, "Junk Parts"},
            {0xD2, "MP Chip"},
            {0xD3, "Power Chip"},
            {0xD4, "Armor Chip"},
            {0xD5, "Speed Chip"},
            {0xD6, "DNA-UpChip"},
            {0xD7, "DNA-DnChip"},
            {0xD8, "EXP Chip"},
            {0xD9, "HP Driver-1"},
            {0xDA, "HP Driver-2"},
            {0xDB, "HP Driver-3"},
            {0xDC, "MP Driver-1"},
            {0xDD, "MP Driver-2"},
            {0xDE, "MP Driver-3"},
            {0xDF, "EX Driver"},
            {0xE0, "Max Driver"},
            {0xE1, "Data-HPROM"},
            {0xE2, "Data-MPROM"},
            {0xE3, "Data-RVROM"},
            {0xE4, "Vac-HP-ROM"},
            {0xE5, "Vac-MP-ROM"},
            {0xE6, "Vac-RV-ROM"},
            {0xE7, "Vir-HP-ROM"},
            {0xE8, "Vir-MP-ROM"},
            {0xE9, "Vir-RV-ROM"},
            {0xEA, "Steel Body"},
            {0xEB, "Titan Body"},
            {0xEC, "AdmantBody"},
            {0xED, "Tamer-Lic."},
            {0xEE, "DB-Browser"},
            {0xEF, "GoldH-Mark"},
            {0xF0, "BlueF-Mark"},
            {0xF1, "BlackSMark"},
            {0xF2, "DynamoPart"},
            {0xF3, "Entry Pass"},
            {0xF4, "Converter"},
            {0xF5, "Titan Core"},
            {0xF6, "Red Orders"},
            {0xF7, "Message-1"},
            {0xF8, "Vac. Patch"},
            {0xF9, "Data Patch"},
            {0xFA, "VirusPatch"},
            {0xFB, "Wild Code"},
            {0xFC, "Gyro Radar"},
            {0xFD, "Message-3"},
            {0xFE, "X-MechPart"},
            {0xFF, "Message-2"},
        };

        public IFloorLayoutObject.MapObjectType ObjectType => IFloorLayoutObject.MapObjectType.Chest;
        public Vector2 Position { get; private set; }
        public Color ObjectColour => Color.FromArgb(255, 0, 255, 0);
        public string ObjectText { get; } = "T";

        public readonly ChestSlot[] chestSlots = new ChestSlot[4];

        public Chest(byte[] data)
        {
            this.Position = new Vector2(data[0], data[1]);

            chestSlots[0] = new ChestSlot(data[2].GetLeftHalfByte());
            chestSlots[1] = new ChestSlot(data[2].GetRightHalfByte());
            chestSlots[2] = new ChestSlot(data[3].GetLeftHalfByte());
            chestSlots[3] = new ChestSlot(data[3].GetRightHalfByte());
        }

        public override string ToString()
        {
            return $"\nObject \"{ObjectType}\" at position {Position}";
        }

        public class ChestSlot
        {
            public byte ItemID;
            public string ItemName;
            public byte TrapLevel;

            public ChestSlot(byte index)
            {
                if(index == 0)
                {
                    ItemName = "No Chest";
                    TrapLevel = 0;
                    return;
                }

                ItemID = DomainFloor.CurrentDomainFloor.PossibleTreasure[index - 1].ItemID;
                if (Chest.TreasureItemsLookupTable.TryGetValue(ItemID, out string value))
                {
                    ItemName = value;
                }
                else
                {
                    ItemName = "Error";
                }
                TrapLevel = DomainFloor.CurrentDomainFloor.PossibleTreasure[index - 1].TrapLevel;
            }
        }
    }
}
