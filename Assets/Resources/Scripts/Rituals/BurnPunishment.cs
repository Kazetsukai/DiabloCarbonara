
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BurnPunishment : MonoBehaviour
{
    public virtual void Randomise()
    {

    }

    public virtual string Description()
    {
        return "burn all your food";
    }

    public virtual void ExecutePunishment()
    {
        foreach (var food in FindObjectsOfType<IngredientBase>())
        {
            food.Burn();
        }
    }
}
