using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace EndlessRunner.Menus
{
    public class SettingsMenu : MonoBehaviour
    {
        #region Variables

        [Header("Screen Settings")]
        public Resolution[] resolutions;
        public Dropdown resolutionDropdown;
        public Dropdown qualityDropdown;
        public Toggle fullscreenToggle;

        [Header("Sound Settings")]
        public AudioMixer mixer;
        public Slider soundFXSlider;
        public Slider musicSlider;
        public Slider backSoundSlider;

        #endregion

        #region Default Methods
        private void Start()
        {
            // add options to dropdowns
            AddResolutionOptions();
            AddQualityOptions();

            LoadPlayerPrefs();

            // setup initial values shown to user
            UpdateFullScreenToggle();
            UpdateQualityDropdown();

            PlayerPrefs.Save();
        }
        #endregion

        #region Custom Methods

        #region Add Options & Choose Value to Show (Dropdowns & Toggles)
        /// <summary>
        /// Add all available screen resolution options for the user's screen.
        /// </summary>
        private void AddResolutionOptions()
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(option);
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = i;
                }
                resolutionDropdown.AddOptions(options);
                resolutionDropdown.value = currentResolutionIndex;
                resolutionDropdown.RefreshShownValue();
            }
        }

        /// <summary>
        /// Add all available quality settings options for the current project.
        /// </summary>
        private void AddQualityOptions()
        {
            qualityDropdown.ClearOptions();
            List<string> options = new List<string>();
            string[] names = QualitySettings.names;
            for (int i = 0; i < names.Length; i++)
            {
                options.Add(names[i]);
            }
            qualityDropdown.AddOptions(options);
            qualityDropdown.RefreshShownValue();
        }

        private static void UpdateQualityDropdown()
        {
            if (!PlayerPrefs.HasKey("quality"))
            {
                // set default value to "Ultra" which is 6th value in the array
                PlayerPrefs.SetInt("quality", 5);
                QualitySettings.SetQualityLevel(5);
            }
            else
            {
                QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
            }
        }

        private static void UpdateFullScreenToggle()
        {
            if (!PlayerPrefs.HasKey("fullscreen"))
            {
                PlayerPrefs.SetInt("fullscreen", 1);
                Screen.fullScreen = true;

            }
            else
            {
                if (PlayerPrefs.GetInt("fullscreen") == 0)
                {
                    Screen.fullScreen = false;
                }
                else
                {
                    Screen.fullScreen = true;
                }
            }
        }
        #endregion

        #region Set Settings
        /// <summary>
        /// Change screen resolution.
        /// </summary>
        /// <param name="resolutionIndex">Int variable showing which resolution user have chosen.</param>
        public void SetResolution(int resolutionIndex)
        {
            Resolution res = resolutions[resolutionIndex];
            Screen.SetResolution(res.width, res.height, false);
        }

        /// <summary>
        /// Change screen from fullscreen to windowed.
        /// </summary>
        /// <param name="fullscreen">Bool variable showing user input.</param>
        public void SetFullScreen(bool fullscreen)
        {
            Screen.fullScreen = fullscreen;
        }

        /// <summary>
        /// Change quality settings.
        /// </summary>
        /// <param name="index">Number of the option chosen from the dropdown.</param>
        public void SetQuality(int index)
        {
            QualitySettings.SetQualityLevel(index);
        }

        /// <summary>
        /// Change SoundFX Volume.
        /// </summary>
        /// <param name="value">Value from the slider.</param>
        public void SetSoundFXVolume(float value)
        {
            mixer.SetFloat("SoundFXVol", value);
        }

        /// <summary>
        /// Change Music Volume.
        /// </summary>
        /// <param name="value">Value from the slider.</param>
        public void SetMusicVolume(float value)
        {
            mixer.SetFloat("MusicVol", value);
        }

        /// <summary>
        /// Chage Background Sound Volume.
        /// </summary>
        /// <param name="value">Value from the slider.</param>
        public void SetBackgroundSoundVolume(float value)
        {
            mixer.SetFloat("BackSoundVol", value);
        }
        #endregion

        #region Save Settings
        /// <summary>
        /// Save all player settings.
        /// </summary>
        public void SavePlayerPrefs()
        {
            SaveQualitySettings();
            SaveFullScreenSettings();
            SaveMusicVolume();
            SaveSoundFXVolume();
            SaveBackgroundSoundVolume();

            PlayerPrefs.Save();
        }

        /// <summary>
        /// Save background sound volume in player prefs.
        /// </summary>
        private void SaveBackgroundSoundVolume()
        {
            float backSoundVol;
            mixer.GetFloat("BackSoundVol", out backSoundVol);
            PlayerPrefs.SetFloat("BackSoundVol", backSoundVol);
        }

        /// <summary>
        /// Save SoundFX sound volume in player prefs.
        /// </summary>
        private void SaveSoundFXVolume()
        {
            float soundFXVol;
            mixer.GetFloat("SoundFXVol", out soundFXVol);
            PlayerPrefs.SetFloat("SoundFXVol", soundFXVol);
        }

        /// <summary>
        /// Save music volume in player prefs.
        /// </summary>
        private void SaveMusicVolume()
        {
            float musicVol;
            mixer.GetFloat("MusicVol", out musicVol);
            PlayerPrefs.SetFloat("MusicVol", musicVol);
        }

        /// <summary>
        /// Save full screen preference settings in player prefs.
        /// </summary>
        private void SaveFullScreenSettings()
        {
            if (fullscreenToggle.isOn)
            {
                PlayerPrefs.SetInt("fullscreen", 1);
            }
            else
            {
                PlayerPrefs.SetInt("fullscreen", 0);
            }
        }

        /// <summary>
        /// Save quality settings in player prefs.
        /// </summary>
        private static void SaveQualitySettings()
        {
            PlayerPrefs.SetInt("quality", QualitySettings.GetQualityLevel());
            //PlayerPrefs.SetInt("quality", qualityDropdown.value);
        }
        #endregion

        #region Load Settings
        /// <summary>
        /// Load all player settings.
        /// </summary>
        public void LoadPlayerPrefs()
        {
            LoadQualitySettings();
            LoadFullScreenSettings();
            LoadAudioSettings();
        }

        /// <summary>
        /// Load full screen preference settings from player prefs.
        /// </summary>
        private void LoadFullScreenSettings()
        {
            if (PlayerPrefs.HasKey("fullscreen"))
            {
                if (PlayerPrefs.GetInt("fullscreen") == 0)
                {
                    fullscreenToggle.isOn = false;
                }
                else
                {
                    fullscreenToggle.isOn = true;
                }
            }
        }

        /// <summary>
        /// Load quality settings from player prefs.
        /// </summary>
        private void LoadQualitySettings()
        {
            if (PlayerPrefs.HasKey("quality"))
            {
                int quality = PlayerPrefs.GetInt("quality");
                qualityDropdown.value = quality;
                if (QualitySettings.GetQualityLevel() != quality)
                {
                    SetQuality(quality);
                }
            }
        }

        /// <summary>
        /// Load all audio settings.
        /// </summary>
        private void LoadAudioSettings()
        {
            LoadMusicVolumeSettings();
            LoadSoundFXVolumeSettings();
            LoadBackgroundSoundVolumeSettings();
        }

        /// <summary>
        /// Load background sound volume settings from player prefs.
        /// </summary>
        private void LoadBackgroundSoundVolumeSettings()
        {
            if (PlayerPrefs.HasKey("BackSoundVol"))
            {
                float backSoundVol = PlayerPrefs.GetFloat("BackSoundVol");
                backSoundSlider.value = backSoundVol;
                mixer.SetFloat("BackSoundVol", backSoundVol);
            }
            else
            {
                // maximum volume is 0 (range is from -80 to 0)
                float maxVol = 0;
                backSoundSlider.value = maxVol;
                mixer.SetFloat("BackSoundVol", maxVol);
            }
        }

        /// <summary>
        /// Load soundFX volume settings from player prefs.
        /// </summary>
        private void LoadSoundFXVolumeSettings()
        {
            if (PlayerPrefs.HasKey("SoundFXVol"))
            {
                float soundFXVol = PlayerPrefs.GetFloat("SoundFXVol");
                soundFXSlider.value = soundFXVol;
                mixer.SetFloat("SoundFXVol", soundFXVol);
            }
            else
            {
                // maximum volume is 0 (range is from -80 to 0)
                float maxVol = 0;
                soundFXSlider.value = maxVol;
                mixer.SetFloat("SoundFXVol", maxVol);
            }
        }

        /// <summary>
        /// Load music volume settings from player prefs.
        /// </summary>
        private void LoadMusicVolumeSettings()
        {
            if (PlayerPrefs.HasKey("MusicVol"))
            {
                float musicVol = PlayerPrefs.GetFloat("MusicVol");
                musicSlider.value = musicVol;
                mixer.SetFloat("MusicVol", musicVol);
            }
            else
            {
                // maximum volume is 0 (range is from -80 to 0)
                float maxVol = 0;
                musicSlider.value = maxVol;
                mixer.SetFloat("MusicVol", maxVol);
            }
        }
        #endregion

        #endregion
    }
}
