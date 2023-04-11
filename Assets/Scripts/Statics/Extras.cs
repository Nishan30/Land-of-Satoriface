using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class Extras
{
    public static void delay(this MonoBehaviour mono, float seconds, Action action)
    {
        mono.StartCoroutine(dealyAndExc(seconds, action));
    }

    public static IEnumerator dealyAndExc(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

    async public static void delayActive(float t, GameObject obj)
    {
        await System.Threading.Tasks.Task.Delay(millisecondsDelay: (int)(t * 1000));
        obj.SetActive(true);
    }

    static public Item findWithName(this List<Item> items, string name, bool caseSensitive = false)
    {
        var result = items.FirstOrDefault(x => (caseSensitive) ? x.name.ToLower() == name.ToLower() : x.name == name);
        return result;
    }

    static public Property findWithName(this List<Property> Properties, string name, bool caseSensitive = false)
    {
        var result = Properties.FirstOrDefault(x => (caseSensitive) ? x.name.ToLower() == name.ToLower() : x.name == name);
        return result;
    }

    static public Property findWithCode(this List<Property> Properties, int propertyCode)
    {
        var result = Properties.FirstOrDefault(x => x.propertyCode == propertyCode);
        return result;
    }

    public static string appendAlignment(this string str, string newString, TextAlignment textAlignment, bool newLine = false)
    {
        string _str = $"<align={textAlignment.ToString()}>{newString}{NewLine(newLine)}" + "\n";
        return str + _str;
    }
    public static string NewLine(bool t)
    {
        return (t) ? "<line-height=1em>" : "<line-height=0>";
    }
    public static List<Item> addItemIncludeCount(this List<Item> items, Item item)
    {
        var searchResult = items.findWithName(item.name);
        if (searchResult != null)
        {
            searchResult.count += 1;
            item.count -= 1;
            return items;
        }
        else
        {
            item.count -= 1;
            items.Add(item with { count = 1 });
            return items;
        }
    }







#if UNITY_EDITOR
    [MenuItem("Tools/Delete All Player Preffs")]
    public static void DeletePlayerPreff()
    {
        PlayerPrefs.DeleteAll();
    }
#endif

}

public enum TextAlignment
{
    left, center, right
}