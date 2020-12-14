using DigimonWorld2MapVisualizer.Interfaces;
using DigimonWorld2MapVisualizer.Utility;
using DigimonWorld2MapVisualizer.Domains;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using DigimonWorld2Tool;

namespace DigimonWorld2MapVisualizer.MapObjects
{
    public class Digimon : IFloorLayoutObject
    {
        public IFloorLayoutObject.MapObjectType ObjectType { get; private set; }
        public Vector2 Position { get; private set; }
        public readonly DigimonPack[] DigimonPacks = new DigimonPack[4];

        public Digimon(IFloorLayoutObject.MapObjectType objectType, byte[] data)
        {
            this.ObjectType = objectType;
            this.Position = new Vector2(data[0], data[1]);

            // The last 2 bytes of data contains the data for the digimon packs, 1 byte each.
            //for (int i = 0; i < 2; i++)
            //{
            //    DigimonPacks[i] = new DigimonPack(data[i + 2]);
            //}

            DigimonPacks[0] = new DigimonPack(data[2].GetLeftHalfByte(), Position);
            DigimonPacks[1] = new DigimonPack(data[2].GetRightHalfByte(), Position);
            DigimonPacks[2] = new DigimonPack(data[3].GetLeftHalfByte(), Position);
            DigimonPacks[3] = new DigimonPack(data[3].GetRightHalfByte(), Position);
        }

        public override string ToString()
        {
            return $"\nObject \"{ObjectType}\" at position {Position}, First ID 0x{DigimonPacks[0].EncounterID:X2}, Second ID 0x{DigimonPacks[1].EncounterID:X2} ";
        }

        public class DigimonPack
        {
            public enum DigimonPackLevel : byte
            {
                Rookie = 0,
                Champion = 1,
                Ultimate = 2,
                Mega = 4,
                Special = 5,
                None = 10,
                Error = 99,
            }

