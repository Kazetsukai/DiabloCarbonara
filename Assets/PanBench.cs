using UnityEngine;
using System.Collections;

public class PanBench : BenchBase
{
	public float progress { get; set; }

	private bool progressingThisFrame;
	private readonly float PROGRESS_SPEED = 3;

	public override IngredientBase Interact()
	{
		if (progress >= 1)
		{
			return contents;
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
