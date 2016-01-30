using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientBench : BenchBase
{
	public GameObject IngredientPrefab;
	public string Type;

	public override IngredientBase Interact(Player player, Vector2 input)
    {
		var newIngredient = Instantiate(IngredientPrefab);
		newIngredient.GetComponent<IngredientBase>().Type = Type;
		newIngredient.transform.position = transform.position;
		return newIngredient.GetComponent<IngredientBase>();
	}

	public override bool CanIReceive(IngredientBase item)
	{
		return false;
	}
}
