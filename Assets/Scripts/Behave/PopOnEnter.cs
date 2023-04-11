using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PopOnEnter : MonoBehaviour
{
    [SerializeField] Renderer _renderer;
    private void Start()
    {
        _renderer.material.SetFloat("_hide", 1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered");
            _renderer.material.DOFloat(0, "_hide", 0.3f).SetEase(Ease.OutQuad);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Exit");
            _renderer.material.DOFloat(1, "_hide", 0.3f).SetEase(Ease.OutQuad);
        }
    }
}
