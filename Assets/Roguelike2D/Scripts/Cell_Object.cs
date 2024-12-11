using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell_Object : MonoBehaviour
{
    protected Vector2Int m_Cell;

    public virtual void Init(Vector2Int cell)
    {
        m_Cell = cell;
    }
    public virtual void PlayerEntered()
    {
        
    }
    public virtual bool PlayerWantToEnter()
    {
        return true;
    }
}