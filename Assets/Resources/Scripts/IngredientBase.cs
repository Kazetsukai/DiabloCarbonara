using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class IngredientBase : MonoBehaviour
{
	public string Type;

	public Color Color;

	public List<string> TasksDone = new List<string>();

    public Sprite[] ProcessStages;

    private int _currentStage = 0;

	public void Start()
	{
		Color = GetComponent<MeshRenderer>().material.color;
	}

    public void Process()
    {
        if (ProcessStages != null)
        {
            GetComponent<SpriteRenderer>().sprite = ProcessStages[Math.Min(_currentStage, ProcessStages.Length - 1)];

            _currentStage++;
        }
    }
}
