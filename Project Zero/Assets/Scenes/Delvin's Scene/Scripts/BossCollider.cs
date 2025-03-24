using UnityEngine;

public class BossCollider : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
         
            if (GameManager.instance.player != null)
            {
                GameManager.instance.playerScript.TakeDamage(GameManager.instance.bossEnemy.damageAmout,0f,0f); // Use Boss's damage value
                Debug.Log("Player took " + GameManager.instance.bossEnemy + " damage from the boss!" + GameManager.instance.bossEnemy.damageAmout);
            }
        }
    }
}
