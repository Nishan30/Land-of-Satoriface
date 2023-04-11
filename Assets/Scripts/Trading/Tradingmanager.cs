using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tradingmanager : SingleTon<Tradingmanager>
{
    [SerializeField] AttachedData _UI_ItemPrefab, _UI_PropertyPrefab;
    [SerializeField] Transform _ItemSpawnContainer, _PropertySpawnContainer;
    [SerializeField] internal GameObject TradingScreen, sellItemPanel, sellPropertyPanel;
    public bool heliOn = false;
    public override void main()
    {
        TradingScreen.SetActive(false);
        RefreshItems();
        RefreshProperties();
        sellItemPanel.SetActive(false);
        sellPropertyPanel.SetActive(false);
        ChatPanelVisibility(false);
    }

    [ContextMenu("addItem")]
    private void addItem()
    {
        addItemUI(new Item());
    }

    [ContextMenu("addProperty")]
    private void addProperty()
    {
        addPropetryUI(new Property() { propertyCode = 2 });
    }

    private void addItemUI(Item item)
    {
        if (item != null)
        {
            var spawned = Instantiate(Instance._UI_ItemPrefab, Instance._ItemSpawnContainer);
            if (spawned.getComponentByName("title", out TextMeshProUGUI title))
            {
                string titleStr = "";
                titleStr = titleStr.appendAlignment("Qt", TextAlignment.left);
                titleStr = titleStr.appendAlignment("Name", TextAlignment.center);
                titleStr = titleStr.appendAlignment("cost", TextAlignment.right, true);
                titleStr = titleStr.appendAlignment(item.count.ToString(), TextAlignment.left);
                titleStr = titleStr.appendAlignment(item.name.ToString(), TextAlignment.center);
                titleStr = titleStr.appendAlignment(item.priceInCash.ToString(), TextAlignment.right, true);
                title.text = titleStr;
            }

            //First Button
            if (spawned.getComponentByName("firstButton", out Button firstButton))
            {
                firstButton.onClick.AddListener(()=> Button_firstButtonLogic(item,spawned.gameObject));
            }
            //First Button Text
            if (spawned.getComponentByName("firstButtonText", out TextMeshProUGUI firstButtonText))
            {
                firstButtonText.text = "Use";
            }


            //Second Button
            if (spawned.getComponentByName("secondButton", out Button secondButton))
            {
                secondButton.gameObject.SetActive(item.itemType != ItemType.food);
                secondButton.onClick.AddListener(() => Button_secondButtonLogic(item, spawned.gameObject));
            }
            //Second Button Text
            if (spawned.getComponentByName("secondButtonText", out TextMeshProUGUI secondButtonText))
            {
                secondButtonText.text = "Drop";
            }
        }
    }
    #region Use Item

    void Button_firstButtonLogic(Item item,GameObject optionUI)
    {
        switch (item.itemType)
        {
            case ItemType.food:
                Destroy(optionUI);
                Hunger.Instance.getEnergy(item.floatValue);
                break;
            case ItemType.things:
                break;
        }
    }

    void Button_secondButtonLogic(Item item,GameObject optionUI)
    {

    }

    #endregion

    private void addPropetryUI(Property property)
    {
        if (property != null)
        {
            var spawned = Instantiate(Instance._UI_PropertyPrefab, Instance._PropertySpawnContainer);

            if(property.propertyCode == 19)
            {
                heliOn = true;
            }
            if (spawned.getComponentByName("title", out TextMeshProUGUI title))
            {
                string titleStr = "";
                titleStr = titleStr.appendAlignment("Spot", TextAlignment.left);
                titleStr = titleStr.appendAlignment("cost", TextAlignment.right, true);
                titleStr = titleStr.appendAlignment("Prop " + property.propertyCode.ToString("0000"), TextAlignment.left);
                titleStr = titleStr.appendAlignment(property.priceInCash.ToString(), TextAlignment.right, true);
                title.text = titleStr;
            }
        }
    }

    public void enableTradingScreen(bool t)
    {
        TradingScreen.SetActive(t);
        if (t)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }



    #region ItemBuy
    [Space(30)]
    [Header("Item Buy")]
    [SerializeField] Transform _itemBuyContainer;
    [SerializeField] AttachedData _itemBuyPrefab;

    public void RefreshItems()
    {
        foreach (Transform child in _ItemSpawnContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in StaticGamemanager.gameDataStructure.itemsCollected)
        {
            addItemUI(item);
        }
    }
    public void RefreshProperties()
    {
        foreach (Transform child in _PropertySpawnContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in StaticGamemanager.gameDataStructure.PropertiesCollected)
        {
            addPropetryUI(item);
        }
    }

    public static void removeAllItemFromSale()
    {
        foreach (Transform item in Instance._itemBuyContainer)
        {
            Destroy(item.gameObject);
        }
    }

    public static void listItemsOnSale(List<Item> items, Action onBuy)
    {
        foreach (var item in items)
        {
            if (item.count > 0)
            {
                Instance.addItemToSale(item, onBuy);
            }
        }
    }

    [SerializeField] AudioClipPreset buyAP;
    private void addItemToSale(Item item, Action onBuy)
    {
        var currentItemOnSale = Instantiate(_itemBuyPrefab, _itemBuyContainer);
        if (currentItemOnSale.getComponentByName<Button>("buyButton", out Button buyButton))
        {
            //buy button init
            buyButton.onClick.AddListener(() =>
            {
                buyAP.play();
                buyItem(item, currentItemOnSale.gameObject);
                onBuy?.Invoke();
            });
        }
        if (currentItemOnSale.getComponentByName("buyText", out TextMeshProUGUI buyButtonText))
        {
            //buy text init
            buyButtonText.text += "<size=50%>" + item.priceInCash + "CCC</size>";
        }
        if (currentItemOnSale.getComponentByName("itemText", out TextMeshProUGUI itemText))
        {
            //item text init
            itemText.text = item.name;
        }
    }

    public void buyItem(Item item, GameObject itemObj)
    {
        if (MoneyManager.Buy(true, item.priceInCash))
        {
            StaticGamemanager.gameDataStructure.itemsCollected = StaticGamemanager.gameDataStructure.itemsCollected.addItemIncludeCount(item);
            StaticGamemanager.SaveGameDataStructure();
            if (item.count <= 0)
                Destroy(itemObj);
            RefreshItems();
        }
    }

    public void SetSellItemPanel(bool t)
    {
        sellItemPanel.SetActive(t);
    }
    #endregion



    #region PropertyBuy
    [Space(30)]
    [Header("Property Buy")]
    [SerializeField] AttachedData _propertyBuyBlock;
    public void SetSellPropertyPanel(bool t)
    {
        sellPropertyPanel.SetActive(t);
    }

    public void addPropertyToSale(Property property, UnityAction onPropertyBuy)
    {
        if (_propertyBuyBlock.getComponentByName("buyButton", out Button buyButton))
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() =>
            {
                buyAP.play();
                onPropertyBuy();
            });
        }
        if (_propertyBuyBlock.getComponentByName("tradeDetails", out TextMeshProUGUI tradeDetailsText))
        {
            tradeDetailsText.text = "Trade Open" + "\n" + "crypnet: Prop " + property.propertyCode.ToString("0000") + " = " + property.priceInCash + " GBIT";
        }
    }
    #endregion



    #region Chat System
    [Space(30)]
    [Header("Chat")]
    [SerializeField] GameObject _chatPanel;
    [SerializeField] TextMeshProUGUI _chatText;
    [SerializeField] String _nameColor, _messageColor;
    public void ChangeChat(string chatStr)
    {
        _chatText.text = chatStr;
    }
    public void AddToChat(string newChat)
    {
        _chatText.text += "\n" + newChat;
    }
    public void ChangeChat(string name, string chatStr)
    {
        _chatText.text = $"<color={_nameColor}>" + name + ": " + $"<color={_messageColor}>" + chatStr + "</color>";
    }
    public void AddToChat(string name, string newChat)
    {
        _chatText.text += "\n" + $"<color={_nameColor}>" + name + ": " + $"<color={_messageColor}>" + newChat + "</color>";
    }
    public void ChatPanelVisibility(bool t)
    {
        _chatPanel.SetActive(t);
    }
    #endregion


    [ContextMenu("ChangeChat")]
    private void changeKKK()
    {
        ChangeChat("Heiifasdkfn");
    }

    [ContextMenu("ChangeChat2")]
    private void changeKKK2()
    {
        ChangeChat("Heiifasdkfn", "hello");
    }

    [ContextMenu("add Chat")]
    private void addChatkkk()
    {
        AddToChat("kya bolti public!");
    }

    [ContextMenu("add Chat2")]
    private void addChatkkk2()
    {
        AddToChat("Prajwal", "kya bolti public!");
    }


    [ContextMenu("Chat True")]
    public void kfalsdkfj()
    {
        ChatPanelVisibility(true);
    }

    [ContextMenu("Chat false")]
    public void kfalsdkfj2()
    {
        ChatPanelVisibility(false);
    }
}
