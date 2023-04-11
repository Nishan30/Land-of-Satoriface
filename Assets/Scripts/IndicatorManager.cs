using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    [SerializeField] Indicator m_indicator;
    public static IndicatorManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public static GameObject AddIndicator(Transform target, string text)
    {
        Indicator _indicator;
        Camera camera = Camera.main;
        _indicator = Instantiate(Instance.m_indicator, camera.WorldToScreenPoint(target.position), Quaternion.identity);
        _indicator.transform.SetParent(Instance.transform);
        _indicator.Init(target, camera, text);
        return _indicator.gameObject;
    }
}
