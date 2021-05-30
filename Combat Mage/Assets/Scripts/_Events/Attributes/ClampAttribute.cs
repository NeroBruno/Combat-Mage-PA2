using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampAttribute : PropertyAttribute
{
	public readonly Vector2 ClampLimits;


	public ClampAttribute(float min, float max)
	{
		ClampLimits = new Vector2(min, max);
	}
}