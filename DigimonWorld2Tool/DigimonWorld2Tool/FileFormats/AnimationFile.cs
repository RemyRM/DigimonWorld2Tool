using System;
using System.Collections.Generic;
using System.Text;

namespace DigimonWorld2Tool.FileFormats
{
    class AnimationFile
    {

    }

    class Header
    {
        public int HeaderPointer { get; private set; }
        public int[] BonesFrameDataPointers { get; private set; }
        public int[] KeyframeTablePointers { get; private set; }
        public int Terminator { get; private set; }
    }
}

