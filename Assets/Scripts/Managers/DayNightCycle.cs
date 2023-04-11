using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] Transform directionalLight;
    [SerializeField] float hours = 5;
    [SerializeField] UnityEngine.UI.Image dayAndNightSymbol;
    [SerializeField] Sprite alternateSprite;
    [SerializeField] Light moonLight, SunLight;
    [SerializeField] float maxMoonIntensity, maxSunIntensity;
    [SerializeField] Color NightAmbientLight, DayAmbientLight;
    [SerializeField] AnimationCurve lightChangeCurve;
    [SerializeField] TextMeshProUGUI clockCounter,CLS_Text;

    float seconds = 0;
    float secondsHalfDay = 0, DaysPassed = 0;
    bool TimeAM;
    private double _lastMinutes = 0;

    private void Awake()
    {
        seconds = hours * 3600;
        secondsHalfDay = seconds / 2f;
        StartCoroutine(dayAndNightImageSwitch());
    }
    private void Update()
    {
        directionalLight.Rotate(new Vector3(360 * (Time.deltaTime / seconds), 0, 0), Space.Self);
        updateIntensity();
        dayCounting();
        CST_TextCounting();
    }

    private void CST_TextCounting()
    {
        StaticGamemanager.gameDataStructure.CLS += TimeSpan.FromSeconds(Time.deltaTime);
        if(StaticGamemanager.gameDataStructure.CLS.TotalMinutes != _lastMinutes)
        {
            StaticGamemanager.SaveGameDataStructure();
            _lastMinutes = StaticGamemanager.gameDataStructure.CLS.TotalMinutes;
            CLS_Text.text = "<align=left><line-height=0>CLS:<align=right>\n"+ StaticGamemanager.gameDataStructure.CLS.ToString(@"hh\:mm");
        }
    }

    IEnumerator dayAndNightImageSwitch()
    {
        float _seconds = hours * 1800;
        yield return new WaitForSeconds(_seconds);
        switchDayNightImage();
        StartCoroutine(dayAndNightImageSwitch());
        yield break;
    }

    private void switchDayNightImage()
    {
        var lastSprite = dayAndNightSymbol.sprite;
        dayAndNightSymbol.sprite = alternateSprite;
        alternateSprite = lastSprite;
    }

    private void updateIntensity()
    {
        float dotProduct = Vector3.Dot(directionalLight.forward, Vector3.down);
        SunLight.intensity = Mathf.Lerp(0, maxSunIntensity, lightChangeCurve.Evaluate(dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(NightAmbientLight, DayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
    }

    private void dayCounting()
    {
        if (secondsHalfDay <= 0)
        {
            secondsHalfDay = seconds / 2f;
            DaysPassed += 0.5f;
            TimeAM = !TimeAM;
            return;
        }
        else
        {
            secondsHalfDay -= Time.deltaTime;
        }
        float timeBeforeOffset = ((seconds / 2) - secondsHalfDay);
        string clockTextStr = "<align=left>" + "Day " + DaysPassed.ToString("0.0");
        clockTextStr += "<line-height=0>\n<align=right>";
        clockTextStr += System.TimeSpan.FromSeconds(timeBeforeOffset).ToString(@"hh\:mm") + ((TimeAM) ? " AM" : " PM") + "<line-height=1em>";
        clockCounter.text = clockTextStr;
    }

}
