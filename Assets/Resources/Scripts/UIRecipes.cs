using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIRecipes : MonoBehaviour {
    public List<Recipe> Recipes
    {
        get;
        private set;
    }

    Dictionary<string, Color> taskColors = new Dictionary<string, Color>()
    {
        { "red", new Color(1,0,0) },
        { "green", new Color(0,1,0) },
        { "blue", new Color(0,0,1) }
    };

    public GameObject RecipePanel;
    public GameObject RecipeIngredient;
    public GameObject RecipeIngredientTask;

    public GameObject CircleIngredient;
    public GameObject RectangleIngredient;
    public GameObject TriangleIngredient;

    // Use this for initialization
    void Start () {
        Recipes = new List<Recipe>();

        //Yay, hardcoded test data
        /*Recipes.AddRange(new Recipe[] {
            new Recipe()
            {
                Ingredients = new Ingredient[]
                {
                    new Ingredient()
                    {
                        Type = "Circle",
                        Tasks = new List<string>
                        (
                        )
                    },
                    new Ingredient()
                    {
                        Type = "Rectangle",
                        Tasks = new List<string>
                        (
                        )
                    }
                }
            },
            new Recipe()
            {
                Ingredients = new Ingredient[]
                {
                    new Ingredient()
                    {
                        Type = "Triangle",
                        Tasks = new List<string>
                        (
                        )
                    },
                    new Ingredient()
                    {
                        Type = "Circle",
                        Tasks = new List<string>
                        (
                        )
                    }
                }
            }
        }
        );*/

        foreach (Recipe recipe in Recipes)
        {
            var newPanel = MakeRecipePanel(recipe);
            newPanel.transform.SetParent(transform);
        }
    }

    GameObject MakeRecipePanel(Recipe recipe)
    {
        GameObject recipePanel = Instantiate(RecipePanel);

        Transform ingredientsPanel = recipePanel.transform.FindChild("UI-Recipe-Ingredients");
        foreach (Ingredient ingredient in recipe.Ingredients)
        {
            GameObject ingredientPanel = MakeIngredientPanel(ingredient);
            ingredientPanel.transform.SetParent(ingredientsPanel, false);
        }

        return recipePanel;
    }

    GameObject MakeIngredientPanel(Ingredient ingredient)
    {
        GameObject ingredientPanel = Instantiate(RecipeIngredient);
        GameObject ingredientImage = MakeIngredient(ingredient.Type);
        ingredientImage.transform.SetParent(ingredientPanel.transform, false);

        Transform ingredientTasksPanel = ingredientPanel.transform.FindChild("UI-Ingredient-Tasks");

        foreach (string task in ingredient.Tasks)
        {
            GameObject ingredientTask = MakeIngredientTask(task);
            ingredientTask.transform.SetParent(ingredientTasksPanel, false);
        }

        return ingredientPanel;
    }

    GameObject MakeIngredient(string ingredientShape)
    {
        switch (ingredientShape)
        {
            case "Circle":
                return Instantiate(CircleIngredient);
            case "Triangle":
                return Instantiate(TriangleIngredient);
            case "Rectangle":
            default:
                return Instantiate(RectangleIngredient);
        }
    }

    GameObject MakeIngredientTask(string task)
    {
        GameObject ingredientTask = Instantiate(RecipeIngredientTask);
        Image taskImage = ingredientTask.GetComponent<Image>();

        

        return ingredientTask;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
