using UnityEngine;

public class TempEnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] float HP;

    public void takeDamage(float amount)
    {
        HP -= amount;
        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       

    }
}