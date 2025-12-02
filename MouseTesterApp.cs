using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using System.Diagnostics;

public class MouseTesterApp : IDisposable
{
    private IWindow _window;
    private ImGuiController _controller;
    private GL _gl;
    private IInputContext _input;

    private long _lastTimestamp = 0;
    private double[] _intervals = new double[2000];
    private int _index = 0;
    private bool _filled = false;
    private double _tickFrequency = (double)Stopwatch.Frequency;

    private double _currentHz = 0;
    private bool _isRaw = false;

    public void Run()
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(500, 300);
        options.Title = "Mouse Polling Rate Tester";
        options.VSync = false;
        options.FramesPerSecond = 2000;
        options.UpdatesPerSecond = 2000;

        _window = Window.Create(options);
        _window.Load += OnLoad;
        _window.Render += OnRender;
        _window.Closing += OnClose;

        _window.Run();
    }

    private void OnLoad()
    {
        _input = _window.CreateInput();

        if (_input.Mice.Count > 0)
        {
            var mouse = _input.Mice[0];

            bool isMac = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);

            if (!isMac)
            {
                try
                {
                    mouse.Cursor.CursorMode = CursorMode.Raw;
                    _isRaw = true;
                }
                catch
                {
                    _isRaw = false;
                }
            }
            else
            {
                _isRaw = false;
            }
            mouse.MouseMove += (m, pos) => Measure();
        }

        if (_input.Keyboards.Count > 0)
        {
            var keyboard = _input.Keyboards[0];
            keyboard.KeyDown += (kb, key, scancode) =>
            {
                if (key == Key.Escape)
                {
                    _window.Close();
                }
            };
        }

        _gl = _window.CreateOpenGL();
        _controller = new ImGuiController(_gl, _window, _input);
    }

    private void OnRender(double delta)
    {
        _gl.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
        _gl.Clear((uint)ClearBufferMask.ColorBufferBit);

        _controller.Update((float)delta);

        ImGuiNET.ImGui.SetNextWindowPos(new System.Numerics.Vector2(10, 10), ImGuiNET.ImGuiCond.Always);
        ImGuiNET.ImGui.SetNextWindowSize(new System.Numerics.Vector2(460, 260), ImGuiNET.ImGuiCond.Always);

        ImGuiNET.ImGui.Begin("Result:", ImGuiNET.ImGuiWindowFlags.NoMove | ImGuiNET.ImGuiWindowFlags.NoResize | ImGuiNET.ImGuiWindowFlags.NoTitleBar);

        ImGuiNET.ImGui.SetWindowFontScale(2.0f);
        ImGuiNET.ImGui.Text("Mouse Polling Rate Tester");
        ImGuiNET.ImGui.Separator();
        ImGuiNET.ImGui.Spacing();

        if (_currentHz > 0)
        {
            var color = new System.Numerics.Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            if (_currentHz > 900) color = new System.Numerics.Vector4(0.0f, 1.0f, 0.0f, 1.0f);
            if (_currentHz > 3000) color = new System.Numerics.Vector4(0.0f, 0.8f, 1.0f, 1.0f);

            ImGuiNET.ImGui.TextColored(color, $"Current: {_currentHz:F0} Hz");
        }
        else
        {
            ImGuiNET.ImGui.Text("Move the mouse...");
        }

        ImGuiNET.ImGui.Spacing();
        ImGuiNET.ImGui.SetWindowFontScale(1.0f);
        ImGuiNET.ImGui.Text($"Mode: {(_isRaw ? "RAW INPUT" : "Standard Input")}");
        ImGuiNET.ImGui.Text($"FPS: {1.0 / delta:F0}");

        ImGuiNET.ImGui.Separator();
        ImGuiNET.ImGui.TextColored(new System.Numerics.Vector4(1.0f, 0.5f, 0.0f, 1.0f), "Press ESC to close");

        ImGuiNET.ImGui.End();

        _controller.Render();
    }

    private void Measure()
    {
        long now = Stopwatch.GetTimestamp();

        if (_lastTimestamp != 0)
        {
            double dtSeconds = (now - _lastTimestamp) / _tickFrequency;
            if (dtSeconds > 0.0000001)
            {
                _intervals[_index] = dtSeconds;
                _index++;
                if (_index >= _intervals.Length)
                {
                    _index = 0;
                    _filled = true;
                }
            }
        }
        _lastTimestamp = now;

        if (_filled)
        {
            double avg = _intervals.Average();
            if (avg > 0) _currentHz = 1.0 / avg;
        }
    }

    private void OnClose()
    {
    }

    public void Dispose()
    {
        _controller?.Dispose();
        _input?.Dispose();
        _gl?.Dispose();
    }
}