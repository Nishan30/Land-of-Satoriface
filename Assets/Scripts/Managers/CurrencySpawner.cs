using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencySpawner : MonoBehaviour
{
    [SerializeField] Transform m_Start, m_End;
    [SerializeField] GameObject smallCoin, bigCoin;
    [SerializeField] int i_smallCoin, i_bigCoin;
    [SerializeField] float m_HeightOffset;
    private void Awake()
    {
        for (int i = 0; i < i_smallCoin; i++)
        {
            spawnNew(smallCoin);
        }
        for (int i = 0; i < i_bigCoin; i++)
        {
            spawnNew(bigCoin);
        }
    }
    private void spawnNew(GameObject _prefab)
    {
    Recursion:
        Vector3 RandomPos = new Vector3(Random.Range(m_Start.position.x, m_End.position.x), 20, Random.Range(m_Start.position.z, m_End.position.z));
        if (Physics.Linecast(RandomPos, RandomPos - new Vector3(0, 25, 0), out RaycastHit hit))
        {
            Instantiate(_prefab, hit.point + new Vector3(0, Mathf.Lerp(m_HeightOffset, m_HeightOffset * 5, Random.value), 0), Quaternion.identity);
        }
        else
        {
            goto Recursion;
        }
    }
}
