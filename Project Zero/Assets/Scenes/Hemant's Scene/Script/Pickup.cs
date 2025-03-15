using UnityEngine;

public class PickUp : MonoBehaviour
{

    [SerializeField] Gunstats gun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gun.AmmoCur = gun.AmmoMax;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        IPickup pick = other.GetComponent<IPickup>();
        if (pick != null)
        {
            pick.getgunstats(gun);
            Destroy(gameObject);
        }
    }
}
