﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIRecipes : MonoBehaviour
{
	readonly Dictionary<string, Color> taskColors = new Dictionary<string, Color>()
	{
		{ "Chop", new Color(0,1,0) },
		{ "Boil", new Color(0,0,1) },
		{ "Fry", new Color(1,0,0) }
	};

	readonly float fadeOutRate = 0.015f;

	//PlateBench[] plateBenches;
	//Dictionary<PlateBench, Recipe> plateRecipes;
	//Dictionary<Recipe, GameObject> recipePanels = new Dictionary<Recipe, GameObject>();
	Dictionary<PlateBench, PlateInfo> plates;

	public GameObject RecipePanel;
	public GameObject RecipeIngredient;
	public GameObject RecipeIngredientTask;

	public Sprite PastaIngredient;
	public Sprite TomatoIngredient;
	public Sprite MeatIngredient;

	public Sprite ChopIcon;
	public Sprite BoilIcon;
	public Sprite FryIcon;

	// Use this for initialization
	void Start()
	{
		var plateBenches = FindObjectsOfType<PlateBench>();
		plates = plateBenches.Select(b => new PlateInfo { Bench = b, Recipe = null, UI = null }).ToDictionary(pi => pi.Bench);
	}

	GameObject MakeRecipePanel(PlateInfo plateInfo)
	{
		GameObject recipePanel = Instantiate(RecipePanel);

		foreach (Ingredient ingredient in plateInfo.Recipe.Ingredients)
		{
			GameObject ingredientPanel = MakeIngredientPanel(ingredient);
			ingredientPanel.transform.SetParent(recipePanel.transform, false);
		}

		Canvas canvas = FindObjectOfType<Canvas>();
		recipePanel.transform.SetParent(canvas.transform);

		Vector3 pos = plateInfo.Bench.transform.position + Vector3.up * 3.5f;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
		recipePanel.transform.position = screenPos;

		return recipePanel;
	}

	GameObject MakeIngredientPanel(Ingredient ingredient)
	{
		GameObject ingredientPanel = Instantiate(RecipeIngredient);
		Transform ingredientImagePanel = ingredientPanel.transform.FindChild("UI-Ingredient-Image");
		Image ingredientImage = ingredientImagePanel.GetComponent<Image>();
		ingredientImage.sprite = MakeIngredient(ingredient.Type);

		Transform ingredientTasksPanel = ingredientPanel.transform.FindChild("UI-Ingredient-Tasks");

		foreach (string task in ingredient.Tasks)
		{
			GameObject ingredientTask = MakeIngredientTask(task);
			ingredientTask.transform.SetParent(ingredientTasksPanel, false);
		}

		CanvasRenderer[] renderers = ingredientPanel.GetComponentsInChildren<CanvasRenderer>();

		foreach (CanvasRenderer renderer in renderers)
		{
			renderer.SetAlpha(0.6f);
		}

		return ingredientPanel;
	}

	Sprite MakeIngredient(string ingredientType)
	{
		switch (ingredientType)
		{
			case "Meat":
				return MeatIngredient;
			case "Tomato":
				return TomatoIngredient;
			case "Pasta":
			default:
				return PastaIngredient;
		}
	}

	Sprite MakeTask(string taskType)
	{
		switch (taskType)
		{
			case "Boil":
				return BoilIcon;
			case "Fry":
				return FryIcon;
			case "Chop":
			default:
				return ChopIcon;
		}
	}

	GameObject MakeIngredientTask(string task)
	{
		GameObject ingredientTask = Instantiate(RecipeIngredientTask);
		Image taskImage = ingredientTask.GetComponent<Image>();

		taskImage.sprite = MakeTask(task);

		return ingredientTask;
	}

	void PartialCompleteRecipePanel(GameObject recipePanel, int ingredientIndex)
	{
		Transform ingredientPanel = recipePanel.transform.GetChild(ingredientIndex);
		CanvasRenderer[] renderers = ingredientPanel.GetComponentsInChildren<CanvasRenderer>();

		foreach (CanvasRenderer renderer in renderers)
		{
			renderer.SetAlpha(1f);
		}
	}

	void CompleteRecipePanel(PlateInfo plateInfo)
	{
		CanvasRenderer[] renderers = plateInfo.UI.GetComponentsInChildren<CanvasRenderer>();
		float currentAlpha = renderers[0].GetAlpha();

		if (currentAlpha > 0)
		{
			foreach (CanvasRenderer renderer in renderers)
			{
				renderer.SetAlpha(currentAlpha - fadeOutRate);
			}
		}
		else
		{
			RemoveRecipePanel(plateInfo);
		}
	}

	void RemoveRecipePanel(PlateInfo plateInfo)
	{
		Destroy(plateInfo.UI);
		plateInfo.Recipe = null;
		plateInfo.UI = null;
		plateInfo.Bench.Clear();
	}

	bool CheckPlateRecipeForChanges(PlateInfo plateInfo)
	{
		var existingRecipe = plateInfo.Recipe;
		var newRecipe = plateInfo.Bench.Recipe;

		if (existingRecipe != newRecipe)
		{
			// recipe changed
			if (newRecipe != null)
			{
				// new recipe
				if (existingRecipe != null)
				{
					// old still around
					RemoveRecipePanel(plateInfo);
				}

				plateInfo.Recipe = newRecipe;
				plateInfo.UI = MakeRecipePanel(plateInfo);
			}

			return true;
		}

		return false;
	}

	// Update is called once per frame
	void Update()
	{
		foreach (PlateInfo plateInfo in plates.Values)
		{
			var plateBench = plateInfo.Bench;
			var recipe = plateInfo.Recipe;
			var ui = plateInfo.UI;

			if (CheckPlateRecipeForChanges(plateInfo))
			{
				//update recipe
				plateInfo.Recipe = plateBench.Recipe;
			}

			if (recipe != null)
			{
				if (recipe.IsDone())
				{
					// complete
					CompleteRecipePanel(plateInfo);
				}
				else
				{
					// update partial complete
					for (int i = 0; i < recipe.Ingredients.Count; i++)
					{
						if (recipe.Ingredients[i].IsSatisfied)
						{
							PartialCompleteRecipePanel(ui, i);
						}
					}
				} 
			}
		}
	}

	private class PlateInfo
	{
		public PlateBench Bench;
		public Recipe Recipe;
		public GameObject UI;
	}
}
