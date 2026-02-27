using Godot;
using System;

/// <summary>
/// индикатор очков
/// </summary>
public partial class ScoreLabel : Label
{
    [Export] public Color positiveColor;
    [Export] public Color negativeColor;
    [Export] public Color neutralColor;
    public override void _Ready()
    {
        base._Ready();
        (GetTree().CurrentScene as GameManager).ScoreChangedSignal +=  ScoreChanged;
    }

    public void ScoreChanged(int score, int delta)
    {
        string newText = $"очки: {score} ";
        if(delta != 0)
        {
            if(delta > 0)
            {
                newText += $"+{delta}";
                LabelSettings.FontColor = positiveColor;
            }
            if(delta < 0)
            {
                newText += $"-{Math.Abs(delta)}";
                LabelSettings.FontColor = negativeColor;
            }
        }
        else
        {
            LabelSettings.FontColor = neutralColor;
        }
        Text = newText;
    }
}
