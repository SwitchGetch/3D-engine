using SFML.Window;
using SFML.System;

public static class MouseMoveHandler
{
    private static int x = (int)Render.Window.Size.X / 2;
    private static int y = (int)Render.Window.Size.Y / 2;

    public static int dx {  get; private set; }
    public static int dy {  get; private set; }

    public static void CountDifference()
    {
        int X = Mouse.GetPosition(Render.Window).X;
        int Y = Mouse.GetPosition(Render.Window).Y;

        dx = X - x;
        dy = Y - y;

        Mouse.SetPosition(new Vector2i(x, y), Render.Window);
    }
}