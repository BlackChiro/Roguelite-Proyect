using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Exit_Object : Cell_Object
{
    public Tile EndTile;
    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        Game_Manager.Instance.board_manager.SetCellTile(coord,EndTile);
    }
    public override void PlayerEntered()
    {
        Game_Manager.Instance.NewLevel();
    }
}
