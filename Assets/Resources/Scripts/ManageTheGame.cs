using UnityEngine;
using System.Collections;

public class ManageTheGame : MonoBehaviour {

    public const int MAX_PLAYERS = 5;
    [HideInInspector]
    public GameObject[] ActivePlayers;

    public GameObject PlayerProto;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

	// Use this for initialization
	void Start () {
        ActivePlayers = new GameObject[MAX_PLAYERS];
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            var pNum = i + 1;

            if (Input.GetButtonDown("Interact_P" + pNum))
                Debug.Log( " " + pNum);

            if (Input.GetButtonDown("Interact_P" + pNum) && ActivePlayers[i] == null)
            {
                ActivePlayers[i] = (GameObject)Instantiate(PlayerProto, new Vector3(((int)((i + 1) / 2) * 2) * Mathf.Pow(-1, (i + 1)), 1, 0), Quaternion.identity);
                var player = ActivePlayers[i].GetComponent<Player>();
                player.HorizontalAxis = "Horizontal_P" + pNum;
                player.VerticalAxis = "Vertical_P" + pNum;
                player.InteractButtonAxis = "Interact_P" + pNum;
            }
        }
	}
}
