using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DigimonWorld2Tool.FileFormats
{
    class ModelFile
    {
        public ModelFileHeader Header { get; private set; }
        public EyesData Eyes { get; private set; }
        public VertexData[] VertexData { get; private set; }
        public NormalData[] NormalData { get; private set; }
        public PrimitiveData[] PrimitiveData { get; private set; }

        public ModelFile(byte[] rawMFileData)
        {
            Stream stream = new MemoryStream(rawMFileData);
            using (BinaryReader reader = new BinaryReader(stream))
            {
                Header = new ModelFileHeader(reader);
                Eyes = new EyesData(reader);

                VertexData = new VertexData[Header.BoneCount];
                for (int i = 0; i < VertexData.Length; i++)
                {
                    reader.BaseStream.Position = Header.VertexPointerTable[i];
                    VertexData[i] = new VertexData(reader);
                }

                NormalData = new NormalData[Header.BoneCount];
                for (int i = 0; i < NormalData.Length; i++)
                {
                    reader.BaseStream.Position = Header.NormalsPointerTable[i];
                    NormalData[i] = new NormalData(reader);
                }

                PrimitiveData = new PrimitiveData[Header.BoneCount];
                for (int i = 0; i < PrimitiveData.Length; i++)
                {
                    reader.BaseStream.Position = Header.PrimitivesPointerTable[i];
                    PrimitiveData[i] = new PrimitiveData(reader);
                }
            }
        }
    }

    class ModelFileHeader
    {
        public int TIMPointer { get; private set; }
        public int TIMTerminator { get; private set; }
        public int BoneCount { get; private set; }
        public int[] VertexPointerTable { get; private set; } //Length always equal to BoneCount
        public int[] NormalsPointerTable { get; private set; } //Length always equal to BoneCount
        public int[] PrimitivesPointerTable { get; private set; } //Length always equal to BoneCount
        public int[] Bones { get; private set; } //Length always equal to BoneCount

        public ModelFileHeader(BinaryReader reader)
        {

            TIMPointer = reader.ReadInt32();
            TIMTerminator = reader.ReadInt32();
            BoneCount = reader.ReadInt32();

            VertexPointerTable = new int[BoneCount];
            for (int i = 0; i < BoneCount; i++)
                VertexPointerTable[i] = reader.ReadInt32();

            NormalsPointerTable = new int[BoneCount];
            for (int i = 0; i < BoneCount; i++)
                NormalsPointerTable[i] = reader.ReadInt32();

            PrimitivesPointerTable = new int[BoneCount];
            for (int i = 0; i < BoneCount; i++)
                PrimitivesPointerTable[i] = reader.ReadInt32();

            Bones = new int[BoneCount];
            for (int i = 0; i < BoneCount; i++)
                Bones[i] = reader.ReadInt32();

        }
    }

    class EyesData
    {
        public byte U { get; private set; }
        public byte V { get; private set; }
        public byte Width { get; private set; } // This is the width in bytes, to get the pixel width multiple by 2
        public byte Height { get; private set; }
        public EyeFrameData[] BlinkAnimation { get; private set; } = new EyeFrameData[8]; //There's a maximum of 8 steps in a blinking animation
        public int LoopTerminator { get; private set; }

        public EyesData(BinaryReader reader)
        {
            U = reader.ReadByte();
            V = reader.ReadByte();
            Width = reader.ReadByte();
            Height = reader.ReadByte();
            for (int i = 0; i < BlinkAnimation.Length; i++)
            {
                byte x = reader.ReadByte();
                byte y = reader.ReadByte();
                BlinkAnimation[i] = new EyeFrameData(x, y);
            }
            LoopTerminator = reader.ReadInt32();
        }
    }

    struct EyeFrameData
    {
        public byte XOffset { get; set; }
        public byte YOffset { get; set; }

        public EyeFrameData(byte xOffset, byte yOffset)
        {
            XOffset = xOffset;
            YOffset = yOffset;
        }

        public override string ToString() => $"Offset X: {XOffset}, Offset Y: {YOffset}";
    }

    class PrimitiveData
    {
        public int QuadPadding { get; private set; }
        public int QuadCount { get; private set; }
        public PrimitiveQuad[] QuadsData { get; private set; }

        public int TrisPadding { get; private set; }
        public int TrisCount { get; private set; }
        public PrimitiveTri[] TrisData { get; private set; }

        public PrimitiveData(BinaryReader reader)
        {
            QuadPadding = reader.ReadInt32();
            QuadCount = reader.ReadInt32();
            QuadsData = new PrimitiveQuad[QuadCount];
            for (int i = 0; i < QuadsData.Length; i++)
                QuadsData[i] = new PrimitiveQuad(reader);


            TrisPadding = reader.ReadInt32();
            TrisCount = reader.ReadInt32();
            TrisData = new PrimitiveTri[TrisCount];
            for (int i = 0; i < TrisData.Length; i++)
                TrisData[i] = new PrimitiveTri(reader);
        }
    }

    class PrimitiveQuad
    {
        public byte[] VertexIds { get; private set; } = new byte[4];
        public byte[] NormalIds { get; private set; } = new byte[4];

        public Quad QuadTexturePosition { get; private set; }

        public byte Unknown1 { get; private set; }
        public byte Unknown2 { get; private set; }
        public byte Unknown3 { get; private set; }
        public byte Unknown4 { get; private set; }

        public PrimitiveQuad(BinaryReader reader)
        {
            VertexIds = reader.ReadBytes(4);
            NormalIds = reader.ReadBytes(4);
            QuadTexturePosition = new Quad(reader.ReadBytes(8));
            Unknown1 = reader.ReadByte();
            Unknown2= reader.ReadByte();
            Unknown3 = reader.ReadByte();
            Unknown4 = reader.ReadByte();
        }

        public byte[] GetVerticesAntiClockWise()
        {
            List<byte> vertices = VertexIds.ToList();
            vertices.Sort();
            var secondLowestByte = vertices[1];
            vertices.RemoveAt(1);
            vertices.Add(secondLowestByte);
            return vertices.ToArray();
        }
    }

    class PrimitiveTri
    {
        public byte[] VertexIds { get; private set; } = new byte[3];
        public byte[] NormalIds { get; private set; } = new byte[3];

        public Tri TriTexturePosition { get; private set; }

        public byte Unknown1 { get; private set; }
        public byte Unknown2 { get; private set; }
        public byte Unknown3 { get; private set; }
        public byte Unknown4 { get; private set; }

        public PrimitiveTri(BinaryReader reader)
        {
            VertexIds = reader.ReadBytes(3);
            NormalIds = reader.ReadBytes(3);
            TriTexturePosition = new Tri(reader.ReadBytes(6));
            Unknown1 = reader.ReadByte();
            Unknown2 = reader.ReadByte();
            Unknown3 = reader.ReadByte();
            Unknown4 = reader.ReadByte();
        }
    }

    struct Point
    {
        public byte X { get; private set; }
        public byte Y { get; private set; }

        public Point(byte x, byte y)
        {
            X = x;
            Y = y;
        }
    }

    struct Quad
    {
        public Point[] Points { get; private set; }

        public Quad(byte[] data)
        {
            Points = new Point[4];
            for (int i = 0; i < data.Length; i+=2)
                Points[(int)Math.Floor((float)i / 2)] = new Point(data[i], data[i+1]);
        }
    }

    struct Tri
    {
        public Point[] Points { get; private set; }

        public Tri(byte[] data)
        {
            Points = new Point[3];
            for (int i = 0; i < data.Length; i += 2)
                Points[(int)Math.Floor((float)i / 2)] = new Point(data[i], data[i + 1]);
        }
    }

    class VertexData
    {
        public int VertexCount { get; private set; }
        public short StartPadding { get; private set; }
        public Vertex[] Vertecis { get; private set; }
        public bool HasEndPadding { get; private set; }

        public VertexData(BinaryReader reader)
        {
            VertexCount = reader.ReadInt32();
            StartPadding = reader.ReadInt16();

            Vertecis = new Vertex[VertexCount];
            for (int i = 0; i < VertexCount; i++)
            {
                short y = reader.ReadInt16();
                short z = reader.ReadInt16();
                short x = reader.ReadInt16();
                Vertecis[i] = new Vertex(x, y, z);
            }
            //Even numbered vertex count has an extra 2 byte padding at the end at allign to 4 bytes
            if (VertexCount % 2 == 0)
                HasEndPadding = true;
        }
    }

    struct Vertex
    {
        //The Verteci in-game use a right handed co-ord system YZX
        public short Y { get; private set; }
        public short Z { get; private set; }
        public short X { get; private set; }

        public Vertex(short x, short y, short z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString() => $"Vertex position: ({Y:X4}, {Z:X4}, {X:X4})";
    }

    class NormalData
    {
        public int NormalsCount { get; private set; }
        public short StartPadding { get; private set; }
        public Normal[] Normals { get; private set; }
        public bool HasEndPadding { get; private set; }

        public NormalData(BinaryReader reader)
        {
            NormalsCount = reader.ReadInt32();
            StartPadding = reader.ReadInt16();
            Normals = new Normal[NormalsCount];
            for (int i = 0; i < NormalsCount; i++)
            {
                short y = reader.ReadInt16();
                short z = reader.ReadInt16();
                short x = reader.ReadInt16();
                Normals[i] = new Normal(x, y, z);
            }

            //Even numbered vertex count has an extra 2 byte padding at the end at allign to 4 bytes
            if (NormalsCount % 2 == 0)
                HasEndPadding = true;
        }
    }

    struct Normal
    {
        //I'm not 100% sure on the coord system for normals by the game, but for now I'll assume its the same as verteci
        public short Y { get; private set; }
        public short Z { get; private set; }
        public short X { get; private set; }

        public Normal(short x, short y, short z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString() => $"Normals: ({Y:X4}, {Z:X4}, {X:X4})";
    }
}
