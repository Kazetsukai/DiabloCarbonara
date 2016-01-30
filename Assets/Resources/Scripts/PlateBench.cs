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

	private Recipe Recipe;

	public override IngredientBase Interact()
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
			//Ingredients.Add(item);
			return true;
		}

		return false;
	}

	private bool MatchesRecipe(IngredientBase item)
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
		progressImage.GetComponent<Image>().fillAmount = 0;

		base.Update();
	}

	public void FixedUpdate()
	{

	}
}
