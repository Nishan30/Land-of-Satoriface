using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class propertyShop : propertyShopBase
{
    public Property _property;
    [SerializeField] Chat[] Chats;
    GameObject _indicator;
    [SerializeField] KeyCode _keyCode = KeyCode.B;
    [SerializeField] float _range = 5, _indicatorZOffset = 2;
    bool inRange = false, showedPanel = false;
    private void Start()
    {
        _propertyShops.Add(this);
        GameObject go = new();
        go.transform.SetParent(transform);
        go.transform.position = transform.position + new Vector3(0, _indicatorZOffset, 0);
        _indicator = IndicatorManager.AddIndicator(go.transform, _keyCode.ToString());
        Start2();
    }

    private void Update()
    {
        bool InRange = Vector3.Distance(transform.position, Player.CurrentPlayer.position) <= _range;
        _indicator.SetActive(InRange);
        bool lastInRange = inRange;
        inRange = Vector3.Distance(transform.position, Player.CurrentPlayer.position) < _range;
        if (lastInRange != inRange)
            InRangeCage(inRange);
        _indicator.SetActive(inRange);
        if (inRange && Input.GetKeyDown(_keyCode))
        {
            if (showedPanel)
            {
                Tradingmanager.Instance.enableTradingScreen(false);
                Tradingmanager.Instance.SetSellPropertyPanel(false);
                Tradingmanager.Instance.ChatPanelVisibility(false);
                showedPanel = false;
            }
            else
            {
                Tradingmanager.Instance.enableTradingScreen(true);
                if (StaticGamemanager.gameDataStructure.PropertiesCollected.findWithCode(_property.propertyCode) == null)
                {
                    Tradingmanager.Instance.SetSellPropertyPanel(true);
                    Tradingmanager.Instance.addPropertyToSale(_property, () => Button_onPropertyBuy(_property));
                }
                Tradingmanager.removeAllItemFromSale();
                Tradingmanager.Instance.ChangeChat("");
                foreach (var item in Chats)
                {
                    Tradingmanager.Instance.AddToChat(item.name, item.chat);
                }
                Tradingmanager.Instance.ChatPanelVisibility(true);
                showedPanel = true;
            }
        }
        if (lastInRange == true && inRange == false && showedPanel == true)
        {
            showedPanel = false;
            Tradingmanager.Instance.enableTradingScreen(false);
            Tradingmanager.Instance.SetSellPropertyPanel(false);
            Tradingmanager.Instance.ChatPanelVisibility(false);
        }
    }
    private void Button_onPropertyBuy(Property property)
    {
        Debug.Log(property.priceInCash);
        
        if (MoneyManager.Buy(false, property.priceInCash))
        {
            Debug.Log("buying");
            StaticGamemanager.gameDataStructure.PropertiesCollected.Add(property);
            StaticGamemanager.SaveGameDataStructure();
            Tradingmanager.Instance.RefreshProperties();
            Tradingmanager.Instance.sellPropertyPanel.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector3 constPos = new Vector3(_range, 8, _range);
        Gizmos.DrawWireCube(transform.position + new Vector3(0, constPos.y, 0) / 2, constPos);
        Gizmos.DrawWireCube(transform.position + new Vector3(0, constPos.y * 0.98f, 0) / 2, constPos * 0.98f);
        Gizmos.DrawWireCube(transform.position + new Vector3(0, constPos.y * 0.96f, 0) / 2, constPos * 0.96f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _range);
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, _indicatorZOffset, 0), 0.2f);
    }

    //cageIndicator
    [SerializeField] Renderer _renderer;
    private void Start2()
    {
        _renderer.material.SetFloat("_hide", 1);
    }
    private void InRangeCage(bool inRange)
    {
        _renderer.material.DOFloat((inRange) ? 0.5f : 1, "_hide", 0.5f).SetEase(Ease.OutFlash);
    }
}

public class propertyShopBase : MonoBehaviour
{
    protected List<propertyShop> _propertyShops = new();
    private void Start()
    {
        int index = 0;
        foreach (var item in _propertyShops)
        {
            index++;
            item._property.propertyCode = index;
        }
    }
}

[Serializable]
public class Chat
{
    public string name;
    public string chat;
}