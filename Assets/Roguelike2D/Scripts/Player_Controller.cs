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
    public bool IsMoving = false;
    private Vector3 m_MoveTarget;
    public float MoveSpeed = 5f;

    private Animator m_animator;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();  
    }
    public void init()
    {
        Game_Manager.Instance.GameOver = false;
        IsMoving = false;
    }

    public void Spawn(Board_Manager board_Manager, Vector2Int cell)
    {
        m_Board_Manager = board_Manager;
        MoveTo(cell,true);
    }

    public void MoveTo (Vector2Int cell,bool inmediate)
    {
        m_CellPosition = cell;
        if (inmediate)
        {
            IsMoving = false;
            transform.position = m_Board_Manager.CellToWorld(m_CellPosition);
        }
        else
        {
            IsMoving = true;
            m_MoveTarget = m_Board_Manager.CellToWorld(m_CellPosition);
        }
        m_animator.SetBool("moving", IsMoving);
    }
   
    
    private void Update()
    {
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;
        if (Game_Manager.Instance.GameOver == true)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                Game_Manager.Instance.StartNewGame();
            }
        }

        if (IsMoving == true)
        {
            transform.position =Vector3.MoveTowards(transform.position, m_MoveTarget, MoveSpeed * Time.deltaTime);
            if (transform.position == m_MoveTarget) 
            {
                IsMoving = false;
                m_animator.SetBool("Moving",false);
                var cellData = m_Board_Manager.GetCelldata(m_CellPosition);
                if (cellData.ContainedObject != null) cellData.ContainedObject.PlayerEntered();
            }
            return;
        }

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

                    Game_Manager.Instance.turn_manager.Tick();

                    if (cellData.ContainedObject == null)
                    {
                        MoveTo(newCellTarget,false);
                        
                    }
                    else if (cellData.ContainedObject.PlayerWantToEnter())
                    {
                        MoveTo(newCellTarget,false);
                    }
                }
            }
        }
    }
}
