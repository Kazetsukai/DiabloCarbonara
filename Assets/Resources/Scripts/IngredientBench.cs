using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientBench : BenchBase
{
	public GameObject IngredientPrefab;
	public string Type;

	public override IngredientBase Interact()
	{
		var newIngredient = Instantiate(IngredientPrefab);
		newIngredient.GetComponent<IngredientBase>().Type = "Triangle";
		newIngredient.transform.position = transform.position;
		return newIngredient.GetComponent<IngredientBase>();
	}

	public override bool CanIReceive(IngredientBase item)
	{
		return false;
	}
}
