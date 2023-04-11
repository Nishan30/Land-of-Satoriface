using UnityEngine;

public class AudioClipPresetAttached : MonoBehaviour
{
    [SerializeField] AudioClipPreset audioClipPreset;

    public void play()
    {
        audioClipPreset.play();
    }
}