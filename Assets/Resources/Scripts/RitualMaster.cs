using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RitualMaster : MonoBehaviour {

    public Image Demon;
    public Text DemonText;
    public Image DemonPanel;
    public RitualBase[] RitualProtos;
    public PunishmentBase[] PunishmentProtos;

    bool _demonvisible = false;
    float _opacity = 0;

    public Player UnluckyPlayer;
    public RitualBase[] CurrentRitual;
    private PunishmentBase _punishment;
    private int _numRemaining;
    private MusicMaster _musicMaster;

    public bool RitualInProgress;

    // Use this for initialization
    void Start ()
    {
        _musicMaster = FindObjectOfType<MusicMaster>();
    }
	
	// Update is called once per frame
	void Update () {
	    if (_demonvisible)
        {
            if (_opacity < 2) _opacity += Time.deltaTime;
        }
        else
        {
            if (_opacity > 0) _opacity -= Time.deltaTime;
        }
        var opac = Mathf.Clamp(_opacity, 0, 1);

        var colour = Demon.color;
        colour.a = opac;
        Demon.color = colour;
    
        var panelColour = DemonPanel.color;
        panelColour.a = opac * 0.7f;
        DemonPanel.color = panelColour;

        var textColour = DemonText.color;
        textColour.a = opac;
        DemonText.color = textColour;
        
        if (_numRemaining != RemainingRituals<RitualBase>().Count())
        {
            UpdateRitual();
        }

        // Debug
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TriggerRitual();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            FinishRitual(false);
        }
    }

    private void UpdateRitual()
    {
        _numRemaining = RemainingRituals<RitualBase>().Count();

        if (_numRemaining > 0)
        {
            if (RitualInProgress)
            {
                var descs = RemainingRituals<RitualBase>().Select(c => c.Description());
                descs = descs
                    .Take(descs.Count() - 1)
                    .Select(d => d += ", ")
                    .Concat(new[] { (_numRemaining > 0 ? "and " : "") + descs.Last() });
                DemonText.text = "I command you to " + string.Join("", descs.ToArray()) + " or I will " + _punishment.Description() + ".";
            }
        }
        else
        {
            FinishRitual();           
        }
    }
    
    public void FinishRitual(bool good = true)
    {
        if (RitualInProgress)
        {
            RitualInProgress = false;
            _musicMaster.TransitionMusic(1);

            if (!good)
                _punishment.ExecutePunishment();

            DemonText.text = good ? "Excellent work team!" : "I'm disappointed in you...";
            _demonvisible = false;
        }
    }

    public void TriggerRitual()
    {
        if (!RitualInProgress)
        {
            RitualInProgress = true;

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
            _punishment = Instantiate(PunishmentProtos[Random.Range(0, PunishmentProtos.Length)]);

            // Change up the music
            _musicMaster.TransitionMusic(2);

            _demonvisible = true;

            UpdateRitual();
        }
    }

    public IEnumerable<T> RemainingRituals<T>() where T : RitualBase
    {
        return CurrentRitual.Where(r => !r.Satisfied).OfType<T>();
    }
}
