using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TrashItemRitual : RitualBase
{
    public string[] ItemDescriptions;

    public string ItemDescription;
    
    public override void Randomise()
    {
        ItemDescription = ItemDescriptions[Random.Range(0, ItemDescriptions.Length)];
    }

    public override string Description()
    {
        return "throw " + ItemDescription + " in the trash";
    }
}
