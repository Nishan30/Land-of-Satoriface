using System;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

public class SoundManager : SingleTon<SoundManager>
{
    public AudioSource oneShotAudioSource;
    public override void main() { }
}

[Serializable]
public class AudioClipPreset
{
    [SerializeField] List<AudioClip> clips = new();
    [SerializeField] bool looping = false;
    public AudioSource AudioSource3d = null;
    public void play()
    {
        if (clips == null || clips.Count <= 0)
            return;
        if (AudioSource3d == null)
        {
            SoundManager.Instance.oneShotAudioSource.PlayOneShot(clips[Random.Range(0, clips.Count)]);
        }
        else
        {
            AudioSource3d.loop = looping;
            if (looping == false)
                AudioSource3d.PlayOneShot(clips[Random.Range(0, clips.Count)]);
            else
            {
                if (AudioSource3d.isPlaying)
                {
                    if (clips.Contains(AudioSource3d.clip))
                        return;
                    AudioSource3d.Stop();
                }
                AudioSource3d.clip = clips[Random.Range(0, clips.Count)];
                AudioSource3d.Play();
            }
        }
    }

    public void Stop()
    {
        AudioSource3d.Stop();
    }
}