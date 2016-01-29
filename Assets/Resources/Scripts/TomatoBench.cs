using UnityEngine;
using System.Collections;

public class TomatoBench : BenchBase
{
	public GameObject IngredientPrefab;

	public override IngredientBase Interact()
	{
		var newIngredient = Instantiate(IngredientPrefab);
		newIngredient.transform.position = transform.position;
		return newIngredient.GetComponent<IngredientBase>();
	}

	public override bool CanIReceive(IngredientBase item)
	{
		return false;
	}
}
