
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class RitualBase : MonoBehaviour
{
    public bool Satisfied { get; set; }

    public virtual void Randomise()
    {

    }

    public virtual string Description()
    {
        return "something something something";
    }
}
