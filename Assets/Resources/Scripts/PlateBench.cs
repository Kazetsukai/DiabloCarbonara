using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class PlateBench : BenchBase
{
	public GameObject progressImagePrefab;
	public GameObject progressImage;

	public Recipe Recipe;
	public List<IngredientBase> Ingredients;

	public override IngredientBase Interact(Player player, Vector2 input)
    {
		return null;
	}

	public override bool CanIReceive(IngredientBase item)
	{
		if (Recipe == null)
		{
			return false;
		}

		if (MatchesRecipe(item))
		{
			return true;
		}

		return false;
	}

	private bool MatchesRecipe(IngredientBase item)
	{
		var pendingIngredients = Recipe.Ingredients.Where(i => !i.IsSatisfied);
		foreach (var ingredient in pendingIngredients)
		{
			if (IngredientMatches(item, ingredient))
			{
				ingredient.IsSatisfied = true;
				Ingredients.Add(item);

				return true;
			}
		}

		return false;
	}

	private bool IngredientMatches(IngredientBase item, Ingredient ingredient)
	{
		// not right type
		if (ingredient.Type != item.Type) return false;

		// incorrect number of operations done
		if (ingredient.Tasks.Count != item.TasksDone.Count) return false;

		for (int i = 0; i < item.TasksDone.Count; i++)
		{
			if (item.TasksDone[i] != ingredient.Tasks[i])
			{
				// wrong operation done
				return false;
			}
		}

		// aaaaaaaaaaaaallllllllllllllllgggggggggggggggg
		return true;
	}

	public void Start()
	{
		Ingredients = new List<IngredientBase>();
		progressImage = Instantiate(progressImagePrefab);
		var canvas = FindObjectOfType<Canvas>();
		progressImage.transform.SetParent(canvas.transform);
	}

	public override void GetItem(IngredientBase item) { }

	public new void Update()
	{
		Vector3 pos = transform.position + Vector3.up * 3;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
		progressImage.GetComponent<RectTransform>().position = screenPos;
		progressImage.GetComponent<Image>().fillAmount = 0;

		if (Recipe != null && Recipe.IsDone())
		{
			Debug.DrawLine(transform.position, transform.position + Vector3.up*2, Color.green);
		}

		base.Update();
	}

	public void Clear()
	{
		var ingArray = Ingredients.ToArray();
		for (int i = 0; i < ingArray.Length; i++)
		{
			Destroy(ingArray[i].gameObject);
		}
		Recipe = null;
		Ingredients = new List<IngredientBase>();
	}
}
