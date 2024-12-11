using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;


public class Player_Controller : MonoBehaviour

{
    private Board_Manager m_Board_Manager;
    private Vector2Int m_CellPosition;
    private Grid m_grid;
    

    public void Spawn(Board_Manager board_Manager, Vector2Int cell)
    {
        m_Board_Manager = board_Manager;
        MoveTo(cell);
    }

    public void MoveTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        transform.position = m_Board_Manager.CellToWorld(m_CellPosition);
    }
    
    
    private void Update()
    {
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if (Game_Manager.Instance.GameOver == false)
        {

            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                newCellTarget.y += 1;
                hasMoved = true;
            }
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                newCellTarget.y -= 1;
                hasMoved = true;
            }
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                newCellTarget.x += 1;
                hasMoved = true;
            }
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                newCellTarget.x -= 1;
                hasMoved = true;
            }
            if (hasMoved)
            {
                Board_Manager.CellData cellData = m_Board_Manager.GetCelldata(newCellTarget);
                if (cellData != null && cellData.passable)
                {
                    //MoveTo(newCellTarget);
                    Game_Manager.Instance.turn_manager.Tick();
                    if (cellData.ContainedObject == null)
                    {
                        MoveTo(newCellTarget);
                        
                    }
                    else if (cellData.ContainedObject.PlayerWantToEnter())
                    {
                        MoveTo(newCellTarget);
                        cellData.ContainedObject.PlayerEntered();
                    }
                    if (Game_Manager.Instance.m_comida >= 100)
                    {
                        Game_Manager.Instance.m_comida = 100;
                    }
                }
            }
        }
    }
}
