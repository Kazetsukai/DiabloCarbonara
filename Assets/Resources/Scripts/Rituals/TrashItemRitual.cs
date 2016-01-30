using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TrashItemRitual : RitualBase
{
    public string[] ItemTypes;

    public string ItemType;
    
    public override void Randomise()
    {
        ItemType = ItemTypes[Random.Range(0, ItemTypes.Length)];
    }

    public override string Description()
    {
        return "throw a " + ItemType + " in the trash";
    }
}