            private readonly DigimonOverworldPack[] DigimonModelLookupTable = new DigimonOverworldPack[]
            {
                new DigimonOverworldPack(0x2C, "Candlemon", DigimonPackLevel.Rookie),
                new DigimonOverworldPack(0x2D, "Patamon", DigimonPackLevel.Rookie),
                new DigimonOverworldPack(0x2E, "Gabumon", DigimonPackLevel.Rookie),
                new DigimonOverworldPack(0x2F, "Crabmon", DigimonPackLevel.Rookie),
                new DigimonOverworldPack(0x30, "Tankmon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x31, "Yanmamon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x32, "Kokatorimon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x33, "Centaurmon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x34, "Drimogemon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x35, "Tyrannomon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x36, "Seadramon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x37, "SkullMeramon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x38, "Pumpkinmon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x39, "Piximon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x3A, "Mamemon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x3b, "Triceramon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x3c, "MegaSeadramon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x3d, "Boltmon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x3e, "PrinceMamemon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x3f, "MetalSeadramon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x40, "Biyomon", DigimonPackLevel.Rookie),
                new DigimonOverworldPack(0x41, "Toyagumon", DigimonPackLevel.Rookie),
                new DigimonOverworldPack(0x42, "Tapirmon", DigimonPackLevel.Rookie),
                new DigimonOverworldPack(0x43, "Penguinmon", DigimonPackLevel.Rookie),
                new DigimonOverworldPack(0x44, "Airdramon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x45, "Birdramon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x46, "Leomon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x47, "Angemon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x48, "Unimon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x49, "Frigimon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x4A, "Greymon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x4b, "MegaKabuterimon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x4C, "Mammothmon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x4d, "Giromon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x4e, "Andromon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x4F, "Metalgreymon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x50, "Zudomon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x51, "SkullMammothmon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x52, "Wargreymon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x53, "MarineAngemon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x54, "Hagurumon", DigimonPackLevel.Rookie),
                new DigimonOverworldPack(0x55, "Gazimon", DigimonPackLevel.Rookie),
                new DigimonOverworldPack(0x56, "SnowGoburimon", DigimonPackLevel.Rookie),
                new DigimonOverworldPack(0x57, "Betamon", DigimonPackLevel.Rookie),
                new DigimonOverworldPack(0x58, "Numemon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x59, "Kuwagamon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x5A, "Woodmon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x5b, "Bakemon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x5C, "Devimon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x5D, "Deltamon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x5E, "Gekomon", DigimonPackLevel.Champion),
                new DigimonOverworldPack(0x5f, "Vademon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x60, "Okuwamon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x61, "Cherrymon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x62, "Phantomon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x63, "Skullgreymon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x64, "MarineDevimon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x65, "Puppetmon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x66, "Piedmon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x67, "Pukumon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x68, "HerculesKabuterimon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x69, "Overlord", DigimonPackLevel.Special),
                new DigimonOverworldPack(0x6a, "VaccineGuardian", DigimonPackLevel.Special),
                new DigimonOverworldPack(0x6b, "DataGuardian", DigimonPackLevel.Special),
                new DigimonOverworldPack(0x6c, "VirusGuardian", DigimonPackLevel.Special),
                new DigimonOverworldPack(0x6D, "Patamon", DigimonPackLevel.Special),
                new DigimonOverworldPack(0x6e, "Chaoslord", DigimonPackLevel.Special),
                new DigimonOverworldPack(0x6f, "NeoCrimson", DigimonPackLevel.Special),
                new DigimonOverworldPack(0x70, "ChaosWargreymon", DigimonPackLevel.Special),
                new DigimonOverworldPack(0x71, "ChaosMetalSeadramon", DigimonPackLevel.Special),
                new DigimonOverworldPack(0x72, "ChaosPiedmon", DigimonPackLevel.Special),
                new DigimonOverworldPack(0x73, "Omnimon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x74, "Diaboromon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x75, "Baihumon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0x76, "MasterTyrannomon", DigimonPackLevel.Ultimate),
                new DigimonOverworldPack(0x77, "Jijimon", DigimonPackLevel.Mega),
                new DigimonOverworldPack(0xFF, "Custom", DigimonPackLevel.Special),
            };

            private readonly Dictionary<byte, byte> PackIDToModelIDLookupTable = new Dictionary<byte, byte>()
            {
                {0x01, 0x2E},{0x02, 0x2F},{0x03, 0x2D},{0x04, 0x2F},{0x05, 0x2E},{0x06, 0x2C},{0x07, 0x2C},
                {0x08, 0x34},{0x09, 0x31},{0x0A, 0x32},{0x0B, 0x32},{0x0C, 0x33},{0x0D, 0x33},{0x0E, 0x35},
                {0x0F, 0x35},{0x10, 0x30},{0x11, 0x36},{0x12, 0x39},{0x13, 0x38},{0x14, 0x3A},{0x15, 0x3A},
                {0x16, 0x3B},{0x17, 0x3B},{0x18, 0x3C},{0x19, 0x37},{0x1A, 0x3F},{0x1B, 0x3D},{0x1C, 0x3F},
                {0x1D, 0x3D},{0x1E, 0x3E},{0x1F, 0x42},{0x20, 0x40},{0x21, 0x41},{0x22, 0x42},{0x23, 0x40},
                {0x24, 0x43},{0x25, 0x41},{0x26, 0x48},{0x27, 0x47},{0x28, 0x45},{0x29, 0x45},{0x2A, 0x4A},
                {0x2B, 0x4A},{0x2C, 0x49},{0x2D, 0x49},{0x2E, 0x46},{0x2F, 0x44},{0x30, 0x4E},{0x31, 0x4F},
                {0x32, 0x4D},{0x33, 0x4D},{0x34, 0x4B},{0x35, 0x4B},{0x36, 0x50},{0x37, 0x4C},{0x38, 0x51},
                {0x39, 0x52},{0x3A, 0x51},{0x3B, 0x52},{0x3C, 0x53},{0x3D, 0x56},{0x3E, 0x57},{0x3F, 0x55},
                {0x40, 0x56},{0x41, 0x54},{0x42, 0x55},{0x43, 0x57},{0x44, 0x5A},{0x45, 0x58},{0x46, 0x58},
                {0x47, 0x5E},{0x48, 0x59},{0x49, 0x59},{0x4A, 0x5C},{0x4B, 0x5C},{0x4C, 0x5B},{0x4D, 0x5D},
                {0x4E, 0x5F},{0x4F, 0x63},{0x50, 0x63},{0x51, 0x62},{0x52, 0x60},{0x53, 0x60},{0x54, 0x64},
                {0x55, 0x61},{0x56, 0x67},{0x57, 0x67},{0x58, 0x65},{0x59, 0x65},{0x5A, 0x66},{0x5B, 0x54},
                {0x5C, 0x32},{0x5D, 0x58},{0x5E, 0x33},{0x5F, 0x45},{0x60, 0x59},{0x61, 0x4A},{0x62, 0x0F},
                {0x63, 0x08},{0x64, 0x08},{0x65, 0x10},{0x66, 0x13},{0x67, 0x08},{0x68, 0x08},{0x69, 0x08},
                {0x6A, 0x08},{0x6B, 0x13},{0x6C, 0x13},{0x6D, 0x12},{0x6E, 0x08},{0x71, 0x14},{0x72, 0x14},
                {0x73, 0x14},{0x74, 0x14},{0x75, 0x10},{0x76, 0x11},{0x77, 0x0A},{0x78, 0x0C},{0x79, 0x0E},
                {0x7A, 0x5E},{0x7B, 0x72},{0x7C, 0x71},{0x7D, 0x70},{0x81, 0x12},{0x82, 0x6E},{0x83, 0x14},
                {0x84, 0x14},{0x85, 0x14},{0x88, 0x13},{0x89, 0x14},{0x8A, 0x14},{0x8C, 0x13},{0x90, 0x12},
                {0x91, 0x75},{0x92, 0x6B},{0x93, 0x73},{0x94, 0x6A},{0x95, 0x74},{0x96, 0x6C},{0x97, 0x69},
                {0x98, 0x69},{0x99, 0x0C},{0x9A, 0x55},{0x9B, 0x46},{0x9C, 0x0C},{0x9D, 0x76},{0x9E, 0x00},
                {0x9F, 0x00},{0xA0, 0x00},{0xA1, 0x00},{0xA2, 0x00},{0xA3, 0x00},{0xA4, 0x00},{0xA5, 0x00},
                {0xA6, 0x00},{0xA7, 0x00},{0xA8, 0x00},{0xA9, 0x00},{0xAA, 0x00},{0xAB, 0x00},{0xAC, 0x00},
                {0xAD, 0x00},{0xAE, 0x00},{0xAF, 0x00},{0xB0, 0x00},{0xB1, 0x00},{0xB2, 0x00},{0xB3, 0x00},
                {0xB4, 0x00},{0xB5, 0x00},{0xB6, 0x00},{0xB7, 0x00},{0xB8, 0x00},{0xB9, 0x2E},{0xBA, 0x41},
                {0xBB, 0x57},{0xBC, 0x47},{0xBD, 0x4F},{0xBE, 0x77},{0xBF, 0x00},{0xC0, 0x00},{0xC1, 0x00},
                {0xC2, 0x00},{0xC3, 0x00},{0xC4, 0x00},{0xC5, 0x00},{0xC6, 0x00},{0xC7, 0x00},{0xC8, 0x00},
                {0xC9, 0x4F},{0xCA, 0x10},{0xCB, 0x11},{0x00, 0x00}
            };

            public readonly byte EncounterID;
            public readonly byte ModelID;
            public readonly string ObjectModelDigimonName;
            public readonly DigimonPackLevel Level;

            public DigimonPack(byte data, Vector2 position)
            {
                if (data == 0)
                {
                    this.ObjectModelDigimonName = "No Digimon";
                    this.Level = DigimonPackLevel.None;
                    return;
                }

                if (data - 1 > 4)
                {
                    var floorId = Domain.Main.floorsInThisDomain.Count;
                    var layoutID = DomainFloor.CurrentDomainFloor.UniqueDomainMapLayouts.Count;
                    DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"Index {data} is not a valid digimon on floor {floorId} " +
                                                                   $"layout {layoutID} at position (Dec){position}");

                    switch (DigimonWorld2ToolForm.ErrorMode)
                    {
                        case DigimonWorld2ToolForm.Strictness.Strict:
                            DigimonWorld2ToolForm.Main.AddLogToLogWindow($"Error checking set to Strict, stopping execution.");
                            return;

                        case DigimonWorld2ToolForm.Strictness.Sloppy:
                            this.ObjectModelDigimonName = "Error";
                            this.Level = DigimonPackLevel.Error;
                            return;
                    }
                }

                this.EncounterID = DomainFloor.CurrentDomainFloor.DigimonPacks[data - 1];
                if (PackIDToModelIDLookupTable.TryGetValue(EncounterID, out byte value))
                {
                    this.ModelID = value;
                }
                else
                {
                    var floorId = Domain.Main.floorsInThisDomain.Count + 1;
                    var layoutID = DomainFloor.CurrentDomainFloor.UniqueDomainMapLayouts.Count;
                    DigimonWorld2ToolForm.Main.AddErrorToLogWindow($"Byte {EncounterID:X2} was not found in lookup table for floor {floorId} layout {layoutID}");
                    // 0xFF maps to the "Custom digimon" encounter
                    switch (DigimonWorld2ToolForm.ErrorMode)
                    {
                        case DigimonWorld2ToolForm.Strictness.Strict:
                            throw new Exception("Unknown encounter found, stopping execution");

                        case DigimonWorld2ToolForm.Strictness.Sloppy:
                            this.ModelID = 0xFF;
                            break;
                    }
                }
                DigimonOverworldPack pack = DigimonModelLookupTable.FirstOrDefault(o => o.OverworldID == ModelID);

                if (pack != null)
                {
                    this.ObjectModelDigimonName = pack.Name;
                    this.Level = pack.Level;
                }
                else
                {
                    switch (EncounterID)
                    {
                        case 0x62:
                            this.ObjectModelDigimonName = "Kim";
                            this.Level = DigimonPackLevel.Special;
                            break;

                        case 0x65:
                        case 0x75:
                            this.ObjectModelDigimonName = "Bertran";
                            this.Level = DigimonPackLevel.Special;
                            break;

                        case 0x66:
                        case 0x6B:
                        case 0x82:
                        case 0x8C:
                            this.ObjectModelDigimonName = "Damien";
                            this.Level = DigimonPackLevel.Special;
                            break;

                        case 0x6C:
                        case 0x81:
                            this.ObjectModelDigimonName = "Crimson";
                            this.Level = DigimonPackLevel.Special;
                            break;

                        case 0x76:
                            this.ObjectModelDigimonName = "Jojo";
                            this.Level = DigimonPackLevel.Special;
                            break;

                        case 0x77:
                            this.ObjectModelDigimonName = "Mark Shultz";
                            this.Level = DigimonPackLevel.Special;
                            break;

                        case 0x78:
                            this.ObjectModelDigimonName = "Debbie";
                            this.Level = DigimonPackLevel.Special;
                            break;

                        case 0x79:
                            this.ObjectModelDigimonName = "Chris Connor";
                            this.Level = DigimonPackLevel.Special;
                            break;

                        case 0x90:
                            this.ObjectModelDigimonName = "NeoCrimson";
                            this.Level = DigimonPackLevel.Special;
                            break;

                        case 0x9c:
                            this.ObjectModelDigimonName = "Centarumon";
                            this.Level = DigimonPackLevel.Special;
                            break;

                        case 0x67:
                        case 0x68:
                        case 0x69:
                        case 0x71:
                        case 0x72:
                        case 0x73:
                        case 0x74:
                        case 0x83:
                        case 0x8A:
                        case 0x89:
                            this.ObjectModelDigimonName = "Blood knight";
                            this.Level = DigimonPackLevel.Special;
                            break;

                        default:
                            this.ObjectModelDigimonName = "Error";
                            this.Level = DigimonPackLevel.Error;
                            break;
                    }
                }
            }

            private class DigimonOverworldPack
            {
                public readonly byte OverworldID;
                public readonly string Name;
                public readonly DigimonPackLevel Level;

                public DigimonOverworldPack(byte ID, string Name, DigimonPackLevel Level)
                {
                    this.OverworldID = ID;
                    this.Name = Name;
                    this.Level = Level;
                }
            }
        }
    }
}
