using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChopBench : BenchBase
{
	public float progress;
	public float HitsRequired;
	public GameObject progressImagePrefab;

	private bool progressingThisFrame;
	private float progressCooldown = 0f;

	private GameObject progressImage;

	public string TaskType = "Chop";

	public override IngredientBase Interact()
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
		else if(progressCooldown < Time.fixedTime)
		{
			progressCooldown = Time.fixedTime + 0.03f;
			progressingThisFrame = true;
			return null;
		}
		else
		{
			progressCooldown = Time.fixedTime + 0.03f;
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
		progressImage.transform.parent = canvas.transform;
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
		if (progressingThisFrame)
		{
			progress += 1f/HitsRequired;
			progressingThisFrame = false;
		}
	}
}
