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

	private ParticleSystem.EmissionModule Emitter;
	private ParticleSystem.EmissionModule BurnEmitter;
	private ParticleSystem.EmissionModule FireEmitter;

	private float FLASH_SCALE = 15f;
	private float burnThreshold = 1f;

	public override IngredientBase Interact(Player player, Vector2 input)
	{
		if (contents == null)
		{
			return null;
		}

		if (progress >= 1)
		{
			contents.TasksDone.Add(TaskType);
			if (burning > burnThreshold)
			{
				contents.Burn();
			}
			else
			{
				contents.Process();
			}

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
		return true;
	}

	public void Start()
	{
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


		BurnEmitter.enabled = false;
		FireEmitter.enabled = false;
		if (contents != null)
		{
			Color newColor;

			//burning
			if (burning > 0)
			{

				if (burning > burnThreshold)
				{
					newColor = Color.black;
					FireEmitter.enabled = true;
				}
				else
				{
					newColor = Color.red * ((Mathf.Sin(Time.time * FLASH_SCALE) / 2) + 0.5f);
					BurnEmitter.enabled = true;
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

		if (!Emitter.enabled && contents != null)
		{
			Emitter.enabled = true;
		}
		else if (Emitter.enabled && contents == null)
		{
			Emitter.enabled = false;
		}

		base.Update();
	}

	public void FixedUpdate()
	{
		if (contents != null)
		{
			progress += Time.fixedDeltaTime / PROGRESS_SPEED;
		}
	}
}
