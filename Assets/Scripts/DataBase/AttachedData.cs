using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AttachedData : MonoBehaviour
{
    [SerializeField] List<componentD> components = new();

    public bool getComponentByName<T>(string name, out T component, bool CaseSensitive = false) where T : Component
    {
        var selected = components.FirstOrDefault(x => (CaseSensitive) ? x.name == name : x.name.ToLower() == name.ToLower());
        component = selected.monoBehaviour as T;
        return (selected != null);
    }
}

[Serializable]
public class componentD
{
    public string name;
    public MonoBehaviour monoBehaviour;
}