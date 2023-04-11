using UnityEngine;
using static Extras;
public class Money : MonoBehaviour
{
    [SerializeField] int m_money;
    [SerializeField] bool rotate;
    [SerializeField] bool goldenBit = false;
    [SerializeField] AudioClipPreset collectAP;
    private void Start()
    {
        if (rotate)
        {
            MoneyManager.Instance.toRotateList.Add(transform);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collectAP.play();
            if (goldenBit)
                MoneyManager.addGoldenBit(m_money);
            else
                MoneyManager.AddCoinConnectCash(m_money);
            MoneyManager.Instance.toRotateList.Remove(transform);
            gameObject.SetActive(false);
            delayActive(3600f, gameObject);
        }
    }
}
