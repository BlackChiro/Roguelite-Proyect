using Unity.VisualScripting;
using UnityEngine;


public class Turn_Manager
{
    public int turno = 0;
    public event System.Action onTick;

    public void Tick()
    {
        turno++;
        onTick?.Invoke();

    }

}