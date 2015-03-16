using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	public GameObject optionsMenu;
	public Text musicVolNum;
	public Text effectVolNum;
	public Text masterVolNum;
	public Slider musicVol;
	public Slider masterVol;
	public Slider effectVol;
	public Toggle VSync;
	public bool _isOptions;

	private float _masterVol;
	private float _musicVol;
	private float _effectVol;
	private int _VSync;

	public void LoadPrefs()
	{
		masterVol.value = _masterVol = PlayerPrefs.GetFloat("MasterVol", 100);
		masterVolNum.text = _masterVol.ToString();
		effectVol.value = _effectVol = PlayerPrefs.GetFloat("EffectVol", 100);
		effectVolNum.text = _effectVol.ToString();
		musicVol.value = _musicVol = PlayerPrefs.GetFloat("MusicVol", 100);
		musicVolNum.text = _effectVol.ToString();
		_VSync = PlayerPrefs.GetInt("VSync", 1);
		if(_VSync > 0)
			VSync.isOn = true;
		else
			VSync.isOn = false;
		ApplyOptions();
	}

	public void ToggleOptions()
	{
		_isOptions = !_isOptions;
		optionsMenu.SetActive(_isOptions);
	}

	public void OptionsCancel()
	{
		LoadPrefs();
		ApplyOptions();
		ToggleOptions();
	}
	
	public void OptionsApply()
	{
		ApplyOptions();
		ToggleOptions();
	}

	void ApplyOptions()
	{
		GameRegistry.EFFECT_VOL = _effectVol/100;
		GameRegistry.MUSIC_VOL = _musicVol/100;
		
		SoundController[] audio = GameObject.FindObjectsOfType<SoundController>() as SoundController[];
		foreach(SoundController s in audio)
		{
			s.UpdateVolumes();
		}
		
		QualitySettings.vSyncCount = _VSync;
		
		PlayerPrefs.SetFloat("MasterVol", _masterVol);
		PlayerPrefs.SetFloat("EffectVol", _effectVol);
		PlayerPrefs.SetFloat("MusicVol", _musicVol);
		PlayerPrefs.SetInt("VSync", _VSync);

	}

	public void SetMasterVol(float vol)
	{
		AudioListener.volume = 100/_masterVol;
		_masterVol = vol;
		masterVolNum.text = vol.ToString();
	}
	
	public void SetEffectVol(float vol)
	{
		_effectVol = vol;
		effectVolNum.text = vol.ToString();
	}
	
	public void SetMusicVol(float vol)
	{
		_musicVol = vol;
		musicVolNum.text = vol.ToString();
	}
	
	public void SetVSync(bool sync)
	{
		if(sync)
			_VSync = 1;
		else
			_VSync = 0;
	}
}
