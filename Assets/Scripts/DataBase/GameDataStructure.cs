using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

using UnityEngine;

[Serializable]
public record GameDataStructure
{
    public ChangeableData<int> CID_Number = new();
    public ChangeableData<int> CoinConnect_Cash = new();
    public ChangeableData<int> GoldenBit = new();
    public List<Item> itemsCollected = new();
    public List<Property> PropertiesCollected = new();
    public TimeSpan CLS = new();

    //World
    public Vector3? lastPlayerPos = null;
    public Quaternion? lastPlayerRot = null;
}

[Serializable]
public record Item
{
    [field: SerializeField] public string name;
    [field: SerializeField] public ItemType itemType;
    [field: SerializeField] public int intValue;
    [field: SerializeField] public float floatValue;
    [field: SerializeField] public int priceInCash;
    [field: SerializeField] public int count;

    public override string ToString()
    {
        return name;
    }
}

[Serializable]
public enum ItemType
{
    food,
    things
}

[Serializable]
public record Property
{
    [field: SerializeField] public string name;
    [field: SerializeField] public int propertyCode;
    [field: SerializeField] public int priceInCash;


    public override string ToString()
    {
        return name;
    }
}


[Serializable]
public struct ChangeableData<T>
{
    private T _Value;
    public T Value
    {
        get => _Value;
        set
        {
            _Value = value;
            onValueChange?.Invoke(value);
        }
    }
    [JsonIgnore]
    public Action<T> onValueChange;
}