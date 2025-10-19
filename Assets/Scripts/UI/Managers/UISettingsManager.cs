using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu
{
	public class UISettingsManager : MonoBehaviour
	{
		[Header("VIDEO SETTINGS")]
		public GameObject fullscreentext;

		[Header("GAME SETTINGS")]
		public GameObject showhudtext;

		// sliders
		public GameObject musicSlider;


		public void Start()
		{
			// check slider values
			musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");

			// check full screen
			if (Screen.fullScreen == true)
			{
				fullscreentext.GetComponent<TMP_Text>().text = "on";
			}
			else if (Screen.fullScreen == false)
			{
				fullscreentext.GetComponent<TMP_Text>().text = "off";
			}

			// check hud value
			if (PlayerPrefs.GetInt("ShowHUD") == 0)
			{
				showhudtext.GetComponent<TMP_Text>().text = "off";
			}
			else
			{
				showhudtext.GetComponent<TMP_Text>().text = "on";
			}
		}

		public void Update()
		{
			//sliderValue = musicSlider.GetComponent<Slider>().value;
		}

		public void FullScreen()
		{
			Screen.fullScreen = !Screen.fullScreen;

			if (Screen.fullScreen == true)
			{
				fullscreentext.GetComponent<TMP_Text>().text = "on";
			}
			else if (Screen.fullScreen == false)
			{
				fullscreentext.GetComponent<TMP_Text>().text = "off";
			}
		}

		public void MusicSlider()
		{
			//PlayerPrefs.SetFloat("MusicVolume", sliderValue);
			PlayerPrefs.SetFloat("MusicVolume", musicSlider.GetComponent<Slider>().value);
		}

		// the playerprefs variable that is checked to enable hud while in game
		public void ShowHUD()
		{
			if (PlayerPrefs.GetInt("ShowHUD") == 0)
			{
				PlayerPrefs.SetInt("ShowHUD", 1);
				showhudtext.GetComponent<TMP_Text>().text = "on";
			}
			else if (PlayerPrefs.GetInt("ShowHUD") == 1)
			{
				PlayerPrefs.SetInt("ShowHUD", 0);
				showhudtext.GetComponent<TMP_Text>().text = "off";
			}
		}
	}
}