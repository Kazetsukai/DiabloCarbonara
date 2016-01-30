using UnityEngine;

public class BinBench : BenchBase
{
	public override IngredientBase Interact(Player player, Vector2 input)
    {
		return null;
	}

	public override bool CanIReceive(IngredientBase item)
	{
		return true;
	}

	public override void GetItem(IngredientBase item) { }

	public override Vector3 TargetLocation(IngredientBase item)
	{
		return IngredientOffsetTransform.position + Random.insideUnitSphere/3;
    }
}
