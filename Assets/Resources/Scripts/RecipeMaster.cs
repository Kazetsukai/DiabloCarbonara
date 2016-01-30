﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RecipeMaster : MonoBehaviour {
    public const int MaxRecipies = 3;

    public const int MinComplexity = 3;
    public const int MaxComplexity = 9;

    public const int MinIngredients = 1;
    public const int MaxIngredients = 3;

    public const int MinSteps = 0;
    public const int MaxSteps = 2;

    public const float MinTimeWithNoOrders = 5;
    public const float MaxTimeWithNoOrders = 15;

    private float TimeSinceLastOrder = MaxTimeWithNoOrders;

    public readonly string[] IngredientTypes = new string[]
    {
        "Pasta",
        "Tomato",
        "Meat"
    };

    public readonly string[] TaskTypes = new string[]
    {
        "Chop",
        "Boil",
        "Fry"
    };
    
	// Use this for initialization
	void Start () {
	    
	}

    string GetRandomIngredient()
    {
        return IngredientTypes[Random.Range(0, IngredientTypes.Length)];
    }

    string GetRandomTask()
    {
        return TaskTypes[Random.Range(0, TaskTypes.Length)];
    }

    public Recipe CreateRandomRecipe()
    {
        Recipe recipe;
        do
        {
            recipe = new Recipe();
            List<Ingredient> ingredients = new List<Ingredient>();
            int numberOfIngredients = Random.Range(MinIngredients, MaxIngredients + 1);

            for (int i = 0; i < numberOfIngredients; i++)
            {
                Ingredient ingredient = new Ingredient();
                ingredient.Type = GetRandomIngredient();
                if (recipe.Ingredients.Any(ing => ing.Type == ingredient.Type))
                {
                    ingredient.Type = GetRandomIngredient();
                }

                int numberOfTasks = Random.Range(MinSteps, MaxSteps + 1);
                for (int a = 0; a < numberOfTasks; a++)
                {
                    string taskType = GetRandomTask();
                    if (ingredient.Tasks.Any(t => t == taskType))
                    {
                        taskType = GetRandomTask();
                    }
                    ingredient.Tasks.Add(taskType);
                }

                recipe.Ingredients.Add(ingredient);
            }

        } while (recipe.Complexity < MinComplexity || recipe.Complexity > MaxComplexity);

        return recipe;
    }
	
	// Update is called once per frame
	void Update () {
        TimeSinceLastOrder += Time.deltaTime;
        PlateBench[] plates = FindObjectsOfType<PlateBench>();
        bool isAPlateFilled = plates.Any(p => p.Recipe != null);

        if (!isAPlateFilled || TimeSinceLastOrder > MaxTimeWithNoOrders || (TimeSinceLastOrder > MinTimeWithNoOrders && Random.Range(0, MaxTimeWithNoOrders) > TimeSinceLastOrder))
        {
            foreach (PlateBench plate in plates)
            {
                if (plate.Recipe == null)
                {
                    plate.Recipe = CreateRandomRecipe();
					print(plate.Recipe);
                    break;
                }
            }

            TimeSinceLastOrder = 0;
        }            
	}
}
