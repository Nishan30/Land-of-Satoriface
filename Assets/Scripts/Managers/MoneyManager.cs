using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    internal List<Transform> toRotateList = new();
    private void Awake()
    {
        Instance = this;
        StartCoroutine(rotateMoney());
        AddCoinConnectCash(0);
        addGoldenBit(0);
    }

    public static void AddCoinConnectCash(int t)
    {
        StaticGamemanager.gameDataStructure.CoinConnect_Cash.Value += t;
    }
    public static void addGoldenBit(int t)
    {
        StaticGamemanager.gameDataStructure.GoldenBit.Value += t;
    }

    IEnumerator rotateMoney()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            foreach (var item in toRotateList)
            {
                if (item)
                    item.Rotate(new Vector3(0, 1, 0), Space.World);
            }
        }
    }

    public static bool Buy(bool CoinConnect_Cash, int cost)
    {
        if (CoinConnect_Cash)
        {
            if (StaticGamemanager.gameDataStructure.CoinConnect_Cash.Value >= cost)
            {
                StaticGamemanager.gameDataStructure.CoinConnect_Cash.Value -= cost;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (StaticGamemanager.gameDataStructure.GoldenBit.Value >= cost)
            {
                StaticGamemanager.gameDataStructure.GoldenBit.Value -= cost;
                return true;
            }
            else
            {
                return false;
            }
        }

    }

    public static bool canBuy(bool CoinConnect_Cash, int cost)
    {
        if (CoinConnect_Cash)
        {
            return StaticGamemanager.gameDataStructure.CoinConnect_Cash.Value >= cost;
        }
        else
        {
            return StaticGamemanager.gameDataStructure.GoldenBit.Value >= cost;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Tools/Add 10 Cash")]
    static void add10Cash()
    {
        AddCoinConnectCash(10);
    }

    [MenuItem("Tools/Add 10 bit")]
    static void add10Bit()
    {
        addGoldenBit(10);
    }

    [MenuItem("Tools/Add 100 Cash")]
    static void add100Cash()
    {
        AddCoinConnectCash(100);
    }

    [MenuItem("Tools/Add 100 bit")]
    static void add100Bit()
    {
        addGoldenBit(100);
    }
#endif
}
