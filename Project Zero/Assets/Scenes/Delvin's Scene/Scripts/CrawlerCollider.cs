using UnityEngine;

public class CrawlerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            if (GameManager.instance.player != null)
            {
                GameManager.instance.playerScript.TakeDamage(GameManager.instance.crawlerEnemy.damageAmout, 0f, 0f); // Use crawler's damage value
              
            }
        }
    }
}
