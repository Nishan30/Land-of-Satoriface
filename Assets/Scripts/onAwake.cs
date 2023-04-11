using UnityEngine;
using UnityEngine.Events;

public class onAwake : MonoBehaviour
{
    [SerializeField] UnityEvent OnAwake;

    private void Awake()
    {
        OnAwake?.Invoke();
    }
}
