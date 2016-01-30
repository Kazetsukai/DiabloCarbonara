using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientBase : MonoBehaviour
{
	public string Type;

	public Color Color;

	public List<string> TasksDone = new List<string>();

	public void Start()
	{
		Color = GetComponent<MeshRenderer>().material.color;
	}
}
