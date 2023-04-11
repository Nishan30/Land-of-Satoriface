using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI m_TMP;
    Transform target;
    Camera cam;
    internal void Init(Transform target, Camera camera, string text)
    {
        cam = camera;
        this.target = target;
        m_TMP.text = text;
    }

    private void Update()
    {
        transform.position = cam.WorldToScreenPoint(target.position);
    }
}
