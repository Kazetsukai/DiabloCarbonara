using UnityEngine;
using System.Collections;

public class BenchBase : MonoBehaviour
{
	public Vector3 IngredientOffset;
	public IngredientBase contents;
	public bool processed;

	public virtual IngredientBase Interact()
	{
		var temp = contents;
		contents = null;
		return temp;
	}

	public bool Put(IngredientBase item)
	{

		// Am I full?
		if (contents != null)
			return false;

		// Can I receive this item?
		if (!CanIReceive(item))
			return false;

		// Get it
		item.gameObject.transform.position = transform.position + IngredientOffset;
		item.gameObject.transform.parent = transform;
		contents = item;

		return true;
	}

	public virtual bool CanIReceive(IngredientBase item)
	{
		return true;
	}

	public void Update()
	{
		if (contents != null)
		{
			Debug.DrawLine(transform.position, transform.position + Vector3.up * 2, Color.red);
		}
	}
}
