using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// базовый класс для обработчиков клеток (по совету грока данные передаются через CellData, а не храняться напрямую в объектах класса)
/// </summary>
public abstract class Cell
{
    public CellAction[] cellActions;
    protected Vector2I _textureCord = new Vector2I(0,0);

    /// <summary>
    /// что делает клетка после хода
    /// </summary>
    /// <param name="self"> состояние клетки</param>
    /// <param name="neighbors"> соседи (первые 4 по сторонам, вторые 4 по диагонали)</param>
    /// <param name="position">корды на всякий</param>
    /// <param name="grid">состояние всего грида на всякий</param>
    /// <returns>(новое состояние клетки, очки)</returns>
    public abstract (CellData newState, int scoreDelta) Execute(CellData self,
        ReadOnlySpan<CellData> neighbors,
        Vector2I position,
        GridData grid
    );

    /// <summary>
    /// возвращает корды текстуры (assets/tilesets/basictileset)
    /// </summary>
    /// <param name="data"></param>
    /// <returns>возвращает корды текстуры (assets/tilesets/basictileset)</returns>
    public virtual Vector2I GetTextureCord(CellData data)
    {
        return _textureCord;
    }

    /// <summary>
    /// как клетку можно изменить в данных условиях
    /// </summary>
    /// <param name="self"></param>
    /// <param name="pos"></param>
    /// <param name="grid"></param>
    /// <returns>все доступные действия</returns>
    public virtual CellAction[] GetAvailableActions(CellData self, Vector2I pos, GridData grid)
    {
        List<CellAction> actions = new List<CellAction>();

        foreach(CellAction action in cellActions)
        {
            if(action.CanExecute(self,pos,grid))
            {
                actions.Add(action);
            }
        }
        return actions.ToArray();
    }
}