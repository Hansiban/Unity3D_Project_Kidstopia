using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{ // Cavase Audio Setting�� �־��ֱ�
    public Slider bgmSlider, sfxSlider;

    public void MuteBGM()
    {
        AudioManager.instance.MuteBGM();
    }

    public void MuteSFX()
    {
        AudioManager.instance.MuteSFX();
    }

    public void BGMVolume()
    {
        AudioManager.instance.BGMVolume(bgmSlider.value);
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
    }

    public void LowQuality()
    {
        QualitySettings.SetQualityLevel(0);
    }

    public void MiddleQuality()
    {
        QualitySettings.SetQualityLevel(1);
    }

    public void HighQuality()
    {
        QualitySettings.SetQualityLevel(2);
    }
}
