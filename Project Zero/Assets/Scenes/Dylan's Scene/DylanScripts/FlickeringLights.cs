using System.Collections;
using UnityEngine;

public class FlickeringLights : MonoBehaviour
{
    public Light flickeringLight;  // Assign your spotlight here
    public float minIntensity; // Minimum light intensity
    public float maxIntensity; // Maximum light intensity
    public float flickerSpeed = 0.1f; // Speed of flickering

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (flickeringLight == null)
            flickeringLight = GetComponent<Light>();

        StartCoroutine(Flicker());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            flickeringLight.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f));
        }
    }
}
