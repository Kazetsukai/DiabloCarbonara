using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChopBench : BenchBase
{
	public float progress;
	public float HitsRequired;
	public GameObject progressImagePrefab;
    public float InputValue_Up = 0f;
    public float InputValue_Down = -0.9f;
    public float ChopSuccessIncrement = 0.07f;

    public bool downTargetReached = false;
    public bool upTargetReached = false;

    private bool progressingThisFrame;
	private float progressCooldown = 0f;

	private GameObject progressImage;
    
	public string TaskType = "Chop";

	public override IngredientBase Interact(Vector2 input)
	{       
		if (contents == null)
		{
			return null;
		}
                
        if (input.y >= InputValue_Up)
        {
            upTargetReached = true;
        }
        if (input.y <= InputValue_Down)
        {
            downTargetReached = true;
        }

        if (upTargetReached && downTargetReached)
        {
            progress += ChopSuccessIncrement;
            upTargetReached = false;
            downTargetReached = false;
        }

        if (progress >= 1)
		{
            upTargetReached = false;
            downTargetReached = false;

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
		if (progressingThisFrame)
		{
			progress += 1f/HitsRequired;
			progressingThisFrame = false;
		}
	}
}
