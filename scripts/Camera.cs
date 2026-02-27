using Godot;
using System;

/// <summary>
/// кусок вайбкодинга не спрашивай как работает
/// </summary>
public partial class Camera : Camera2D
{
    [ExportGroup("Скорость")]
    [Export] public float BasePanSpeed = 750f;     // базовая скорость паннинга
    [Export] public float EdgeMargin = 18f;        // сколько пикселей от края экрана начинается edge-scroll

    [ExportGroup("Зум")]
    [Export] public float ZoomStep = 0.13f;        // насколько сильно зумится за один тик колёсика
    [Export] public Vector2 MinZoom = new(0.4f, 0.4f);
    [Export] public Vector2 MaxZoom = new(2.8f, 2.8f);

    private bool _isDragging = false;

    public override void _Ready()
    {
        // Если есть границы карты — поставь LimitLeft/Right/Top/Bottom
    }

    public override void _Process(double delta)
    {
        Vector2 dir = Vector2.Zero;

        // === WASD / стрелки ===
        dir.X += Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        dir.Y += Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

            Vector2 mousePos = GetViewport().GetMousePosition();
            Vector2 viewportSize = GetViewportRect().Size;
        if(!_isDragging)
        {
            if (mousePos.X < EdgeMargin) dir.X -= 1f;
            if (mousePos.X > viewportSize.X - EdgeMargin) dir.X += 1f;
            if (mousePos.Y < EdgeMargin) dir.Y -= 1f;
            if (mousePos.Y > viewportSize.Y - EdgeMargin) dir.Y += 1f;
        }
        if (dir.LengthSquared() > 0)
        {
            dir = dir.Normalized();
            // Скорость растёт при отдалении (как в HOI4)
            float speed = BasePanSpeed * Zoom.X;
            Position += dir * speed * (float)delta;
        }
    }

    public override void _Input(InputEvent @event)
    {
        switch (@event)
        {
            // === Drag средней кнопкой мыши ===
            case InputEventMouseButton mb when mb.ButtonIndex == MouseButton.Middle:
                _isDragging = mb.Pressed;
                break;

            // === Зум колёсиком к курсору ===
            case InputEventMouseButton mb when mb.ButtonIndex == MouseButton.WheelDown:
                ZoomAtCursor(-ZoomStep);
                break;

            case InputEventMouseButton mb when mb.ButtonIndex == MouseButton.WheelUp:
                ZoomAtCursor(ZoomStep);
                break;

            // === Drag ===
            case InputEventMouseMotion mm when _isDragging:
                Position -= mm.Relative / Zoom.X;   // самый стабильный вариант
                break;
        }
    }

    private void ZoomAtCursor(float deltaZoom)
    {
        Vector2 worldMouseBefore = GetGlobalMousePosition();

        // Применяем зум (равномерно по X/Y)
        Zoom = (Zoom + new Vector2(deltaZoom, deltaZoom)).Clamp(MinZoom, MaxZoom);

        // Сдвигаем камеру, чтобы точка под курсором осталась на месте
        Position += worldMouseBefore - GetGlobalMousePosition();
    }
}