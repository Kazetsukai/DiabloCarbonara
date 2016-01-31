using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class IngredientBase : MonoBehaviour
{
	public string Type;

    public Color Color;

	public List<string> TasksDone = new List<string>();
    public List<GameObject> TaskIcons = new List<GameObject>();

    public Sprite[] ProcessStages;

    public GameObject TaskIcon;

    public Sprite FryIcon;
    public Sprite ChopIcon;
    public Sprite BoilIcon;

    private int _currentStage = 0;
	private bool burnt;

	public void Start()
	{
	}

    public void Process(string TaskType)
    {
        TasksDone.Add(TaskType);

        if (ProcessStages != null)
        {
            GetComponent<SpriteRenderer>().sprite = ProcessStages[Math.Min(_currentStage, ProcessStages.Length - 1)];
            GameObject taskIcon = Instantiate(TaskIcon);
            SpriteRenderer taskSprite = taskIcon.GetComponent<SpriteRenderer>();
            switch (TaskType)
            {
                case "Boil":
                    taskSprite.sprite = BoilIcon;
                    break;
                case "Fry":
                    taskSprite.sprite = FryIcon;
                    break;
                case "Chop":
                    taskSprite.sprite = ChopIcon;
                    break;
            }

            taskIcon.transform.SetParent(transform);

            TaskIcons.Add(taskIcon);

            float startingPoint = TaskIcons.Count * (0.3f / 4);

            for (int i = 0; i < TaskIcons.Count; i++)
            {
                TaskIcons[i].transform.localPosition = new Vector3(TaskIcons.Count > 1 ? startingPoint + (-0.3f * i) : 0, -0.5f, -5);
            }

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
