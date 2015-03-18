using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	public Color dimColor = new Color(0, 0, 0, .5f);
	public float dimSpeed = 4;
	public Image dimmer;

	private bool _isDimmed = false;
	private OptionsMenu _options;

	void Start()
	{
		_options = GetComponent<OptionsMenu>();
		_options.LoadPrefs();
		_options.optionsMenu.SetActive(false);
	}

	void Update()
	{
		if(_isDimmed)
			dimmer.color = Color.Lerp(dimmer.color, dimColor, Time.deltaTime * dimSpeed);
		else
			dimmer.color = Color.clear;
	}

	public void StartGame()
	{
		Application.LoadLevel(1);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void ToggleDim()
	{
		_isDimmed = !_isDimmed;
	}
}
