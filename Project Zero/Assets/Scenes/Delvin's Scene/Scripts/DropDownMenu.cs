using UnityEngine;

public class DropDownMenu : MonoBehaviour
{
    
    public BossCollider bossCollider;
    public CrawlerCollider crawlerCollider;

    public void ChangeDifficulty(int index)
    {
        switch (index) 
        {    
            case 0:
                GameManager.instance.bossEnemy.Damage(.5f);
                GameManager.instance.crawlerEnemy.Damage(.25f);
                Debug.Log("Index 0 selected");
                    
                break;
            case 1:
                GameManager.instance.bossEnemy.Damage(1f);
                GameManager.instance.crawlerEnemy.Damage(.5f);
                Debug.Log("Index 1 selected");
                break;
            case 2:
                GameManager.instance.bossEnemy.Damage(2f);
                GameManager.instance.crawlerEnemy.Damage(1f);
                Debug.Log("Index 2 selected");
                break;
            default:
                Debug.Log("Default index selected");
                break;
        }
 
    }
}
