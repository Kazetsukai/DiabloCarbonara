using UnityEngine;
using System.Collections;

public class ProcessItemRitual : RitualBase {
    public string[] ItemTypes;

    public string[] TaskTypes;

    public string ItemType;
    public string TaskType;

    public override void Randomise()
    {
        ItemType = ItemTypes[Random.Range(0, ItemTypes.Length)];
        TaskType = TaskTypes[Random.Range(0, TaskTypes.Length)];
    }

    public override string Description()
    {
        return TaskType + " some " + ItemType;
    }
}
