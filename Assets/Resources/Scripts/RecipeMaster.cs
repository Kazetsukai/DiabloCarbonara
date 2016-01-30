using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecipeMaster : MonoBehaviour {
    public List<Recipe> CurrentRecipies;

    public const int MaxRecipies = 3;

    public const int MinComplexity = 3;
    public const int MaxComplexity = 9;

    public const int MinIngredients = 1;
    public const int MaxIngredients = 3;

    public const int MinSteps = 0;
    public const int MaxSteps = 2;

    public const float MinTimeWithNoOrders = 5;
    public const float MaxTimeWithNoOrders = 10;

    private float TimeSinceLastOrder = 0;

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
                ingredient.Type = IngredientTypes[Random.Range(0, IngredientTypes.Length)];

                int numberOfTasks = Random.Range(MinSteps, MaxSteps + 1);
                for (int a = 0; a < numberOfTasks; a++)
                {
                    ingredient.Tasks.Add(TaskTypes[Random.Range(0, TaskTypes.Length)]);
                }

                recipe.Ingredients.Add(ingredient);
            }

        } while (recipe.Complexity < MinComplexity || recipe.Complexity > MaxComplexity);

        return recipe;
    }
	
	// Update is called once per frame
	void Update () {
        if (CurrentRecipies.Count < 3)
        {
            TimeSinceLastOrder += Time.deltaTime;

            if (TimeSinceLastOrder > MaxTimeWithNoOrders || (TimeSinceLastOrder > MinTimeWithNoOrders && Random.Range(0, MaxTimeWithNoOrders) > TimeSinceLastOrder))
            {

            }
        }
        else
        {
            TimeSinceLastOrder = 0;
        }
	}
}
