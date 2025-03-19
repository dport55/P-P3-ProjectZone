//using System.Collections;
//using UnityEngine;

//public class Doors : MonoBehaviour
//{
//    [SerializeField] Transform doorPos;
//    [SerializeField] float speed;
//    [SerializeField] bool keycardDoor; // For keycard implementation later
//    [SerializeField] float doorInteractionDist;
//    [SerializeField] GameObject doorPrompt;

//    // Positions
//    //private Vector3 openPos;
//    //private Vector3 closedPos;

//    public GameObject openPos;
//    public GameObject closedPos;

//    private bool doorIsOpen = false;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        //closedPos = transform.position;
//        //openPos = closedPos + new Vector3(0, 3f, 0);
//        doorPrompt.SetActive(false);
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    public void ToggleDoor()
//    {
//        if(openPos)
//        {
//            closedPos.SetActive(true);
//            openPos.SetActive(false);
//        }   
//        else 
//        {
//            closedPos.SetActive(false);
//            openPos.SetActive(true);
//        }

//        doorIsOpen = false;
//    }

//    //private void OnTriggerEnter(Collider other)
//    //{ 

//    //    if (other.CompareTag("Door"))
//    //    {
//    //        doorPrompt.SetActive(true);
//    //        if (Input.GetKeyDown(KeyCode.E)) 
//    //        {
//    //            ToggleDoor(); 
//    //        }

//    //    }
//    //}


//    //bool IsLookingAtDoor()
//    //{
//    //    Camera playerCamera = GameManager.instance.playerScript.Camera.main;
//    //    RaycastHit hit;
//    //    if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, doorInteractionDist))
//    //    {
//    //        return hit.collider.gameObject == gameObject;
//    //    }
//    //    return false;
//    //}
//}

using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject door;
    public float openHeight = 3f;
    public float openSpeed = 2f;
    public string requiredKeyID;

    public float closeHeight = 0f;
    public float closeSpeed = 2f;

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private Vector3 targetPosition2;
    private bool isOpening = false;

    void Start()
    {
        initialPosition = door.transform.position;
        targetPosition = initialPosition + new Vector3(0, openHeight, 0);
        targetPosition2 = initialPosition + new Vector3(0, closeHeight, 0);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpening)
        {
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>(); // Get the PlayerInventory component
            if (playerInventory != null && playerInventory.HasKey(requiredKeyID)) // Check if player has the key
            {
                isOpening = true;
                StartCoroutine(OpenDoor());
            }
        }
    }


    System.Collections.IEnumerator OpenDoor()
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * openSpeed;
            door.transform.position = Vector3.Lerp(initialPosition, targetPosition, time);
            yield return null;
        }

        door.transform.position = targetPosition;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isOpening)
        {
            isOpening = false;
            StartCoroutine(CloseDoor());
        }
    }


    System.Collections.IEnumerator CloseDoor()
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime * closeSpeed;
            door.transform.position = Vector3.Lerp(initialPosition, targetPosition2, time);
            yield return null;
        }

        door.transform.position = targetPosition2;
    }
}