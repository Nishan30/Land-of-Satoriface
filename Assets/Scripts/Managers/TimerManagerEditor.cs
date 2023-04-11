using UnityEngine;

public class TimerManagerEditor : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] float time;
    void Update()
    {
        Time.timeScale = time;
    }
#endif
}
