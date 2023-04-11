using UnityEngine;

public class Play3DSoundAttached : MonoBehaviour
{
    [SerializeField] AudioClipPreset clips;
    public void play3D()
    {
        clips.play();
    }

    private void Reset()
    {
        clips.AudioSource3d = GetComponent<AudioSource>();
    }
}
