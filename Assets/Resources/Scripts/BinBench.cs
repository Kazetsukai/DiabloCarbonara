using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class BinBench : BenchBase
{
	public override IngredientBase Interact(Player player, Vector2 input)
    {
		return null;
	}

	public override bool CanIReceive(IngredientBase item)
	{
		return true;
	}

	public override void GetItem(IngredientBase item) { }
}
