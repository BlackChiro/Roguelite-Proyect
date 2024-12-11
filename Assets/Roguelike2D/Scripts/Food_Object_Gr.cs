using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food_Object_Gr : Cell_Object
{
    public int HealValue = 21;
    public override void PlayerEntered()
    {
        Destroy(gameObject);
        Game_Manager.Instance.ChangeFood(HealValue);

        Debug.Log("Ñam Ñam ñam");
    }
}
