using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RecipeMaster : MonoBehaviour {
    public List<Recipe> CurrentRecipies;

    public const int MaxRecipies = 3;

    public const int MinIngredients = 1;
    public const int MaxIngredients = 3;

    public const int MinSteps = 0;
    public const int MaxSteps = 2;
    
	// Use this for initialization
	void Start () {
	    
	}

    public Recipe CreateRandomRecipe()
    {
        return new Recipe()
        {
            //Ingredients = 
        };
    }

    List<Ingredient> CreateRandomIngredientList()
    {
        return null;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
