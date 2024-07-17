public static class ObjectFileConverter
{
    private static List<Point> Points = new List<Point>();
    private static List<Point> Normals = new List<Point>();

    public static Point Center
    {
        get
        {
            Point S = new Point();

            for (int i = 0; i < Points.Count; i++) S += Points[i];

            return S / Points.Count;
        }
    }



    public static List<Triangle> GetObject(string Path)
    {
        Points.Clear();
        Normals.Clear();

        List<Triangle> Polygons = new List<Triangle>();
        List<string> Lines = File.ReadAllLines(Path).ToList();

        foreach (string Line in Lines)
        {
            switch(GetFirstLiteral(Line))
            {
                case "v": Points.Add(ConvertToPoint(Line)); break;
                case "vn": Normals.Add(ConvertToPoint(Line)); break;
                case "f": Polygons.Add(ConvertToPolygon(Line)); break;
            }
        }

        return Polygons;
    }

    private static string GetFirstLiteral(string Str) => Str.Substring(0, (Str.IndexOf(' ') < 0 ? Str.Length : Str.IndexOf(' ')));

    private static string GetWithoutLiteral(string Str) => Str.Substring(Str.IndexOf(' ') + 1, Str.Length - Str.IndexOf(' ') - 1);

    private static Point ConvertToPoint(string Str)
    {
        Str = GetWithoutLiteral(Str);
        Str = Str.Replace('.', ',');

        List<string> C = Str.Split(' ').ToList();

        float X = (float)Convert.ToDouble(C[0]);
        float Y = (float)Convert.ToDouble(C[1]);
        float Z = (float)Convert.ToDouble(C[2]);

        return new Point(X, Y, Z);
    }

    private static Triangle ConvertToPolygon(string Str)
    {
        Str = GetWithoutLiteral(Str);
        List<string> C = Str.Split(' ').ToList();

        List<int> PointIndex = new List<int>();
        List<int> NormalIndex = new List<int>();

        for (int i = 0; i < C.Count; i++)
        {
            List<string> c = C[i].Split('/').ToList();

            PointIndex.Add(Convert.ToInt32(c[0]));
            if (c.Count > 1) NormalIndex.Add(Convert.ToInt32(c[c.Count - 1]));
        }

        Point P1 = Points[PointIndex[0] - 1];
        Point P2 = Points[PointIndex[1] - 1];
        Point P3 = Points[PointIndex[2] - 1];
        Point Normal = new Point();

        if (NormalIndex.Count > 0)
        {
            for (int i = 0; i < NormalIndex.Count; i++)
            {
                int Index = NormalIndex[i] - 1;

                if (Index > -1 && Index < Normals.Count)
                {
                    Normal += Normals[Index];
                }
            }

            Normal = Vector.Normalize(Normal);
        }
        else
        {
            Normal = Vector.Normalize(Vector.VectorProduct(P2 - P1, P3 - P1));
        }

        return new Triangle(P1, P2, P3, Normal);
    }
}