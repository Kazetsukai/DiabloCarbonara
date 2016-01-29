using UnityEngine;
using System.Collections;

public class PanBench : BenchBase
{
	public float progress;

	private bool progressingThisFrame;
	private readonly float PROGRESS_SPEED = 3;

	public GUITexture progressImage;

	public override IngredientBase Interact()
	{
		if (progress >= 1)
		{
			progress = 0;
			var temp = contents;
			contents = null;
            return temp;
		}
		else
		{
			progressingThisFrame = true;
			return null;
		}
	}

	public override bool CanIReceive(IngredientBase item)
	{
		return true;
	}

	public void FixedUpdate()
	{
		if (progressingThisFrame)
		{
			progressingThisFrame = false;
			progress += Time.fixedDeltaTime / PROGRESS_SPEED;
		}
	}
}
