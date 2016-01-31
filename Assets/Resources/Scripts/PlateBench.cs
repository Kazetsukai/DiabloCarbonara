using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class PlateBench : BenchBase
{
    public int PlateNumber;
	public GameObject progressImagePrefab;
	public GameObject progressImage;

	public Recipe Recipe;
	public List<IngredientBase> Ingredients;

    public GameObject Plate;
    public Transform PlateGoalTransform;
    public float PlateOutDuration = 1f;
    public float PlateInDuration = 1f;

    public Transform FoodOffsetTransform;
    
    public List<GameObject> CompletionSpritePrefabs;
    public ParticleSystem RecipeCompleteParticles;

    GameObject currentCompletionSprite;

    Vector3 plateStartPos;
    Vector3 plateStartScale;

    bool completed;
    
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
        plateStartPos = Plate.transform.position;
        plateStartScale = Plate.transform.localScale;
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

        if (Recipe != null)
        {
            if (Recipe.IsDone() && !completed)
            {
                //Hide icons of all other foods on plate
                foreach (IngredientBase ingSprite in GetComponentsInChildren<IngredientBase>())
                {
                    ingSprite.gameObject.SetActive(false);
                }
                                
                //Put completion food sprite on plate
                //Generate prefab completion sprite
                int spriteIndex = UnityEngine.Random.Range(0, CompletionSpritePrefabs.Count);
                currentCompletionSprite = Instantiate(CompletionSpritePrefabs[spriteIndex]);
                currentCompletionSprite.transform.parent = FoodOffsetTransform;
                currentCompletionSprite.transform.localPosition = Vector3.zero;
                completed = true;

                //Do particles
                RecipeCompleteParticles.Emit(30);
            }
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

    public void DoRecipeCompleteAnim()
    {
        StartCoroutine(TransitionOutPlate());

    }

    IEnumerator TransitionOutPlate()
    {
        float elapsed = 0;        

        do
        {
            elapsed += Time.deltaTime;
            Plate.transform.position = Vector3.Lerp(plateStartPos, PlateGoalTransform.position, elapsed / PlateOutDuration);
                       
            yield return null;
        }
        while (elapsed / PlateOutDuration < 1f);

        //Destroy food on plate
        Destroy(currentCompletionSprite);

        //Bring new plate in
        StartCoroutine(TransitionInPlate());

        completed = false;
    }

    IEnumerator TransitionInPlate()
    {
        float elapsed = 0;

        do
        { 
            elapsed += Time.deltaTime;
            Plate.transform.position = plateStartPos;

            Plate.transform.localScale = Vector3.Lerp(Vector3.zero, plateStartScale, elapsed / PlateInDuration);

            yield return null;
        }
        while (elapsed / PlateInDuration < 1f);
    }    

}
