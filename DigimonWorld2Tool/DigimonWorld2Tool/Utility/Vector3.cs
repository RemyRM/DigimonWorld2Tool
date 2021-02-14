using System;

namespace DigimonWorld2Tool.Utility
{
    class Vector3
    {
        public int x;
        public int y;
        public int z;

        /// <summary>
        /// Shorthand for Vector3(0,0,0)
        /// </summary>
        public static Vector3 Zero { get; } = new Vector3(0, 0, 0);
        /// <summary>
        /// Shorthand for Vector3(1,0,0)
        /// </summary>
        public static Vector3 Right { get; } = new Vector3(1, 0, 0);
        /// <summary>
        /// Shorthand for Vector3(0,1,0)
        /// </summary>
        public static Vector3 Up { get; } = new Vector3(0, 1, 0);
        /// <summary>
        /// Shorthand for Vector3(0,0,1)
        /// </summary>
        public static Vector3 Forward { get; } = new Vector3(0, 0, 1);
        /// <summary>
        /// Shorthand for Vector3(1,1,1)
        /// </summary>
        public static Vector3 One { get; } = new Vector3(1, 1, 1);

        public double Magnitude
        {
            get
            {
                return Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
            }
        }

        public Vector3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return $"({x},{y},{z})";
        }

        public string ToStringHex()
        {
            return $"{x:X2} {y:X2} {z:X2}";
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3 operator -(Vector3 a, Vector3 b) => new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vector3 operator *(Vector3 a, int b) => new Vector3(a.x * b, a.y * b, a.z * b);
        public static Vector3 operator *(int a, Vector3 b) => new Vector3(b.x * a, b.y * a, b.z * a );

        public static bool operator ==(Vector3 a, Vector3 b) => a.x == b.x && a.y == b.y && a.z == b.z;
        public static bool operator !=(Vector3 a, Vector3 b) => a.x != b.x || a.y != b.y && a.z != b.z;
    }
}
