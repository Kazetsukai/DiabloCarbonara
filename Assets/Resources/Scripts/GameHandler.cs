using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public float FadeDuration = 1f;
    public Text GameOverText;
    public Text GO_ScoreText;
    public Image Fade;
    public Button restartButton;
    bool GameOver = false;

    [Header("Rituals")]
    [SerializeField] GameObject RitualTimerObj;
    [SerializeField] Image RitualTimerFill;
    [SerializeField] bool RitualInProgress;
    [SerializeField] float CurrentRitualElapsed;
    [SerializeField] float CurrentRitualDuration;

    [SerializeField] float MinTimeBetweenRituals = 20f;
    [SerializeField] float MaxTimeBetweenRituals = 40f;

    float timeSinceLastRitual;
    float nextRitualTime;
    
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

        //Generate next ritual time
        nextRitualTime = Random.Range(MinTimeBetweenRituals, MaxTimeBetweenRituals);
    }
 
    void Update()
    {
        if (RitualInProgress)
        {
            RitualTimerObj.gameObject.SetActive(true);
            CurrentRitualElapsed += Time.deltaTime;

            RitualTimerFill.fillAmount = 1f - (CurrentRitualElapsed / CurrentRitualDuration);

            if (CurrentRitualDuration >= CurrentRitualElapsed)
            {
                //Ritual was not completed by players! Ritual is failed. Punishment!
                GameObject.FindObjectOfType<RitualMaster>().FinishRitual(false);
                RitualInProgress = false;

                //Calculate time to next ritual
                nextRitualTime = Random.Range(MinTimeBetweenRituals, MaxTimeBetweenRituals);
            }
        }       
        else
        {
            RitualTimerObj.gameObject.SetActive(false);

            //Check if it is time to start a new ritual
            timeSinceLastRitual += Time.deltaTime;
            if (timeSinceLastRitual >= nextRitualTime)
            {
                timeSinceLastRitual = 0;
                GameObject.FindObjectOfType<RitualMaster>().TriggerRitual();
                RitualInProgress = true;
            }
        }
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
