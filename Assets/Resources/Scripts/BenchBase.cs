using UnityEngine;
using System.Collections;

public class BenchBase : MonoBehaviour
{
    [Header("Bench base")]
	public Transform IngredientOffsetTransform;
	public IngredientBase contents;
    public Transform HandIKTarget_R;
    public Transform HandIKTarget_L;
            
	public virtual IngredientBase Interact(Player player, Vector2 input)
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

		// Move it
		item.gameObject.transform.parent = transform;
		StartCoroutine(LerpItemPosition(item, item.gameObject.transform.position, IngredientOffsetTransform.position, 0.2f));

		// Get it
		GetItem(item);

		return true;
	}

	public virtual void GetItem(IngredientBase item)
	{
		contents = item;
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

    IEnumerator LerpItemPosition(IngredientBase itemToLerp, Vector3 startPos, Vector3 endPos, float duration)
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
