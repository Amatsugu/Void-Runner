using UnityEngine;
using System.Collections;

public class VoidUtils : MonoBehaviour {

	public static float Round(float n, int d)
	{
		return Mathf.Round(n*d)/d;
	}

	public static bool IntBool(int i)
	{
		return i > 0 ? true:false;
	}

	public static int BoolInt(bool b)
	{
		return b ? 1 : 0;
	}
}
