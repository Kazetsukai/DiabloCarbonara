using UnityEngine;
using System.Collections;
using FMOD.Studio;
using System;
using System.Collections.Generic;
using System.Linq;

public class MusicMaster : MonoBehaviour
{
	System.Random random = new System.Random();

	private EventInstance _musicEvent;
	[FMODUnity.EventRef]
	public string musicEventName = "event:/Italian_Kitchen";

	public string boilSoundNamePrefix = "event:/Boil Station/Boil_Station_";
	public string frySoundNamePrefix = "event:/Fry Station/Fry_Station_";
	public string chopSoundNamePrefix = "event:/Boil Station/Boil_Station_";

	public Dictionary<string, string> soundNameMap;
	public List<EventInstance> currentSounds;

	// Use this for initialization
	void Start()
	{
		soundNameMap = new Dictionary<string, string>();
		FillMap("boil", boilSoundNamePrefix, 3);
        FillMap("fry", frySoundNamePrefix, 3);
		FillMap("chop", chopSoundNamePrefix, 3);

		currentSounds = new List<EventInstance>();
		_musicEvent = FMODUnity.RuntimeManager.CreateInstance(musicEventName);
		_musicEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
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

	public int PlaySound(string name, Vector3 pos)
	{
		var keys = soundNameMap.Keys.Where(x => x.StartsWith(name)).ToList();
		var soundName = soundNameMap[keys[random.Next(1, 3)]];
		var soundEvent = FMODUnity.RuntimeManager.CreateInstance(soundName);
		soundEvent.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(pos));
		soundEvent.start();

		currentSounds.Add(soundEvent);
		return currentSounds.IndexOf(soundEvent);
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
