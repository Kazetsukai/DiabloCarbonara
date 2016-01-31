
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PunishmentBase : MonoBehaviour
{
    public virtual void Randomise()
    {

    }

    public virtual string Description()
    {
        return "do something bad to you";
    }

    public virtual void ExecutePunishment()
    {

    }
}
