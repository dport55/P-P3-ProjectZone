using UnityEngine;

public class PartsPickup : MonoBehaviour
{
    [SerializeField] GameObject part;
    public int collectedParts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        IPickup playerController = GetComponent<IPickup>();
    //        playerController.CollectPart();
    //        Destroy(gameObject);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        IPickup pick = other.GetComponent<IPickup>();
        if (pick != null)
        {
            pick.getParts(part);
            Destroy(gameObject);
        }
    }
}
