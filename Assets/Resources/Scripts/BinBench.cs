using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class BinBench : BenchBase
{
	private MusicMaster musicMaster;

	public override IngredientBase Interact(Player player, Vector2 input)
    {
		return null;
	}

	public void Start()
	{
		musicMaster = FindObjectOfType<MusicMaster>();
	}

	public override bool CanIReceive(IngredientBase item)
	{
		musicMaster.OneShot("bin", transform.position);
		return true;
	}

	public override void GetItem(IngredientBase item)
    {
        var ritual = FindObjectOfType<RitualMaster>().RemainingRituals<TrashItemRitual>().Where(r => r.ItemType == item.Type.ToLower()).FirstOrDefault();
        if (ritual != null)
            ritual.Satisfied = true;
    }
}
