using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private Button playButton;
    [SerializeField] private AndroidNotificationsHandler androidNotificationsHandler;
    [SerializeField] private IOSNotificationsHandler iOSNotificationsHandler;
    [SerializeField] private int maxEnergy = 5;
    [SerializeField] private int energyRachargeDuration = 2;

    private int energy;

    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";

    private void Start() 
    {
        OnApplicationFocus(true);
    }

    private void OnApplicationFocus(bool hasFocus) 
    {
        if(!hasFocus) { return; }

        CancelInvoke();
        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highScoreText.text = $"High score: {highScore}";
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if(energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);
            if(energyReadyString == string.Empty) { return; }
            DateTime energyReady = DateTime.Parse(energyReadyString);
            if(DateTime.Now > energyReady)
            {
                energy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, energy);
            }
            else
            {
                playButton.interactable = false;
                Invoke(nameof(EnergyRecharged), (energyReady - DateTime.Now).Seconds);
            }
        }
        energyText.text = $"Play ({energy})";
    }

    private void EnergyRecharged()
    {
        playButton.interactable = true;
        energy = maxEnergy;
        PlayerPrefs.SetInt(EnergyKey, energy);
        energyText.text = $"Play ({energy})"; 
    }

    public void Play()
    {
        if(energy == 0) { return; }
        if(energy > 0)
        {
            energy--;
            PlayerPrefs.SetInt(EnergyKey, energy);
            if(energy == 0)
            {
                DateTime energyReady = DateTime.Now.AddMinutes(energyRachargeDuration);
                PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());
#if UNITY_ANDROID
                androidNotificationsHandler.ScheduleNotification(energyReady);
#elif UNITY_IOS
                iOSNotificationsHandler.ScheduleNotification(energyRachargeDuration);
#endif
            }
        }

        SceneManager.LoadScene(1);
    }
}
