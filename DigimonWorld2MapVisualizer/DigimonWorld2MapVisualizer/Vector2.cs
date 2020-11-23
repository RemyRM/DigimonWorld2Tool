using System;
using System.Collections.Generic;
using System.Text;

namespace DigimonWorld2MapVisualizer
{
    struct Vector2
    {
        public double x;
        public double y;

        
        public static Vector2 Zero { get; } = new Vector2(0, 0);
        public static Vector2 Right { get; } = new Vector2(1, 0);
        public static Vector2 Up { get; } = new Vector2(0, 1);

        public double Magnitude
        {
            get
            {
                return Math.Sqrt(this.x * this.x + this.y * this.y);
            }
        }

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"({x},{y})";
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
        public static Vector2 operator *(Vector2 a, double b) => new Vector2(a.x * b, a.y * b);
        public static Vector2 operator *(double a, Vector2 b) => new Vector2(b.x * a, b.y * a);
        public static Vector2 operator /(Vector2 a, float d) => new Vector2(a.x / d, a.y / d);

        public static double SqrMagnitude(Vector2 a)
        {
            return (a.x * a.x + a.y * a.y);
        }

        public static bool operator ==(Vector2 lhs, Vector2 rhs) =>(double)Vector2.SqrMagnitude(lhs - rhs) < 9.99999943962493E-11;

        public static bool operator !=(Vector2 lhs, Vector2 rhs)
        {
            return Vector2.SqrMagnitude(lhs - rhs) >= 9.99999943962493E-11;
        }
    }
}
