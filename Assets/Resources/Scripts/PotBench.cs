using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class PotBench : BenchBase
{
	private const float doneTime = 0.5f;
	public float progress;
	public float done
	{
		get
		{
			return Mathf.Max(progress - 1, 0);
		}
	}
	public float burning
	{
		get
		{
			return Mathf.Max(done - doneTime, 0);
		}
	}

	private readonly float PROGRESS_SPEED = 10;

	public GameObject progressImagePrefab;
	public GameObject progressImage;

	public string TaskType = "Boil";
    public LightFader FireLight;

    private ParticleSystem.EmissionModule Emitter;
	private ParticleSystem.EmissionModule BurnEmitter;
	private ParticleSystem.EmissionModule FireEmitter;

	private float FLASH_SCALE = 15f;
	private float burnThreshold = 0.5f;
	private MusicMaster musicMaster;
	private int currentSound;
	private int currentFire = -1;

	public override IngredientBase Interact(Player player, Vector2 input)
	{
		if (contents == null)
		{
			return null;
		}

		if (progress >= 1)
		{
			if (burning > burnThreshold)
			{
				contents.Burn();
			}
			else
			{
				contents.Process(TaskType);
			}
			musicMaster.StopSound(currentSound);

            //Turn off burning     
            FireLight.TurnOff();

            progress = 0;
			var temp = contents;
			contents = null;
			return temp;
		}
		else
		{
			return null;
		}
	}

	public override bool CanIReceive(IngredientBase item)
	{
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
	}

	public new void Update()
	{
		Vector3 pos = transform.position + Vector3.up * 3;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
		progressImage.GetComponent<RectTransform>().position = screenPos;
		progressImage.GetComponent<Image>().fillAmount = progress;
		progressImage.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

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
					newColor = Color.black;
					onFire = true;

				}
				else
				{
					newColor = Color.red * ((Mathf.Sin(Time.time * FLASH_SCALE) / 2) + 0.5f);
					onSmoke = true;
				}

			}
			else if (done > 0)
			{
				newColor = Color.green;
			}
			else
			{
				newColor = Color.blue;
			}

			newColor.a = 0.8f;
			progressImage.GetComponent<Image>().color = newColor;
		}
		else if (currentFire >= 0)
		{
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
			mod.enabled = true;
		}
		else if (mod.enabled && !should)
		{
			musicMaster.StopSound(currentFire);
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
			var prevProgress = progress;

			progress += Time.fixedDeltaTime / PROGRESS_SPEED;

			if (prevProgress < 1 && progress > 1)
			{
				musicMaster.OneShot("complete", transform.position);
			}
		}
	}
}