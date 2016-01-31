using UnityEngine;
using System.Collections;

public class ChopIndicator : MonoBehaviour
{
    public float FlipDelay = 0.3f;

    float elapsed;

    void Start()
    {

    }
    
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= FlipDelay)
        {
            elapsed = 0;
            transform.localEulerAngles += new Vector3(0, 0, 180);
        }

    }
}
