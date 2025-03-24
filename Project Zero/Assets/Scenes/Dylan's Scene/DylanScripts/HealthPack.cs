using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] GameObject HPPack;
    [SerializeField] int healAmount = 11;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.getHealth(healAmount);
            Destroy(gameObject);
        }
    }
}
