using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Destructible : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject destroy;

    public void Destroy()
    {
        Instantiate(destroy, transform.position, transform.rotation);
        Destroy(destroy);
    }
}
