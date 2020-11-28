using System;

namespace DigimonWorld2MapVisualizer.Utility
{
    public struct Vector2
    {
        public int x;
        public int y;

        /// <summary>
        /// Shorthand for Vector2(0,0)
        /// </summary>
        public static Vector2 Zero { get; } = new Vector2(0, 0);
        /// <summary>
        /// Shorthand for Vector2(1,0)
        /// </summary>
        public static Vector2 Right { get; } = new Vector2(1, 0);
        /// <summary>
        /// Shorthand for Vector2(0,1)
        /// </summary>
        public static Vector2 Up { get; } = new Vector2(0, 1);
        /// <summary>
        /// Shorthand for Vector2(0,1)
        /// </summary>
        public static Vector2 One { get; } = new Vector2(1, 1);

        public double Magnitude
        {
            get
            {
                return Math.Sqrt(this.x * this.x + this.y * this.y);
            }
        }

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"({x},{y})";
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
        public static Vector2 operator *(Vector2 a, int b) => new Vector2(a.x * b, a.y * b);
        public static Vector2 operator *(int a, Vector2 b) => new Vector2(b.x * a, b.y * a);

        public static bool operator ==(Vector2 a, Vector2 b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Vector2 a, Vector2 b) => a.x != b.x || a.y != b.y;
    }
}
