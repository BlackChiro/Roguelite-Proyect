using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Object : Cell_Object
{
    public int enemyHP = 3;

    private int m_CurrentHealth;

    private void Awake()
    {
        Game_Manager.Instance.turn_manager.onTick += EnemyTurnHappen;
    }

    private void OnDestroy()
    {
        Game_Manager.Instance.turn_manager.onTick -= EnemyTurnHappen;
    }

    public override void Init(Vector2Int coord)
    {
        base.Init(coord);
        m_CurrentHealth = enemyHP;
    }
    public override bool PlayerWantToEnter()
    {
        m_CurrentHealth--;
        
        if (m_CurrentHealth < 0)
        {
            Destroy(gameObject);
        }
        Game_Manager.Instance.player_controller.animator.SetTrigger("mining");
        return false;
    }
    bool MoveTo(Vector2Int coord)
    {
        var board = Game_Manager.Instance.board_manager;
        var targetCell = board.GetCelldata(coord);
        if(targetCell == null
            ||!targetCell.passable
            ||targetCell.ContainedObject != null)
        {
            return false;
        }
        var currentCell = board.GetCelldata(m_Cell);
        currentCell.ContainedObject = null;

        targetCell.ContainedObject = this;
        m_Cell = coord;
        transform.position = board.CellToWorld(coord);

        return true;
    }

    void EnemyTurnHappen()
    {
        var playerCell = Game_Manager.Instance.player_controller.CellPosition;

        int xDist = playerCell.x - m_Cell.x;
        int yDist = playerCell.y - m_Cell.y;

        int absXDist = Mathf.Abs(xDist);
        int absYDist = Mathf.Abs(yDist);

        if (xDist == 0 && absYDist == 1 || yDist == 0 && absXDist == 1)
        {
            Game_Manager.Instance.ChangeFood(-3);
            Game_Manager.Instance.player_controller.animator.SetTrigger("dmg");
        }
        else
        {
            if (absXDist > absYDist)
            {
                if (!TryMoveIntX(xDist))
                {
                    TryMoveIntY(yDist);
                }
            }
            else
            {
                if (!TryMoveIntY(yDist))
                {
                    TryMoveIntX(xDist);
                }
            }
        }
    }
    bool TryMoveIntX (int xDist)
    {
        if (xDist > 0)
        {
            return MoveTo(m_Cell + Vector2Int.right);
        }
        return MoveTo(m_Cell + Vector2Int.left);
    }
    bool TryMoveIntY(int yDist)
    {
        if (yDist > 0)
        {
            return MoveTo(m_Cell + Vector2Int.up);
        }
        return MoveTo(m_Cell + Vector2Int.down);
    }

}
