using UnityEngine;
using System.Collections;

public class VoidUtils : MonoBehaviour {

	public static float Round(float n, int d)
	{
		return Mathf.Round(n*d)/d;
	}
}
