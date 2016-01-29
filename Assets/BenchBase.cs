using UnityEngine;
using System.Collections;

public class BenchBase : MonoBehaviour
{
	public IngredientBase contents { get; set; }
	public bool processed { get; private set; }

	public virtual IngredientBase Interact()
	{
		return contents;
	}

	public bool Put(IngredientBase item)
	{

		// Am I full?
		if (contents != null)
			return false;

		// Can I receive this item?
		if (!CanIReceive(item))
			return false;

		return true;
	}

	public virtual bool CanIReceive(IngredientBase item)
	{
		return true;
	}
}
