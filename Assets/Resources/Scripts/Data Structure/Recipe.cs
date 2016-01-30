using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Recipe
{
    public float TimeElapsed;
    public float MaxTime;
  
    public Recipe()
    {
        
    }

    public int Complexity
    {
        get
        {
            return Ingredients.Sum(i => i.Tasks.Count + 1);
        }
    }

    public List<Ingredient> Ingredients = new List<Ingredient>();

	public bool IsDone()
	{
		return Ingredients.All(i => i.IsSatisfied);
	}

	public override string ToString()
	{
		return string.Join(" + ", Ingredients.Select(i => i.ToString()).ToArray());
	}
}
