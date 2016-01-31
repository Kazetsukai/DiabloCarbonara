using UnityEngine;
using System.Collections;
using FMOD.Studio;
using System;
using System.Collections.Generic;
using System.Linq;

public class MusicMaster : MonoBehaviour
{
	private EventInstance _musicEvent;
	[FMODUnity.EventRef]
	public string musicEventName = "event:/Italian_Kitchen";

	private string boilSoundNamePrefix = "event:/Boil Station/Boil_Station_";
	private string frySoundNamePrefix = "event:/Fry Station/Fry_Station_";
	private string chopSoundNamePrefix = "event:/Chop Station/Chop_Station_";
	private string binSoundNamePrefix = "event:/Bin/Bin";
	private string footstepSoundNamePrefix = "event:/Footsteps/Footstep";
	private string pickSoundNamePrefix = "event:/Ingredient/Ingredient_Grab";

	private string completeSoundName = "event:/Completion Ding/Completion_Ding";
	private string dishDoneSoundName = "event:/Dish Completion/Dish_Completion";
	private string dropSoundName = "event:/Ingredient/Ingredient_Drop";
	private string loseSoundName = "event:/Lose Star/LoseStar";
	private string bellSoundName = "event:/Order Bell/Order_Bell";

    private string demonTalkSoundName = "event:/Demon Talking/Demon_Talking";
	private string fireSoundName = "event:/Fire/Fire";
	private string ritualSuccessSoundName = "event:/Ritual/Ritual_Success";
	private string ritualFailureSoundName = "event:/Ritual/Ritual_Failure";
	private string smokeAlarmSoundName = "event:/Smoke Alarm/Smoke_Alarm";

	public Dictionary<string, string> soundNameMap;
	public List<EventInstance> currentSounds;

	// Use this for initialization
	void Start()
	{
		soundNameMap = new Dictionary<string, string>();
		FillMap("boil", boilSoundNamePrefix, 3);
		FillMap("fry", frySoundNamePrefix, 3);
		FillMap("chop", chopSoundNamePrefix, 3);
		FillMap("bin", binSoundNamePrefix, 3);
		FillMap("footstep", footstepSoundNamePrefix, 6);
		FillMap("pick", pickSoundNamePrefix, 3);

		FillMap("complete", completeSoundName);
		FillMap("dishDone", dishDoneSoundName);
		FillMap("drop", dropSoundName);
		FillMap("lose", loseSoundName);
		FillMap("bell", bellSoundName);

		FillMap("demonTalk", demonTalkSoundName);
		FillMap("ritualSuccess", ritualSuccessSoundName);
		FillMap("ritualFailure", ritualFailureSoundName);
		FillMap("fire", fireSoundName);
		FillMap("smokeAlarm", smokeAlarmSoundName);

		currentSounds = new List<EventInstance>();
		_musicEvent = FMODUnity.RuntimeManager.CreateInstance(musicEventName);
		_musicEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
	}

	private void FillMap(string id, string prefix)
	{
		soundNameMap.Add(id, prefix);
	}

	private void FillMap(string id, string prefix, int count)
	{
		for (int i = 1; i < count + 1; i++)
		{
			soundNameMap.Add(id + i, prefix + i);
		}
	}

	// Update is called once per frame
	void Update()
	{
		//debug
		if (Input.GetKeyDown(KeyCode.F1))
			TransitionMusic(0f);
		if (Input.GetKeyDown(KeyCode.F2))
			TransitionMusic(1f);
		if (Input.GetKeyDown(KeyCode.F3))
			TransitionMusic(2f);
	}

	public void OneShot(string name, Vector3 position)
	{
		string soundName = getSoundKey(name);
		FMODUnity.RuntimeManager.PlayOneShot(soundName, position);
	}

	public int PlaySound(string name, Vector3 pos)
	{
		string soundName = getSoundKey(name);
		var soundEvent = FMODUnity.RuntimeManager.CreateInstance(soundName);
		soundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(pos));
		soundEvent.start();

		currentSounds.Add(soundEvent);
		return currentSounds.IndexOf(soundEvent);
	}

	private string getSoundKey(string name)
	{
		var keys = soundNameMap.Keys.Where(x => x.StartsWith(name)).ToList();
		var key = keys.Count > 1 ? keys[UnityEngine.Random.Range(0, keys.Count)] : keys.First();
		var soundName = soundNameMap[key];
		return soundName;
	}

	public void StopSound(int id)
	{
		var soundEvent = currentSounds[id];
		soundEvent.setParameterValue("Parameter 1", 1f);
	}

	public void TransitionSound(int id, float val)
	{
		var soundEvent = currentSounds[id];
		soundEvent.setParameterValue("Parameter 1", val);
	}

	public void StartIntro()
	{
		_musicEvent.start();

		// Intro music
		TransitionMusic(0);
	}

	public void TransitionMusic(float param)
	{
		_musicEvent.setParameterValue("Parameter 1", param);
	}
}
