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
	private bool burnt;

	public void Start()
	{
	}

    public void Process()
    {
        if (ProcessStages != null)
        {
            GetComponent<SpriteRenderer>().sprite = ProcessStages[Math.Min(_currentStage, ProcessStages.Length - 1)];

            _currentStage++;
        }
    }

	public void Burn()
	{
		if (ProcessStages != null)
		{
			GetComponent<SpriteRenderer>().sprite = ProcessStages[ProcessStages.Length - 1];
			burnt = true;
		}
	}
}
