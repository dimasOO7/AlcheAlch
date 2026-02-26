using Godot;
using System;
using System.Collections.Generic;

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

    public virtual Vector2I GetTextureCord(CellData data) //возвращает корды текстуры (assets/tilesets/basictileset)
    {
        return _textureCord;
    }


    public virtual CellAction[] GetAvailableActions(CellData self, Vector2I pos, GridData grid) //как клетку изменить можно
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