using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RitualMaster : MonoBehaviour {

    public SpriteRenderer Demon;
    public Text DemonText;
    public RitualBase[] RitualProtos;

    bool _demonvisible = false;
    float _opacity = 0;

    public Player UnluckyPlayer;
    public RitualBase[] CurrentRitual;


    // Use this for initialization
    void Start () {
        _demonvisible = true;
        
	}
	
	// Update is called once per frame
	void Update () {
	    if (_demonvisible)
        {
            if (_opacity < 1) _opacity += Time.deltaTime;
        }
        else
        {
            if (_opacity > 0) _opacity -= Time.deltaTime;
        }
        _opacity = Mathf.Clamp(_opacity, 0, 1);

        var colour = Demon.color;
        colour.a = _opacity;
        Demon.color = colour;




        // Debug
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TriggerRitual();
        }
	}

    private void TriggerRitual()
    {
        // Pick unlucky victim
        var players = FindObjectsOfType<Player>();
        UnluckyPlayer = players[Random.Range(0, players.Length)];
        
        // Clean up old rituals
        foreach (var ritual in CurrentRitual)
        {
            Destroy(ritual);
        }

        // Come up with a ritual
        var ritualLength = 3;
        CurrentRitual = new RitualBase[ritualLength];
        for (int i = 0; i < ritualLength; i++)
        {
            CurrentRitual[i] = Instantiate(RitualProtos[Random.Range(0, RitualProtos.Length)]);
            CurrentRitual[i].Randomise();
        }


        // Choose a punishment

        // Change up the music

        _demonvisible = true;
    }

    public IEnumerable<T> RemainingRituals<T>() where T : RitualBase
    {
        return CurrentRitual.Where(r => !r.Satisfied).OfType<T>();
    }
}
