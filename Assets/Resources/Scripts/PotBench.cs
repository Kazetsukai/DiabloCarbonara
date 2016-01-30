using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PotBench : BenchBase
{
	public float progress;
	
	private readonly float PROGRESS_SPEED = 10;

	public GameObject progressImagePrefab;
	public GameObject progressImage;

	public string TaskType = "Boil";

	public override IngredientBase Interact(Player player, Vector2 input)
    {
		if (contents == null)
		{
			return null;
		}

		if (progress >= 1)
		{
			contents.TasksDone.Add(TaskType);

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
		progressImage = Instantiate(progressImagePrefab);
		var canvas = FindObjectOfType<Canvas>();
		progressImage.transform.SetParent(canvas.transform);
    }

	public new void Update()
	{
		Vector3 pos = transform.position + Vector3.up * 3;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
		progressImage.GetComponent<RectTransform>().position = screenPos;
		progressImage.GetComponent<Image>().fillAmount = progress;

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
