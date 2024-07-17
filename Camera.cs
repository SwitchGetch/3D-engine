using SFML.Graphics;
using SFML.Window;
using SFML.System;

public static class Camera
{
    public static Point Position = new Point();
    public static Point Direction = new Point(0, 0, 1);


    public static float Distance = 500;

    public static float MovingSpeed = 25;
    public static float RotatingSpeed = 4;
    public static float ZoomingSpeed = 10;

    public static bool DrawTriangle = true;
    public static bool DrawBorder = false;
    public static bool DrawNormal = false;
    public static bool Pause = true;

    public static List<Triangle> Polygons = new List<Triangle>();
    public static int RenderedPolygonsCount = 0;

    private static float _AngleXY = 0;
    private static float _AngleXZ = 0;
    private static float _AngleYZ = 0;

    public static float AngleXY
    {
        get => _AngleXY;
        private set
        {
            _AngleXY = value;

            if (_AngleXY > 180) _AngleXY -= 360;
            else if (_AngleXY < -180) _AngleXY += 360;
        }
    }
    public static float AngleXZ
    {
        get => _AngleXZ;
        private set
        {
            _AngleXZ = value;

            if (_AngleXZ > 180) _AngleXZ -= 360;
            else if (_AngleXZ < -180) _AngleXZ += 360;
        }
    }
    public static float AngleYZ
    {
        get => _AngleYZ;
        private set
        {
            _AngleYZ = value;

            if (_AngleYZ > 180) _AngleYZ -= 360;
            else if (_AngleYZ < -180) _AngleYZ += 360;
        }
    }

    public static Vector2f Projection(Point P)
    {
        P -= Position;

        P = Vector.Rotate(P, -AngleXY, Plane.XY);
        P = Vector.Rotate(P, -AngleXZ, Plane.XZ);
        P = Vector.Rotate(P, -AngleYZ, Plane.YZ);

        if (P.Z <= 0) return new Vector2f();

        float X = Distance * P.X / P.Z + Render.Window.Size.X / 2;
        float Y = Distance * P.Y / P.Z + Render.Window.Size.Y / 2;

        return new Vector2f(X, Y);
    }

    private static void DrawPolygon(Triangle T)
    {
        float k = Vector.ScalarProduct(T.N, Vector.Normalize(Position - T.Center));

        if (k <= 0) return;

        VertexArray Triangle = new VertexArray() { PrimitiveType = PrimitiveType.Triangles };
        VertexArray Border = new VertexArray() { PrimitiveType = PrimitiveType.LineStrip };
        VertexArray Normal = new VertexArray() { PrimitiveType = PrimitiveType.Lines };

        byte C = Convert.ToByte(200 * k);
        //byte C = 150;
        Color TriangleColor = new Color(C, C, C);
        Color BorderColor = Color.White;
        Color PointsColor = Color.Red;

        Vector2f P1 = Projection(T.P1);
        Vector2f P2 = Projection(T.P2);
        Vector2f P3 = Projection(T.P3);

        Vector2f N1 = Projection(T.Center);
        Vector2f N2 = Projection(T.Center + Distance * T.N);

        bool DrawPolygon = P1 != new Vector2f() && P2 != new Vector2f() && P3 != new Vector2f();
        bool DrawLine = N1 != new Vector2f() && N2 != new Vector2f();

        if (DrawPolygon)
        {
            if (DrawTriangle)
            {
                Triangle.Append(new Vertex(P1, TriangleColor));
                Triangle.Append(new Vertex(P2, TriangleColor));
                Triangle.Append(new Vertex(P3, TriangleColor));

                Render.Window.Draw(Triangle);
            }

            if (DrawBorder)
            {
                Border.Append(new Vertex(P1, BorderColor));
                Border.Append(new Vertex(P2, BorderColor));
                Border.Append(new Vertex(P3, BorderColor));
                Border.Append(new Vertex(P1, BorderColor));

                Render.Window.Draw(Border);
            }

            RenderedPolygonsCount++;
        }

        if (DrawNormal && DrawLine)
        {
            Normal.Append(new Vertex(N1, PointsColor));
            Normal.Append(new Vertex(N2, PointsColor));

            Render.Window.Draw(Normal);
        }
    }

