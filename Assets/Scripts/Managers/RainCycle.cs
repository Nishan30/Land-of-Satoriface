using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainCycle : MonoBehaviour
{
    [SerializeField] GameObject rainBar;
    [SerializeField] ParticleSystem rainObj;
    [SerializeField] float toggleInSeconds;

    float targetHalfTime;
    float targetTime;

    private void Awake()
    {
        setTimer();
    }
    private void Update()
    {
        if (targetTime > targetHalfTime && targetTime - Time.deltaTime <= targetHalfTime)
        {
            if (rainObj.isPlaying)
                rainObj.Stop();
            else
                rainObj.Play();
        }
        targetTime -= Time.deltaTime;
        float factor = targetTime / targetHalfTime;
        if (factor > 1)
        {
            factor = 1 - (factor % 1);
        }
        rainBar.transform.localScale = new Vector3(factor, 1, 1);

        if (factor >= 1 || factor <= 0)
        {
            if (rainObj.isPlaying)
                rainObj.Stop();
            else
                rainObj.Play();
        }

        if (targetTime <= 0)
            setTimer();
    }

    private void setTimer()
    {
        targetTime = toggleInSeconds * 2;
        targetHalfTime = toggleInSeconds;
    }
}
