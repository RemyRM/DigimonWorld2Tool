using System;
using System.Collections.Generic;
using System.Text;
using DigimonWorld2Tool.Utility;

namespace DigimonWorld2Tool.Interfaces
{
    public abstract class IDungLayoutObject
    {
        public byte X { get; set; }
        public byte Y { get; set; }

        public Vector2 Position 
        {
            get
            {
                return new Vector2(X, Y);
            }
        }

        public abstract byte[] ToBytes();
    }
}
