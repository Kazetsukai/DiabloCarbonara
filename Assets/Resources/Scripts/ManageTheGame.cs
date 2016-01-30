using UnityEngine;
using System.Collections;

public class ManageTheGame : MonoBehaviour {

    public const int MAX_PLAYERS = 4;
    public GameObject[] ActivePlayers = new GameObject[MAX_PLAYERS];

    public GameObject PlayerProto;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            var pNum = i + 1;
            if (Input.GetButtonDown("Interact_P" + pNum) && ActivePlayers[i] == null)
                ActivePlayers[i] = Instantiate(PlayerProto);

        }
	}
}
