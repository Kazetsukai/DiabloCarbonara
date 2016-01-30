using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Ingredient
{
    public string Type;

	public bool IsSatisfied;

	public List<string> Tasks = new List<string>();

	public override string ToString()
	{
		return Type + " (" + string.Join(", ", Tasks.ToArray()) + ")";
	}
}
