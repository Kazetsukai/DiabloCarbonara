using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BenchBase : MonoBehaviour
{
	[Header("Bench base")]
	public Transform IngredientOffsetTransform;
	public IngredientBase contents;
	public Transform HandIKTarget_R;
	public Transform HandIKTarget_L;
	public Player LastInteractedPlayer;

	List<Player> PlayersLookingAtMe = new List<Player>();
	private MusicMaster baseMusicMaster;

	public virtual IngredientBase Interact(Player player, Vector2 input)
	{
		var temp = contents;
		contents = null;
		if (temp != null && baseMusicMaster != null)
		{
			baseMusicMaster.OneShot("pick", transform.position);
		}

		return temp;
	}

	void SetCurrentHighlightColor(Color highlightColor)
	{
		Transform selectionTransform = transform.FindChild("SelectionHighlight");
		if (selectionTransform != null)
		{
			Renderer renderer = selectionTransform.GetComponent<Renderer>();
			renderer.enabled = true;
			Material mat = renderer.material;
			mat.SetColor("_EmissionColor", highlightColor);
			mat.EnableKeyword("_EMISSION");
		}
	}

	void RemoveHighlightColor()
	{
		Transform selectionTransform = transform.FindChild("SelectionHighlight");
		if (selectionTransform != null)
		{
			Renderer renderer = selectionTransform.GetComponent<Renderer>();
			renderer.enabled = false;
		}
	}

	public void AddLookingAt(Player player)
	{
		if (!PlayersLookingAtMe.Contains(player))
		{
			PlayersLookingAtMe.Add(player);
		}

		SetCurrentHighlightColor(player.SelectColor);
	}

	public void RemoveLookingAt(Player player)
	{
		if (PlayersLookingAtMe.Contains(player))
		{
			PlayersLookingAtMe.Remove(player);
		}

		if (PlayersLookingAtMe.Count > 0)
		{
			SetCurrentHighlightColor(PlayersLookingAtMe[PlayersLookingAtMe.Count - 1].SelectColor);
		}
		else
		{
			RemoveHighlightColor();
		}
	}

	public bool Put(IngredientBase item)
	{
		if (baseMusicMaster != null)
		{
			baseMusicMaster.OneShot("drop", transform.position);
		}

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

	public void Start()
	{
		baseMusicMaster = FindObjectOfType<MusicMaster>();
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
