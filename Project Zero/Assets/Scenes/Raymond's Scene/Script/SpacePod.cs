using UnityEngine;

public class SpacePod : MonoBehaviour
{
    [SerializeField] int requiredParts = 5;
    

    int insertedParts = 0;
    bool isFixed = false;

    private void Start()
    {
        gameObject.tag = "SpacePod"; 
    }

    public void InsertPart()
    {
        if (isFixed) return;

        insertedParts++;
        Debug.Log($"Parts inserted: {insertedParts}/{requiredParts}");

        if (insertedParts >= requiredParts)
        {
            FixPod();
        }
    }

    private void FixPod()
    {
        isFixed = true;
        Debug.Log("SpacePod fixed!");
    }

    public bool IsFixed()
    {
        return isFixed;
    }
}
