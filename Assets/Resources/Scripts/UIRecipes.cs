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

    public Sprite PastaIngredient;
    public Sprite TomatoIngredient;
    public Sprite MeatIngredient;

    // Use this for initialization
    void Start () {
        plateBenches = FindObjectsOfType<PlateBench>();
        plateRecipes = plateBenches.ToDictionary(p => p, p => (Recipe)null);
    }

    void MakeRecipePanel(Recipe recipe, PlateBench plateBench)
    {
        GameObject recipePanel = Instantiate(RecipePanel);

        foreach (Ingredient ingredient in recipe.Ingredients)
        {
            GameObject ingredientPanel = MakeIngredientPanel(ingredient);
            ingredientPanel.transform.SetParent(recipePanel.transform, false);
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        recipePanel.transform.SetParent(canvas.transform);

        Vector3 pos = plateBench.transform.position + Vector3.up * 3;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        recipePanel.transform.position = screenPos;

        recipePanels.Add(recipe, recipePanel);
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

    bool CheckPlateRecipeForChanges(Recipe existingRecipe, Recipe currentRecipe, PlateBench plateBench)
    {
        if (existingRecipe != currentRecipe)
        {
            if (existingRecipe != null)
            {
                RemoveRecipePanel(existingRecipe);
            }

            if (currentRecipe != null)
            {
                MakeRecipePanel(currentRecipe, plateBench);
            }

            return true;
        }

        return false;
    }
    
    // Update is called once per frame
	void Update () {
        foreach (PlateBench plateBench in plateBenches)
        {
            if(CheckPlateRecipeForChanges(plateRecipes[plateBench], plateBench.Recipe, plateBench))
            {
                plateRecipes[plateBench] = plateBench.Recipe;
            }
        }
	}
}
