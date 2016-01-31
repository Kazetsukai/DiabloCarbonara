
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BurnPunishment : PunishmentBase
{
    public override void Randomise()
    {

    }

    public override string Description()
    {
        return "burn all your food";
    }

    public override void ExecutePunishment()
    {
        foreach (var food in FindObjectsOfType<IngredientBase>().Where(f => !FindObjectsOfType<PlateBench>().Any(b => b.contents == f)))
        {
            food.Burn();
        }
    }
}
