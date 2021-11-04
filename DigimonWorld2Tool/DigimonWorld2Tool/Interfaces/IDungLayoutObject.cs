using System;
using System.Collections.Generic;
using System.Text;
using DigimonWorld2Tool.Utility;

namespace DigimonWorld2Tool.Interfaces
{
    public abstract class IDungLayoutObject
    {
        public int X { get; protected set; }
        public int Y { get; protected set; }

        public Vector2 Position 
        {
            get
            {
                return new Vector2(X, Y);
            }
        }
    }
}
