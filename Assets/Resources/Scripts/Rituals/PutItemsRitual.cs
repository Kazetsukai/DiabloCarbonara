using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class PutItemsRitual : RitualBase {
    public string[] ItemTypes;

    public string ItemType;
    public int NumBenches;
     
    public override void Randomise()
    {
        ItemType = ItemTypes[UnityEngine.Random.Range(0, ItemTypes.Length)];
        //because all benches are derived from BenchBase, need to check that they are actually just BenchBase to get normal benches
        int totalBenches = FindObjectsOfType<BenchBase>().Where(b => b.GetType() == typeof(BenchBase)).Count();

        NumBenches = UnityEngine.Random.Range((totalBenches / 2), NumBenches + 1);
    }

    public int GetBenchesCompleted()
    {
        return FindObjectsOfType<BenchBase>().Where(b => b.GetType() == typeof(BenchBase) && b.contents != null && b.contents.Type.ToLower() == ItemType).Count();
    }

    public override string Description()
    {
        return "put " + ItemType + " on " + NumBenches.ToString() + " empty bench" + (NumBenches > 1 ? "es" : "");
    }
}
