using UnityEngine;

public abstract class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    internal void Awake()
    {
        if (Instance == null)
            Instance = this as T;
        else
            Destroy(gameObject);
        main();
    }

    public abstract void main();
}