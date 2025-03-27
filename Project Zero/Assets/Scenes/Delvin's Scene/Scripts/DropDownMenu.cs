using UnityEngine;

public class DropDownMenu : MonoBehaviour
{
    public PlayerController player;
    

    public void ChangeDifficulty(int index)
    {
        switch (index) 
        {    
            case 0:

                player.HP = 10;
                player.shootDamage = 3;
                Debug.Log("Index 0 selected");
                    
                break;
            case 1:
                player.HP = 8;
                player.shootDamage = 2;
                Debug.Log("Index 1 selected");
                break;
            case 2:
                player.HP = 6;
                player.shootDamage = 1;
                Debug.Log("Index 2 selected");
                break;
            default:
                Debug.Log("Default index selected");
                break;
        }
 
    }
}
