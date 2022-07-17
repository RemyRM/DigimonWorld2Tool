using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DigimonWorld2Tool.FileFormats
{
    /// <summary>
    /// This handles the animations for the A- through K files foundin 4.AAA/MODEL/
    /// </summary>
    class AnimationFile
    {
        public AnimationHeader header { get; private set; }
        public Tuple<int, List<AnimationData>> AnimationPerbone { get; private set; }

        public AnimationFile(byte[] rawFileData)
        {
            Stream stream = new MemoryStream(rawFileData);
            using (BinaryReader reader = new BinaryReader(stream))
            {

            }
        }
    }

    class AnimationHeader
    {
        public int FileStart { get; set; }
        public int[] BonesFrameDataPointers { get; private set; }
        public int KeyFrameTablePointer { get; private set; }
        public int HeaderPointer { get; private set; }

        public byte[] KeyframeTablePointers { get; private set; }
        public int LoopMode { get; private set; }

        public AnimationHeader(ref BinaryReader reader)
        {
            FileStart = reader.ReadInt32();
            //This should be changed to be bone count based, however that requires a link to the M file which isn't yet there.
            
        }
    }



    class AnimationData
    {
        public short MoveTopAlongX { get; private set; }
        public short SkewYaw { get; private set; }
        public short XScale { get; private set; }
        public short YScale { get; private set; }
        public short SkewTilt { get; private set; }
        public short SkewRoll { get; private set; }
        public short MoveTopAlongZ { get; private set; }
        public short ZScale { get; private set; }
        public short SkewalongY { get; private set; }

        public AnimationData(BinaryReader reader)
        {
            MoveTopAlongX = reader.ReadInt16();
            SkewYaw = reader.ReadInt16();
            XScale = reader.ReadInt16();
            YScale = reader.ReadInt16();
            SkewTilt = reader.ReadInt16();
            SkewRoll = reader.ReadInt16();
            MoveTopAlongZ = reader.ReadInt16();
            ZScale = reader.ReadInt16();
            SkewalongY = reader.ReadInt16();
        }
    }
}

