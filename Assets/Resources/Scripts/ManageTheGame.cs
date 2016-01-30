using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD.Studio;

public class ManageTheGame : MonoBehaviour
{

    public const int MAX_PLAYERS = 5;
    [HideInInspector]
    public GameObject[] ActivePlayers;

    public Color[] PlayerColors;

    public GameObject PlayerProto;

    public Text StartingText;

    bool _beginning = false;
    bool _playing = false;
    
    [FMODUnity.EventRef]
    public string eventName = "event:/Italian_Kitchen";
    private EventInstance _musicEvent;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start()
    {
        ActivePlayers = new GameObject[MAX_PLAYERS];
        _musicEvent = FMODUnity.RuntimeManager.CreateInstance(eventName);
        _musicEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
        _musicEvent.start();

        // Intro music
        _musicEvent.setParameterValue("Parameter 1", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playing)
        {
            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                var pNum = i + 1;

                if (Input.GetButtonDown("Interact_P" + pNum))
                    Debug.Log(" " + pNum);

                if (Input.GetButtonDown("Interact_P" + pNum))
                {
                    if (ActivePlayers[i] == null)
                    {
                        ActivePlayers[i] = (GameObject)Instantiate(PlayerProto, new Vector3(((int)((i + 1) / 2) * 2) * Mathf.Pow(-1, (i + 1)), 1, 0), Quaternion.identity);
                        DontDestroyOnLoad(ActivePlayers[i]);

                        var player = ActivePlayers[i].GetComponent<Player>();
                        player.SelectColor = PlayerColors[i % PlayerColors.Length];
                        player.HorizontalAxis = "Horizontal_P" + pNum;
                        player.VerticalAxis = "Vertical_P" + pNum;
                        player.InteractButtonAxis = "Interact_P" + pNum;
                    }
                    else
                    {
                        StartCoroutine(BeginGame());
                    }
                }
            }
        }
    }

    IEnumerator BeginGame()
    {
        if (!_beginning)
        {

            _musicEvent.setParameterValue("Parameter 1", 1.0f);

            _beginning = true;

            for (int i = 0; i < 3; i++)
            {
                StartingText.text = "Beginning in " + (3 - i) + "...";
                yield return new WaitForSeconds(1);
            }

            _playing = true;
            SceneManager.LoadScene("Kitchen");
        }
    }
}
