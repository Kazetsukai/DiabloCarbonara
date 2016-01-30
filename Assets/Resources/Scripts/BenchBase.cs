using UnityEngine;
using System.Collections;

public class BenchBase : MonoBehaviour
{
	public Vector3 IngredientOffset;
	public IngredientBase contents;
    
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
        item.gameObject.transform.parent = transform;
        StartCoroutine(LerpItemPosition(item, item.gameObject.transform.position, transform.position + IngredientOffset, 0.2f));
	
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

    public IEnumerator LerpItemPosition(IngredientBase itemToLerp, Vector3 startPos, Vector3 endPos, float duration)
    {
        float t_elapsed = 0;
        do
        {
            t_elapsed += Time.deltaTime;
            itemToLerp.gameObject.transform.position = Vector3.Lerp(startPos, endPos, t_elapsed / duration);
            yield return null;
        }
        while (t_elapsed / duration < 1f);  
    }
}
