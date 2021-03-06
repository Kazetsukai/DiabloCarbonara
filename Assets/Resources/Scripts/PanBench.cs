﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class PanBench : BenchBase
{
	[Header("Pan Bench")]
	public GameObject Spatula;
	public Transform StirTarget;
	public float StirRadius = 0.5f;
	public float progress;
	public float burning;
	public float burningCooldown;
	private const float burnTime = 5f;
    private readonly float PROGRESS_SPEED = 5;
    public float MinimumStirAngleProgress = 15f;        //To stop player skipping stirring full circles

	public GameObject progressImagePrefab;
	public GameObject progressImage;
    public GameObject StirIndicator;
    public LightFader FireLight;
 
	public string TaskType = "Fry";

	public float StirAngle;
	public float StirAngleProgress;

	Vector3 SpatulaIdlePosition;
	Vector3 SpatulaIdleRotation;
	float InteractSpatulaResetTime = 0.1f;
	float InteractSpatulaResetElapsed;

	float lastStirAngle;

	private ParticleSystem.EmissionModule Emitter;
	private ParticleSystem.EmissionModule BurnEmitter;
	private ParticleSystem.EmissionModule FireEmitter;

	private float FLASH_SCALE = 15f;
	private float burnThreshold = 1f;
	private bool burnt;
	private MusicMaster musicMaster;
	private int currentSound;

    bool fireLightActive;
	private int currentFire = -1;

	public override IngredientBase Interact(Player player, Vector2 input)
	{
		if (contents == null)
		{
			return null;
		}

		LastInteractedPlayer = player;

        //Get stir angle from input
        if (player.HorizontalAxis.EndsWith("P5"))
        {
            if (input.x > 0)
            { 
                if (StirAngle <= 110 || StirAngle >= 250)
                {
                    StirAngle = (StirAngle + 20) % 360;
                }
            }

            if (input.x < 0)
            {
                if (StirAngle >= 70 && StirAngle <= 290)
                {
                    StirAngle = (StirAngle + 20) % 360;
                }
            }
        }
        else
        {
            StirAngle = Vector2.Angle(Vector2.up, input);
            Vector3 cross = Vector3.Cross(Vector2.up, input);
            if (cross.z > 0)
            {
                StirAngle = 360 - StirAngle;
            }
        }		

		//Update stir angle progress (only allow increases by counter clockwise stirring)
		float deltaStirAngle = Mathf.Clamp(StirAngle - lastStirAngle, 0, 360);

		if (deltaStirAngle > MinimumStirAngleProgress)  //Prevent cheating by skipping stirring in full circles
		{
			StirAngleProgress = 0;
		}
		else
		{
			StirAngleProgress += deltaStirAngle;
		}

		if (StirAngleProgress >= 360f)
		{
			StirAngleProgress = 0f;

			burningCooldown = Time.time + 2f;
			burning = 0;
		}

		lastStirAngle = StirAngle;

		//Do animation for players arms
		player.IKArm_R.solver.target = HandIKTarget_R;          //Set IK target R of player to be IK transform R of this bench                                                    
		player.IKArm_L.solver.target = HandIKTarget_L;          //Set IK target L of player to be IK transform L of this bench

		HandIKTarget_L.transform.position = StirTarget.position;

		//Move stir target for spatula
		StirTarget.transform.localPosition = new Vector3(StirRadius * Mathf.Cos((-StirAngle + 180) * Mathf.Deg2Rad), StirTarget.transform.position.y, StirRadius * Mathf.Sin((-StirAngle + 180) * Mathf.Deg2Rad));

		//Move spatula accordingly
		InteractSpatulaResetElapsed = 0;
		Spatula.transform.position = player.HandLeft.transform.position;
		Spatula.transform.LookAt(StirTarget.position);

        //Show stir indicator
        if (contents != null)
        {
            StirIndicator.gameObject.SetActive(true);
            StirIndicator.transform.position = LastInteractedPlayer.transform.position + new Vector3(0, 1.3f, 0);
        }
        else
        {
            StirIndicator.gameObject.SetActive(false);
        }               

		if (progress >= 1)
		{
			if (burnt)
			{
				contents.Burn();
			}
			else
			{
				contents.Process(TaskType);
			}
			musicMaster.StopSound(currentSound);

			//Reset player arm IK targets
			player.IKArm_R.solver.target = player.ArmIKTarget_R;
			player.IKArm_L.solver.target = player.ArmIKTarget_L;

            //Turn off burning
            burning = 0;
			FireLight.TurnOff();

			progress = 0;
			var temp = contents;
			contents = null;
            burnt = false;
            return temp;
		}
		else
		{
			return null;
		}
	}

	public override bool CanIReceive(IngredientBase item)
	{
        if (item.burnt)
        {
            return false;
        }

		burningCooldown = Time.time + 2f;
		currentSound = musicMaster.PlaySound(TaskType.ToLowerInvariant(), transform.position);
		return true;
	}

	public void Start()
	{
		musicMaster = FindObjectOfType<MusicMaster>();

		var particleSystems = GetComponentsInChildren<ParticleSystem>();
		var psDict = particleSystems.ToDictionary(ps => ps.gameObject.name, ps => ps.emission);

		if (psDict.ContainsKey("particles")) Emitter = psDict["particles"];
		if (psDict.ContainsKey("burning")) BurnEmitter = psDict["burning"];
		if (psDict.ContainsKey("fire")) FireEmitter = psDict["fire"];

		progressImage = Instantiate(progressImagePrefab);
		progressImage.GetComponent<Image>().color = Color.blue;
		var canvas = FindObjectOfType<Canvas>();
		progressImage.transform.SetParent(canvas.transform);

		SpatulaIdlePosition = Spatula.transform.position;
		SpatulaIdleRotation = Spatula.transform.eulerAngles;

        //Face stir indicator to camera on start (must be this direction for rotation to be right!!)
        StirIndicator.transform.forward = Camera.main.transform.forward;
    }

	public new void Update()
	{    
		Vector3 pos = transform.position + Vector3.up * 3;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
		progressImage.GetComponent<RectTransform>().position = screenPos;
		progressImage.GetComponent<Image>().fillAmount = progress;
        progressImage.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

        //Update Spatula position
        InteractSpatulaResetElapsed += Time.deltaTime;
		if (InteractSpatulaResetElapsed >= InteractSpatulaResetTime)
		{
			Spatula.transform.position = SpatulaIdlePosition;
			Spatula.transform.eulerAngles = SpatulaIdleRotation;

			//Reset player arm IK targets
			if (LastInteractedPlayer != null)
			{
				LastInteractedPlayer.IKArm_R.solver.target = LastInteractedPlayer.ArmIKTarget_R;
				LastInteractedPlayer.IKArm_L.solver.target = LastInteractedPlayer.ArmIKTarget_L;
				LastInteractedPlayer = null;
			}

            //Hide stir indicator
            StirIndicator.gameObject.SetActive(false);
        }

		bool onFire = false;
		bool onSmoke = false;
		if (contents != null)
		{
			Color newColor;

			//burning
			if (burning > 0)
			{
				if (burning > burnThreshold)
				{
                    contents.Burn();
                    burnt = true;
					progress = 1;
					newColor = Color.black;
					onFire = true;
				}
				else
				{
					newColor = Color.red * ((Mathf.Sin(Time.time * FLASH_SCALE) / 2) + 0.5f);
					onSmoke = true;
				}
			}
			else
			{
				newColor = Color.blue;
			}

			newColor.a = 0.8f;
			progressImage.GetComponent<Image>().color = newColor;
		}

		aasd(BurnEmitter, onSmoke);
		aasdd(FireEmitter, onFire);
		aasd(Emitter, contents != null);

		base.Update();
	}

	private void aasdd(ParticleSystem.EmissionModule mod, bool should)
	{
		if (!mod.enabled && should)
		{
			currentFire = musicMaster.PlaySound("fire", transform.position);
			FireLight.TurnOn();
			mod.enabled = true;
		}
		else if (mod.enabled && !should)
		{
			musicMaster.StopSound(currentFire);
			FireLight.TurnOff();
			currentFire = -1;
			mod.enabled = false;
		}
	}

	private void aasd(ParticleSystem.EmissionModule mod, bool should)
	{
		if (!mod.enabled && should)
		{
			mod.enabled = true;
		}
		else if (mod.enabled && !should)
		{
			mod.enabled = false;
		}
	}

	public void FixedUpdate()
	{
		if (contents != null)
		{
            if (Time.time > burningCooldown)
            {
                burning += (1f / burnTime) * Time.fixedDeltaTime;
            }
            else
            {
                var prevProgress = progress;
                progress += Time.fixedDeltaTime / PROGRESS_SPEED;

                if (prevProgress < 1 && progress > 1)
                {
                    musicMaster.OneShot("complete", transform.position);
                }
            }
        }
	}

  
}