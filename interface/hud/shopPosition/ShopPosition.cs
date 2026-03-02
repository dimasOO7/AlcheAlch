using Godot;

/// <summary>
/// 1 позиция в магазе
/// </summary>
public partial class ShopPosition : Control
{
    public CellAction action;

    [Export] TileMapLayer icon;
    [Export] Button button;

    [Export] Color costColor;
    [Export] Color costColorText;
    [Export] Color negativeCostColor;
    [Export] Color negativeCostColorText;
    [Export] Color inactiveCostColorText;
    [Export] Color inactiveColor;

    public void Initialize(CellAction action, Shop shop, bool active)
    {
        this.action = action;

        // Текст цены (уже было)
        button.Text = $"{(action.Cost >= 0 ? "" : "+")}{Mathf.Abs(action.Cost)}";

        // Иконка (уже было)
        icon.SetCell(Vector2I.Zero, 0, action.IconCoord);

        Color bgColor;
        Color textColor;

        if (active)
        {
            // === АКТИВНАЯ КНОПКА ===
            if (action.Cost >= 0)
            {
                bgColor   = costColor;
                textColor = costColorText;
            }
            else // цена < 0
            {
                bgColor   = negativeCostColor;
                textColor = negativeCostColorText;
            }

            button.Disabled = false;
            button.Pressed += () => shop.OnButtonPressed(action); // привязываем событие ТОЛЬКО когда активна
        }
        else
        {
            // === НЕАКТИВНАЯ КНОПКА ===
            bgColor   = inactiveColor;
            textColor = inactiveCostColorText;

            button.Disabled = true;
            // событие Pressed НЕ привязываем — как ты просил
        }

        // Применяем цвета
        button.SelfModulate = bgColor;                                   // цвет фона кнопки
        button.AddThemeColorOverride("font_color", textColor);           // обычный текст
        button.AddThemeColorOverride("font_disabled_color", textColor);  // текст когда Disabled
    }
}