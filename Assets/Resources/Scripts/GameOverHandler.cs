using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    public float FadeDuration = 1f;
    public Text GameOverText;
    public Text GO_ScoreText;
    public Image Fade;
    public Button restartButton;
    bool GameOver = false;
   
    void Start()
    {
        GameOverText.gameObject.SetActive(false);
        Fade.gameObject.SetActive(false);
        GO_ScoreText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        //Enable movement for all players
        foreach (Player player in GameObject.FindObjectsOfType<Player>().ToList())
        {
            player.MovementLocked = false;
        }
    }
 
    void Update()
    {

    }
   
    public void DoGameOver()
    {
        if (!GameOver)
        {
            GameOver = true;
            StartCoroutine(FadeToBlack(FadeDuration));

            GO_ScoreText.text = GameObject.FindObjectOfType<StarsManager>().OrdersCompleteText.text; //Copy score text from main game

            //Stop all players from moving
            foreach (Player player in GameObject.FindObjectsOfType<Player>().ToList())
            {
                player.MovementLocked = true;
            }     
        }
    }

    IEnumerator FadeToBlack(float fadeDuration)
    {
        float fadeElapsed = 0;
        Fade.color = new Color(0, 0, 0, 0);
        Fade.gameObject.SetActive(true);

        do
        {
            fadeElapsed += Time.deltaTime;
            Fade.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, fadeElapsed / fadeDuration);
            yield return null;
        }
        while (fadeElapsed / fadeDuration < 1f);

        GameOverText.gameObject.SetActive(true);
        GO_ScoreText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }
}
