using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    [SerializeField] float CurrentRitualElapsed;
    [SerializeField] float CurrentRitualDuration;

    [SerializeField] float MinTimeBetweenRituals = 20f;
    [SerializeField] float MaxTimeBetweenRituals = 40f; 

    [SerializeField] float timeSinceLastRitual;
    [SerializeField] float nextRitualTime;

    [SerializeField] List<Light> LevelLights;
    [SerializeField] Color NormalLightColor;
    [SerializeField] Color RitualLightColor;
    [SerializeField] float RitualFadeDuration = 1f;
    bool fadingToNormal;
    bool fadingToRitual;

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
        var RitualInProgress = GameObject.FindObjectOfType<RitualMaster>().RitualInProgress;
        if (RitualInProgress)
        {
            if (!fadingToRitual)
            {
                fadingToRitual = true;

                //Turn lights back to ritual mode
                StartCoroutine(FadeLightsToRitual());
            }

            RitualTimerObj.gameObject.SetActive(true);
            CurrentRitualElapsed += Time.deltaTime;

            RitualTimerFill.fillAmount = 1f - (CurrentRitualElapsed / CurrentRitualDuration);

            if (CurrentRitualElapsed >= CurrentRitualDuration)
            {
                //Ritual was not completed by players! Ritual is failed. Punishment!
                GameObject.FindObjectOfType<RitualMaster>().FinishRitual(false);
             
                //Calculate time to next ritual
                nextRitualTime = Random.Range(MinTimeBetweenRituals, MaxTimeBetweenRituals);                
            }
        }       
        else
        {
            fadingToRitual = false;
            RitualTimerObj.gameObject.SetActive(false);

            if (!fadingToNormal)
            {
                fadingToNormal = true;

                //Turn lights back to ritual mode
                StartCoroutine(FadeLightsToNormal());
            }

            //Check if it is time to start a new ritual
            timeSinceLastRitual += Time.deltaTime;
            if (timeSinceLastRitual >= nextRitualTime)
            {
                timeSinceLastRitual = 0;
                CurrentRitualElapsed = 0;
                GameObject.FindObjectOfType<RitualMaster>().TriggerRitual();
                fadingToNormal = false;
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


    IEnumerator FadeLightsToRitual()
    {
        float fadeElapsed = 0;       

        do
        {
            fadeElapsed += Time.deltaTime;

            foreach (Light l in LevelLights)
            {
                l.color = Color.Lerp(NormalLightColor, RitualLightColor, fadeElapsed / RitualFadeDuration);
            }          
            yield return null;
        }
        while (fadeElapsed / RitualFadeDuration < 1f);
    }

    IEnumerator FadeLightsToNormal()
    {
        float fadeElapsed = 0;

        do
        {
            fadeElapsed += Time.deltaTime;

            foreach (Light l in LevelLights)
            {
                l.color = Color.Lerp(RitualLightColor, NormalLightColor, fadeElapsed / RitualFadeDuration);
            }
            yield return null;
        }
        while (fadeElapsed / RitualFadeDuration < 1f);        
    }

}
