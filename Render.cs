using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Diagnostics;

public static class Render
{
    public static RenderWindow Window = new RenderWindow(new VideoMode(1920, 1080), "", Styles.Fullscreen);

    public static int FPS = 0;

    public static void Start()
    {
        Window.SetFramerateLimit(60);
        Window.Closed += (s, e) => Window.Close();
        Window.KeyPressed += Camera.KeyboardEvents;
        Window.MouseWheelScrolled += Camera.Zoom;

        Window.SetMouseCursorVisible(false);
        Mouse.SetPosition(new Vector2i((int)Window.Size.X / 2, (int)Window.Size.Y / 2), Window);

        Objects.Initialize();

        Stopwatch timer = new Stopwatch();

        while (Window.IsOpen)
        {
            timer.Restart();

            Window.DispatchEvents();

            Window.Clear();

            Camera.Move();
            Camera.Rotate();
            Camera.Draw();

            Info.Draw();

            Window.Display();

            FPS = 1000 / (int)timer.ElapsedMilliseconds;
            Window.SetTitle("FPS: " + FPS);
        }
    }
}