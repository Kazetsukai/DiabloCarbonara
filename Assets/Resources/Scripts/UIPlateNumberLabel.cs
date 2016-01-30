using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class UIPlateNumberLabel : MonoBehaviour
{
    public int PlateNumber;
    public Image TimerImage;

    PlateBench plateBench;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //Get platebench
        if (plateBench == null)
        {
            plateBench = GameObject.FindObjectsOfType<PlateBench>().Where(p => p.PlateNumber == PlateNumber).ToList()[0];
            TimerImage.fillAmount = 1f;
        }
        else
        {
            //Update timer image
            if (plateBench.Recipe != null)
            {
                TimerImage.fillAmount = 1f - plateBench.Recipe.TimeElapsed / plateBench.Recipe.MaxTime;
            }
        }
    }
}
