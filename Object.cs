using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

public class Triangle
{
    public Point P1 = new Point();
    public Point P2 = new Point();
    public Point P3 = new Point();

    public Point N = new Point();

    public Point Center { get => (P1 + P2 + P3) / 3; }

    public Triangle() { }

    public Triangle(Point P1, Point P2, Point P3, Point N)
    {
        this.P1 = P1;
        this.P2 = P2;
        this.P3 = P3;
        this.N = N;
    }
}

public class Hitbox
{
    public Point Position = new Point();
    public Point Size = new Point();

    public List<Point> Points
    {
        get => new List<Point>()
        {
            new Point(Position.X, Position.Y, Position.Z),
            new Point(Position.X + Size.X, Position.Y, Position.Z),
            new Point(Position.X, Position.Y, Position.Z + Size.Z),
            new Point(Position.X + Size.X, Position.Y, Position.Z + Size.Z),
            new Point(Position.X, Position.Y + Size.Y, Position.Z),
            new Point(Position.X + Size.X, Position.Y + Size.Y, Position.Z),
            new Point(Position.X, Position.Y + Size.Y, Position.Z + Size.Z),
            new Point(Position.X + Size.X, Position.Y + Size.Y, Position.Z + Size.Z)
        };
    }

    public Hitbox() { }

    public Hitbox(Point P, Point S)
    {
        Position = P;
        Size = S;
    }

    public bool Collision(Hitbox H)
    {
        return
            Math.Abs(H.Position.X - Position.X) < H.Size.X + Size.X &&
            Math.Abs(H.Position.Y - Position.Y) < H.Size.Y + Size.Y &&
            Math.Abs(H.Position.Z - Position.Z) < H.Size.Z + Size.Z;
    }
}

public class Cube
{
    public Point Position = new Point();
    public Point Size = new Point();

    public Point Center { get; private set; }

    public List<Triangle> Polygons = new List<Triangle>();

    public Cube() { Center = new Point(); }

    public Cube(Point P, Point S)
    {
        Position = P;
        Size = S;
        Center = P + S / 2;

        Point P0 = P;
        Point P1 = new Point(P.X + S.X, P.Y, P.Z);
        Point P2 = new Point(P.X, P.Y, P.Z + S.Z);
        Point P3 = new Point(P.X + S.X, P.Y, P.Z + S.Z);
        Point P4 = new Point(P.X, P.Y + S.Y, P.Z);
        Point P5 = new Point(P.X + S.X, P.Y + S.Y, P.Z);
        Point P6 = new Point(P.X, P.Y + S.Y, P.Z + S.Z);
        Point P7 = P + S;

        Point XN = new Point(-1, 0, 0);
        Point XP = new Point(1, 0, 0);
        Point YN = new Point(0, -1, 0);
        Point YP = new Point(0, 1, 0);
        Point ZN = new Point(0, 0, -1);
        Point ZP = new Point(0, 0, 1);

        Polygons.Add(new Triangle(P0, P1, P2, YN));
        Polygons.Add(new Triangle(P1, P2, P3, YN));
        Polygons.Add(new Triangle(P0, P1, P5, ZN));
        Polygons.Add(new Triangle(P0, P4, P5, ZN));
        Polygons.Add(new Triangle(P0, P2, P4, XN));
        Polygons.Add(new Triangle(P2, P4, P6, XN));
        Polygons.Add(new Triangle(P1, P3, P7, XP));
        Polygons.Add(new Triangle(P1, P5, P7, XP));
        Polygons.Add(new Triangle(P2, P3, P6, ZP));
        Polygons.Add(new Triangle(P3, P6, P7, ZP));
        Polygons.Add(new Triangle(P4, P5, P6, YP));
        Polygons.Add(new Triangle(P5, P6, P7, YP));
    }

    public void Rotate(float Angle, Plane Plane, Point Depending)
    {
        for (int i = 0; i < Polygons.Count; i++)
        {
            Polygons[i].P1 = Vector.Rotate(Polygons[i].P1 - Depending, Angle, Plane) + Depending;
            Polygons[i].P2 = Vector.Rotate(Polygons[i].P2 - Depending, Angle, Plane) + Depending;
            Polygons[i].P3 = Vector.Rotate(Polygons[i].P3 - Depending, Angle, Plane) + Depending;

            Polygons[i].N = Vector.Rotate(Polygons[i].N, Angle, Plane);
        }

        Center = Vector.Rotate(Center - Depending, Angle, Plane) + Depending;
    }
}

public static class Objects
{
    public static List<Cube> Cubes = new List<Cube>();

    public static void Initialize()
    {
        //Cubes.Add(new Cube() { Polygons = new List<Triangle>(ObjectFileConverter.GetObject("Objects/suzanne.obj")) });

        float C = 1000;

        Point P = new Point(C, C, C);
        Point S = new Point(C, C, C);

        int I = 5;
        int J = 5;
        int K = 5;

        for (int i = 0; i < I; i++)
        {
            for (int j = 0; j < J; j++)
            {
                for (int k = 0; k < K; k++)
                {
                    Cubes.Add(new Cube(P + new Point(2 * C * i, 2 * C * j, 2 * C * k), S));
                }
            }
        }
    }
}