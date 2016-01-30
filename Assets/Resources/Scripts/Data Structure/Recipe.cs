﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Recipe {
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

    public List<Ingredient> Ingredients;

	public bool IsDone()
	{
		return Ingredients.All(i => i.IsSatisfied);
	}
}
