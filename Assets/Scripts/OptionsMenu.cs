using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
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
	public Toggle Bloom;
	public bool _isOptions;

	private float _masterVol;
	private float _musicVol;
	private float _effectVol;
	private int _VSync;
	private int _Bloom;

	public void LoadPrefs()
	{
		masterVol.value = _masterVol = PlayerPrefs.GetFloat("MasterVol", 100);
		masterVolNum.text = _masterVol.ToString();
		effectVol.value = _effectVol = PlayerPrefs.GetFloat("EffectVol", 100);
		effectVolNum.text = _effectVol.ToString();
		musicVol.value = _musicVol = PlayerPrefs.GetFloat("MusicVol", 100);
		musicVolNum.text = _effectVol.ToString();
		_VSync = PlayerPrefs.GetInt("VSync", 1);
		_Bloom = PlayerPrefs.GetInt("Bloom", 1);
		VSync.isOn = VoidUtils.IntBool(_VSync);
		Bloom.isOn = VoidUtils.IntBool(_Bloom);
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
		//Volumes
		GameRegistry.EFFECT_VOL = (_effectVol/100);
		GameRegistry.MUSIC_VOL = (_musicVol/100);
		SoundController[] audio = GameObject.FindObjectsOfType<SoundController>() as SoundController[];
		foreach(SoundController s in audio)
		{
			s.UpdateVolumes();
		}
		//Bloom
		Camera.main.GetComponent<BloomOptimized>().enabled = VoidUtils.IntBool(_Bloom);
		//VSync
		QualitySettings.vSyncCount = _VSync;
		//Save Settings
		PlayerPrefs.SetFloat("MasterVol", _masterVol);
		PlayerPrefs.SetFloat("EffectVol", _effectVol);
		PlayerPrefs.SetFloat("MusicVol", _musicVol);
		PlayerPrefs.SetInt("VSync", _VSync);
		PlayerPrefs.SetInt("Bloom", _Bloom);

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
		_VSync = VoidUtils.BoolInt(sync);
	}

	public void SetBloom(bool bloom)
	{
		_Bloom = VoidUtils.BoolInt(bloom);
	}
}
