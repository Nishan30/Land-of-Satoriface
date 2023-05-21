using UnityEngine;
using TMPro;
using Newtonsoft.Json;
using System.Collections.Generic;
using Photon.Pun;

public class GameManager : SingleTon<GameManager>
{
    [SerializeField] TextMeshProUGUI CID_ID_TEXT;
    [SerializeField] TextMeshProUGUI _CoinConnect_Cash;
    [SerializeField] TextMeshProUGUI _GoldenBit;
    [SerializeField] TextMeshProUGUI _gems;
    [SerializeField] public GameObject prefab;

    public override void main()
    {
        //Init
        StaticGamemanager.gameDataStructure.CoinConnect_Cash.Value = 0;
        StaticGamemanager.gameDataStructure.GoldenBit.Value= 0;
        StaticGamemanager.gameDataStructure.Gems.Value = 0;
        StaticGamemanager.gameDataStructure.CID_Number.onValueChange += onChangeCID_ID;
        StaticGamemanager.gameDataStructure.CoinConnect_Cash.onValueChange += OnCoinConnectCashChange;
        StaticGamemanager.gameDataStructure.GoldenBit.onValueChange += onGoldenBitChange;
        StaticGamemanager.gameDataStructure.Gems.onValueChange += onGemsChanged;

       // PhotonNetwork.Instantiate(prefab.name,prefab.transform.position,prefab.transform.rotation);


    }

    void onChangeCID_ID(int id)
    {
        CID_ID_TEXT.text = id.ToString("00000000");
    }

    void OnCoinConnectCashChange(int i)
    {
        _CoinConnect_Cash.text = "Coinconnect Cash " + i.ToString("0");
        Debug.Log("coin");
    }

    void onGoldenBitChange(int i)
    {
        _GoldenBit.text = "Goldenbit " + i.ToString("0");
        Debug.Log("gold");
    }

    void onGemsChanged(int i)
    {
        Debug.Log("gems");
        _gems.text = "Gems " + i.ToString("0");
    }
}
