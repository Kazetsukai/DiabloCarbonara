using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIRecipes : MonoBehaviour {
    Dictionary<string, Color> taskColors = new Dictionary<string, Color>()
    {
        { "Chop", new Color(0,1,0) },
        { "Boil", new Color(0,0,1) },
        { "Fry", new Color(1,0,0) }
    };

    PlateBench[] plateBenches;
    Dictionary<PlateBench, Recipe> plateRecipes;
    Dictionary<Recipe, GameObject> recipePanels = new Dictionary<Recipe, GameObject>();

    public GameObject RecipePanel;
    public GameObject RecipeIngredient;
    public GameObject RecipeIngredientTask;

    public GameObject PastaIngredient;
    public GameObject TomatoIngredient;
    public GameObject MeatIngredient;

    // Use this for initialization
    void Start () {
        plateBenches = FindObjectsOfType<PlateBench>();
        plateRecipes = plateBenches.ToDictionary(p => p, p => (Recipe)null);
    }

    void MakeRecipePanel(Recipe recipe)
    {
        GameObject recipePanel = Instantiate(RecipePanel);

        Transform ingredientsPanel = recipePanel.transform.FindChild("UI-Recipe-Ingredients");
        foreach (Ingredient ingredient in recipe.Ingredients)
        {
            GameObject ingredientPanel = MakeIngredientPanel(ingredient);
            ingredientPanel.transform.SetParent(ingredientsPanel, false);
        }

        recipePanel.transform.SetParent(transform);
        recipePanels.Add(recipe, recipePanel);
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

    GameObject MakeIngredient(string ingredientType)
    {
        switch (ingredientType)
        {
            case "Meat":
                return Instantiate(MeatIngredient);
            case "Tomato":
                return Instantiate(TomatoIngredient);
            case "Pasta":
            default:
                return Instantiate(PastaIngredient);
        }
    }

    GameObject MakeIngredientTask(string task)
    {
        GameObject ingredientTask = Instantiate(RecipeIngredientTask);
        Image taskImage = ingredientTask.GetComponent<Image>();

        if (taskColors.ContainsKey(task))
        {
            taskImage.color = taskColors[task];
        }

        return ingredientTask;
    }
	
	void RemoveRecipePanel(Recipe recipe)
    {
        Destroy(recipePanels[recipe]);
        recipePanels.Remove(recipe);
    }

    bool CheckPlateRecipeForChanges(Recipe existingRecipe, Recipe currentRecipe)
    {
        if (existingRecipe != currentRecipe)
        {
            if (existingRecipe != null)
            {
                RemoveRecipePanel(existingRecipe);
            }

            if (currentRecipe != null)
            {
                MakeRecipePanel(currentRecipe);
            }

            return true;
        }

        return false;
    }
    
    // Update is called once per frame
	void Update () {
        foreach (PlateBench plateBench in plateBenches)
        {
            if(CheckPlateRecipeForChanges(plateRecipes[plateBench], plateBench.Recipe))
            {
                plateRecipes[plateBench] = plateBench.Recipe;
            }
        }
	}
}
