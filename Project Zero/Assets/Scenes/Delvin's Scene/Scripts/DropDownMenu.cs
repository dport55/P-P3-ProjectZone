using UnityEngine;

public class DropDownMenu : MonoBehaviour
{
    
    public BossCollider[] bossCollider;
    public CrawlerCollider[] crawlerCollider;
    public BossEnemy[] bossEnemy; 
    public CrawlerEnemy[] crawlerEnemy;

    public void ChangeDifficulty(int index)
    {
        switch (index) 
        {    
            case 0:
                foreach (BossCollider boss in bossCollider)
                { 
                    GameManager.instance.bossEnemy.Damage(.5f);
                    
                }
               foreach (CrawlerCollider crawler in crawlerCollider)
                {
                    GameManager.instance.crawlerEnemy.Damage(.25f);
                }
               foreach (BossEnemy boss in bossEnemy)
                {
                    GameManager.instance.bossEnemy.HP = 5;
                }
                foreach (CrawlerEnemy crawler in crawlerEnemy)
                {
                    GameManager.instance.crawlerEnemy.HP = 2;
                }
             
                Debug.Log("Index 0 selected");
                    
                break;
            case 1:
                foreach (BossCollider boss in bossCollider)
                {
                    GameManager.instance.bossEnemy.Damage(1f);

                }
                foreach (CrawlerCollider crawler in crawlerCollider)
                {
                    GameManager.instance.crawlerEnemy.Damage(.5f);
                }
                foreach (BossEnemy boss in bossEnemy)
                {
                    GameManager.instance.bossEnemy.HP = 10;
                }
                foreach (CrawlerEnemy crawler in crawlerEnemy)
                {
                    GameManager.instance.crawlerEnemy.HP = 5;
                }
                Debug.Log("Index 1 selected");
                break;
            case 2:
                foreach (BossCollider boss in bossCollider)
                {
                    GameManager.instance.bossEnemy.Damage(2f);

                }
                foreach (CrawlerCollider crawler in crawlerCollider)
                {
                    GameManager.instance.crawlerEnemy.Damage(1f);
                }
                foreach (BossEnemy boss in bossEnemy)
                {
                    GameManager.instance.bossEnemy.HP = 15;
                }
                foreach (CrawlerEnemy crawler in crawlerEnemy)
                {
                    GameManager.instance.crawlerEnemy.HP = 8;
                }
                Debug.Log("Index 2 selected");
                break;
            default:
                Debug.Log("Default index selected");
                break;
        }
 
    }
}