    private static void SortPolygons()
    {
        Polygons.Clear();

        //Objects.Cubes.Sort
        //    (
        //        (C1, C2) =>
        //        {
        //            float V1 = Vector.Distance(Position, C1.Center);
        //            float V2 = Vector.Distance(Position, C2.Center);

        //            return (int)(V2 - V1);
        //        }
        //    );

        for (int i = 0; i < Objects.Cubes.Count; i++)
        {
            for (int j = 0; j < Objects.Cubes[i].Polygons.Count; j++)
            {
                Polygons.Add(Objects.Cubes[i].Polygons[j]);
            }
        }

        Polygons.Sort((T1, T2) => (int)(Vector.Distance(Position, T2.Center) - Vector.Distance(Position, T1.Center)));
    }

    private static void RotateCubes()
    {
        if (Pause) return;

        for (int i = 0; i < Objects.Cubes.Count; i++)
        {
            Objects.Cubes[i].Rotate(1.0f, Plane.XZ, Objects.Cubes[i].Center);
        }
    }

    public static void Draw()
    {
        RenderedPolygonsCount = 0;

        RotateCubes();

        SortPolygons();

        for (int i = 0; i < Polygons.Count; i++)
        {
            DrawPolygon(Polygons[i]);
        }
    }

    public static void KeyboardEvents(object sender, EventArgs e)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.T)) DrawTriangle = DrawTriangle ? false : true;
        if (Keyboard.IsKeyPressed(Keyboard.Key.B)) DrawBorder = DrawBorder ? false : true;
        if (Keyboard.IsKeyPressed(Keyboard.Key.N)) DrawNormal = DrawNormal ? false : true;
        if (Keyboard.IsKeyPressed(Keyboard.Key.P)) Pause = Pause ? false : true;
        if (Keyboard.IsKeyPressed(Keyboard.Key.F)) Info.Show = Info.Show ? false : true;
    }

    public static void Move()
    {
        Point P = new Point();

        if (Keyboard.IsKeyPressed(Keyboard.Key.W)) P += Vector.Rotate(new Point(0, 0, 1), AngleXZ, Plane.XZ);
        if (Keyboard.IsKeyPressed(Keyboard.Key.S)) P -= Vector.Rotate(new Point(0, 0, 1), AngleXZ, Plane.XZ);
        if (Keyboard.IsKeyPressed(Keyboard.Key.D)) P += Vector.Rotate(new Point(1, 0, 0), AngleXZ, Plane.XZ);
        if (Keyboard.IsKeyPressed(Keyboard.Key.A)) P -= Vector.Rotate(new Point(1, 0, 0), AngleXZ, Plane.XZ);
        if (Keyboard.IsKeyPressed(Keyboard.Key.LShift)) P.Y += 1;
        if (Keyboard.IsKeyPressed(Keyboard.Key.Space)) P.Y -= 1;

        if (P != new Point()) Position += MovingSpeed * Vector.Normalize(P);
    }

    public static void Rotate()
    {
        MouseMoveHandler.CountDifference();

        if (MouseMoveHandler.dx < 0)
        {
            Direction = Vector.Rotate(Direction, -RotatingSpeed, Plane.XZ);
            AngleXZ -= RotatingSpeed;
        }
        else if (MouseMoveHandler.dx > 0)
        {
            Direction = Vector.Rotate(Direction, RotatingSpeed, Plane.XZ);
            AngleXZ += RotatingSpeed;
        }

        if (MouseMoveHandler.dy > 0 && AngleYZ > -90f)
        {
            Direction = Vector.Rotate(Direction, -RotatingSpeed, Plane.YZ);
            AngleYZ -= RotatingSpeed;
        }
        else if (MouseMoveHandler.dy < 0 && AngleYZ < 90f)
        {
            Direction = Vector.Rotate(Direction, RotatingSpeed, Plane.YZ);
            AngleYZ += RotatingSpeed;
        }

        Direction = Vector.Normalize(Direction);
    }

    public static void Zoom(object sender, EventArgs e)
    {
        MouseWheelScrollEventArgs m = (MouseWheelScrollEventArgs)e;
        Distance += ZoomingSpeed * m.Delta;

        if (Distance < 0) Distance = 0;
    }
}