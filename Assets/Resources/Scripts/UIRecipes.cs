using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UIRecipes : MonoBehaviour {
    readonly Dictionary<string, Color> taskColors = new Dictionary<string, Color>()
    {
        { "Chop", new Color(0,1,0) },
        { "Boil", new Color(0,0,1) },
        { "Fry", new Color(1,0,0) }
    };

    readonly float fadeOutRate = 0.015f;

    PlateBench[] plateBenches;
    Dictionary<PlateBench, Recipe> plateRecipes;
    Dictionary<Recipe, GameObject> recipePanels = new Dictionary<Recipe, GameObject>();

    public GameObject RecipePanel;
    public GameObject RecipeIngredient;
    public GameObject RecipeIngredientTask;

    public Sprite PastaIngredient;
    public Sprite TomatoIngredient;
    public Sprite MeatIngredient;

    public Sprite ChopIcon;
    public Sprite BoilIcon;
    public Sprite FryIcon;

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

        Vector3 pos = plateBench.transform.position + Vector3.up * 3.5f;
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

        CanvasRenderer[] renderers = ingredientPanel.GetComponentsInChildren<CanvasRenderer>();

        foreach (CanvasRenderer renderer in renderers)
        {
            renderer.SetAlpha(0.6f);
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

    Sprite MakeTask(string taskType)
    {
        switch (taskType)
        {
            case "Boil":
                return BoilIcon;
            case "Fry":
                return FryIcon;
            case "Chop":
            default:
                return ChopIcon;
        }
    }

    GameObject MakeIngredientTask(string task)
    {
        GameObject ingredientTask = Instantiate(RecipeIngredientTask);
        Image taskImage = ingredientTask.GetComponent<Image>();

        taskImage.sprite = MakeTask(task);

        return ingredientTask;
    }

    void PartialCompleteRecipePanel(GameObject recipePanel, int ingredientIndex)
    {
        Transform ingredientPanel = recipePanel.transform.GetChild(ingredientIndex);
        CanvasRenderer[] renderers = ingredientPanel.GetComponentsInChildren<CanvasRenderer>();

        foreach (CanvasRenderer renderer in renderers)
        {
            renderer.SetAlpha(1f);
        }
    }

    void CompleteRecipePanel(Recipe recipe, GameObject recipePanel)
    {
        CanvasRenderer[] renderers = recipePanel.GetComponentsInChildren<CanvasRenderer>();
        float currentAlpha = renderers[0].GetAlpha();

        if (currentAlpha > 0)
        {
            foreach (CanvasRenderer renderer in renderers)
            {
                renderer.SetAlpha(currentAlpha - fadeOutRate);
            }
        }
        else
        {
            RemoveRecipePanel(recipe);
        }
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
            if (existingRecipe != null && currentRecipe != null)
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

        foreach (KeyValuePair<Recipe, GameObject> recipePanel in recipePanels)
        {
            if (recipePanel.Key.IsDone())
            {
                CompleteRecipePanel(recipePanel.Key, recipePanel.Value);
            }
            else
            {
                for (int i = 0; i < recipePanel.Key.Ingredients.Count; i++)
                {
                    if (recipePanel.Key.Ingredients[i].IsSatisfied)
                    {
                        PartialCompleteRecipePanel(recipePanel.Value, i);
                    }
                }
            }
        }
	}
}
