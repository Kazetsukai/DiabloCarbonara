using UnityEngine;
using System.Collections;

public class temp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var master = FindObjectOfType<MusicMaster>();
		master.StartIntro();
		master.TransitionMusic(1f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
