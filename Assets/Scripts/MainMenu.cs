using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	public void StartGame()
	{
		Application.LoadLevel(1);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
