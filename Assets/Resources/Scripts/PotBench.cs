using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PotBench : BenchBase
{
	public float progress;
    public float burnt;
	
	private readonly float PROGRESS_SPEED = 10;
    private readonly float BURNT_SPEED = 15;

	public GameObject progressImagePrefab;
	public GameObject progressImage;
    public GameObject burnImage;

    public Color burnColor;

	public string TaskType = "Boil";
	private ParticleSystem.EmissionModule Emitter;

	public override IngredientBase Interact(Player player, Vector2 input)
    {
		if (contents == null)
		{
			return null;
		}

		if (progress >= 1)
		{
			contents.TasksDone.Add(TaskType);
            contents.Process();

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
		Emitter = GetComponentInChildren<ParticleSystem>().emission;
		progressImage = Instantiate(progressImagePrefab);

        burnImage = Instantiate(progressImagePrefab);
        burnImage.GetComponent<Image>().color = burnColor;

        var canvas = FindObjectOfType<Canvas>();
		progressImage.transform.SetParent(canvas.transform);
        burnImage.transform.SetParent(canvas.transform);
    }

	public new void Update()
	{
		Vector3 pos = transform.position + Vector3.up * 3;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
		progressImage.GetComponent<RectTransform>().position = screenPos;
		progressImage.GetComponent<Image>().fillAmount = progress;

        burnImage.GetComponent<RectTransform>().position = screenPos;
		//burnImage.GetComponent<Image>().fillAmount = burnt;
		
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
            if (progress >= 1)
            {
                burnt += Time.fixedDeltaTime / BURNT_SPEED;
            }
		}
	}
}
