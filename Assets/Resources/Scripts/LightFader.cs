using UnityEngine;
using System.Collections;

public class LightFader : MonoBehaviour
{
    public float FadeInDuration = 0.5f;
    public float FadeOutDuration = 0.2f;
    public float MaxIntensity = 10f;

    Light light;
    bool lightIsOn;

    // Use this for initialization
    void Start()
    {
        light = GetComponent<Light>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnOn()
    {
       if (!lightIsOn)
        {
            StartCoroutine(FadeInLight());
        }
    }

    public void TurnOff()
    {
        if (lightIsOn)
        {
            StartCoroutine(FadeOutLight());
        }
    }

    IEnumerator FadeInLight()
    {
        float elapsed = 0;

        do
        {
            lightIsOn = true;
            elapsed += Time.deltaTime;
            light.intensity = (elapsed / FadeInDuration) * MaxIntensity;
            yield return null;
        }
        while (elapsed / FadeInDuration < 1f);        
    }

    IEnumerator FadeOutLight()
    {
        float elapsed = 0;

        do
        {
            elapsed += Time.deltaTime;
            light.intensity = MaxIntensity - ((elapsed / FadeInDuration) * MaxIntensity);
            yield return null;
        }
        while (elapsed / FadeInDuration < 1f);

        lightIsOn = false;
    }
}
