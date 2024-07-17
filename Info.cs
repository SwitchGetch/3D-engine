using SFML.Graphics;

public static class Info
{
    private static Font Font = new Font("arial.ttf");
    private static Text Text = new Text() { Font = Font, CharacterSize = 20 };

    public static bool Show = true;

    public static void Draw()
    {
        if (!Show) return;

        Text.DisplayedString =
            " POSITION:" + "\n X: " + Camera.Position.X + "\n Y: " + Camera.Position.Y + "\n Z: " + Camera.Position.Z +
            "\n\n DIRECTION:" + "\n X: " + Camera.Direction.X + "\n Y: " + Camera.Direction.Y + "\n Z: " + Camera.Direction.Z +
            "\n\n ROTATION: " + "\n XY: " + Camera.AngleXY + "°" + "\n XZ: " + Camera.AngleXZ + "°" + "\n YZ: " + Camera.AngleYZ + "°" +
            "\n\n DISTANCE: " + Camera.Distance + "\n\n PAUSE: " + Camera.Pause +
            "\n\n MOVING SPEED: " + Camera.MovingSpeed + "\n\n ROTATING SPEED: " + Camera.RotatingSpeed + "\n\n ZOOMING SPEED: " + Camera.ZoomingSpeed +
            "\n\n DRAW TRIANGLE: " + Camera.DrawTriangle + "\n\n DRAW BORDER: " + Camera.DrawBorder + "\n\n DRAW NORMAL: " + Camera.DrawNormal +
            "\n\n FPS: " + Render.FPS + "\n\n RENDERED POLYGONS COUNT: " + Camera.RenderedPolygonsCount;

        Render.Window.Draw(Text);
    }
}