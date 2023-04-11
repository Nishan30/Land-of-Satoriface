using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "S_Collections", menuName = "Scriptable Object/S_Collections", order = 0)]
public class S_Collections : ScriptableObject
{
    public List<Item> ItemsCanCollect = new();
    public List<Property> PropertiesCanCollect = new();
}
