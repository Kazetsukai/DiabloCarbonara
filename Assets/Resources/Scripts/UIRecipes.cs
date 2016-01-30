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
        Recipes.AddRange(new Recipe[] {
            new Recipe()
            {
                Name = "Recipe Test",
                Ingredients = new Ingredient[]
                {
                    new Ingredient()
                    {
                        Type = "Circle",
                        Tasks = new IngredientTask[]
                        {
                            new IngredientTask()
                            {
                                Name = "Red",
                                Color = new Color(1,0,0)
                            },
                            new IngredientTask()
                            {
                                Name = "Blue",
                                Color = new Color(0,0,1)
                            }
                        }
                    },
                    new Ingredient()
                    {
                        Type = "Rectangle",
                        Tasks = new IngredientTask[]
                        {
                            new IngredientTask()
                            {
                                Name = "Blue",
                                Color = new Color(0,0,1)
                            },
                            new IngredientTask()
                            {
                                Name = "Green",
                                Color = new Color(0,1,0)
                            }
                        }
                    }
                }
            },
            new Recipe()
            {
                Name = "Different Recipe",
                Ingredients = new Ingredient[]
                {
                    new Ingredient()
                    {
                        Type = "Triangle",
                        Tasks = new IngredientTask[]
                        {
                            new IngredientTask()
                            {
                                Name = "Red",
                                Color = new Color(1,0,0)
                            },
                            new IngredientTask()
                            {
                                Name = "Green",
                                Color = new Color(0,1,0)
                            }
                        }
                    },
                    new Ingredient()
                    {
                        Type = "Circle",
                        Tasks = new IngredientTask[]
                        {
                            new IngredientTask()
                            {
                                Name = "Blue",
                                Color = new Color(0,0,1)
                            },
                            new IngredientTask()
                            {
                                Name = "Red",
                                Color = new Color(1,0,0)
                            }
                        }
                    }
                }
            }
        }
        );

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

        foreach (IngredientTask task in ingredient.Tasks)
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

    GameObject MakeIngredientTask(IngredientTask task)
    {
        GameObject ingredientTask = Instantiate(RecipeIngredientTask);
        Image taskImage = ingredientTask.GetComponent<Image>();

        taskImage.color = task.Color;

        return ingredientTask;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
