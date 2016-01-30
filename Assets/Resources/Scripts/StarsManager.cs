using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StarsManager : MonoBehaviour
{
    public int StarsRemaining = 5;
    public Image[] Stars;

    public Text OrdersCompleteText;
    public int OrdersCompleted = 0;

    void Start()
    {

    }

    public void ResetStars()
    {
        StarsRemaining = 5;
    }
    
    void Update()
    {
        if (StarsRemaining <= 0)
        {
            //Do game over screen
            GameObject.FindObjectOfType<GameOverHandler>().DoGameOver();
        }

        //Update stars UI images
        for(int i = 0; i < Stars.Length; i++)
        {
            if (i + 1 > StarsRemaining)
            {
                Stars[i].gameObject.SetActive(false);
            }
            else
            {
                Stars[i].gameObject.SetActive(true);
            }
        }

        //Update score text
        OrdersCompleteText.text = "Orders Completed: " + OrdersCompleted;
    }
}
