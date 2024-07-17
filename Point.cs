using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

public class Point
{
    public float X = 0, Y = 0, Z = 0;

    public Point() { }

    public void Write()
    {
        Console.WriteLine(" X: " + X + " Y: " + Y + " Z: " + Z);
    }

    public Point(float X = 0, float Y = 0, float Z = 0) { this.X = X; this.Y = Y; this.Z = Z; }

    public static Point operator +(Point P1, Point P2) => new Point(P1.X + P2.X, P1.Y + P2.Y, P1.Z + P2.Z);

    public static Point operator -(Point P1, Point P2) => new Point(P1.X - P2.X, P1.Y - P2.Y, P1.Z - P2.Z);

    public static Point operator *(float C, Point P) => new Point(C * P.X, C * P.Y, C * P.Z);

    public static Point operator *(Point P, float C) => new Point(C * P.X, C * P.Y, C * P.Z);

    public static Point operator /(Point P, float C) => new Point(P.X / C, P.Y / C, P.Z / C);

    public static bool operator ==(Point P1, Point P2) => P1.X == P2.X && P1.Y == P2.Y && P1.Z == P2.Z;

    public static bool operator !=(Point P1, Point P2) => P1.X != P2.X || P1.Y != P2.Y || P1.Z != P2.Z;

    public override bool Equals(object? obj) => base.Equals(obj);

    public override int GetHashCode() => base.GetHashCode();
}

public static class Vector
{
    public static Point Normalize(Point P) => P / Length(P);

    public static float Length(Point P) => (float)Math.Sqrt(P.X * P.X + P.Y * P.Y + P.Z * P.Z);

    public static float Distance(Point P1, Point P2) => (float)Math.Sqrt(Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2) + Math.Pow(P1.Z - P2.Z, 2));

    public static float ScalarProduct(Point P1, Point P2) => P1.X * P2.X + P1.Y * P2.Y + P1.Z * P2.Z;

    public static Point VectorProduct(Point P1, Point P2) => new Point(P1.Y * P2.Z - P1.Z * P2.Y, P1.X * P2.Z - P1.Z * P2.X, P1.X * P2.Y - P1.Y * P2.X);

    public static Point Rotate(Point P, float A, Plane Plane)
    {
        A *= (float)Math.PI / 180;
        float Sin = (float)Math.Sin(A);
        float Cos = (float)Math.Cos(A);

        switch (Plane)
        {
            case Plane.XY: return new Point(Cos * P.X - Sin * P.Y, Sin * P.X + Cos * P.Y, P.Z);
            case Plane.XZ: return new Point(Cos * P.X + Sin * P.Z, P.Y, Cos * P.Z - Sin * P.X);
            case Plane.YZ: return new Point(P.X, Cos * P.Y - Sin * P.Z, Sin * P.Y + Cos * P.Z);
            default: return P;
        }
    }
}

public enum Plane { XY, XZ, YZ }