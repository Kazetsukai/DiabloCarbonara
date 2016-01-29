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
        Recipes.Add(new Recipe()
        {
            Name = "Recipe Test",
            Ingredients = new Ingredient[]
            {
                new Ingredient()
                {
                    Shape = "Circle",
                    Tasks = new IngredientTask[]
                    {
                        new IngredientTask()
                        {
                            Name = "Fire",
                            Color = new Color(1,0,0)
                        }
                    }
                }
            }
        });

        foreach (Recipe recipe in Recipes)
        {
            var newPanel = MakeRecipePanel(recipe);
            newPanel.transform.SetParent(transform);
        }
    }

    GameObject MakeRecipePanel(Recipe recipe)
    {
        GameObject recipePanel = Instantiate(RecipePanel);
        Transform label = recipePanel.transform.FindChild("UI-Recipe-Label");
        Text labelText = label.GetComponent<Text>();
        labelText.text = recipe.Name;

        Transform ingredientsPanel = recipePanel.transform.FindChild("UI-Recipe-Ingredients");
        foreach (Ingredient ingredient in recipe.Ingredients)
        {
            GameObject ingredientPanel = MakeIngredientPanel(ingredient);
            ingredientPanel.transform.SetParent(ingredientsPanel);
        }

        return recipePanel;
    }

    GameObject MakeIngredientPanel(Ingredient ingredient)
    {
        GameObject ingredientPanel = Instantiate(RecipeIngredient);

        switch (ingredient.Shape)
        {
            case "Circle":
                
                break;
            case "Triangle":
                
                break;
            case "Rectangle":
            default:
                
                break;
        }

        return ingredientPanel;
    }

    /*GameObject MakeIngredient(string ingredientShape)
    {
        switch (ingredientShape)
        {
            case "Circle":
                return Instantiate(CircleIngredient)
                break;
            case "Triangle":

                break;
            case "Rectangle":
            default:

                break;
        }
    }*/

    GameObject MakeIngredientTask(IngredientTask task)
    {
        GameObject ingredientTask = Instantiate(RecipeIngredientTask);

        return ingredientTask;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
