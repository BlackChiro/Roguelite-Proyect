using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Wall_Object : Cell_Object
{
    public Tile obstacleTile;
    public int MaxHPWall = 3;
    private int m_HPWall;
    private Tile m_OriginalTile;

    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        m_HPWall = MaxHPWall;
        m_OriginalTile = Game_Manager.Instance.board_manager.GetCellTile(cell);
        Game_Manager.Instance.board_manager.SetCellTile(cell, obstacleTile);
    }
    public override bool PlayerWantToEnter()
    {
        m_HPWall--;
        Game_Manager.Instance.player_controller.mining = true;
        if (m_HPWall > 0)
        {
            return false;
        }
        Game_Manager.Instance.board_manager.SetCellTile(m_Cell,m_OriginalTile);
        Destroy(gameObject);
        return true;
    }
}
