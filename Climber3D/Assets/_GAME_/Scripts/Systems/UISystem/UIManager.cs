using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIManager : PersistentSingleton<UIManager>
{
    
    [SerializeField, LabelText("UI Priority List")]
    private List<_UI> uiPriorityList;

    private readonly Dictionary<Type, _UI> uis = new Dictionary<Type, _UI>();

    protected override void Awake()
    {
        base.Awake();
        PopulateDictionary();
    }

    private void PopulateDictionary()
    {
        foreach (_UI ui in uiPriorityList)
        {
            uis.Add(ui.GetType(), ui);
        }
    }

    public static T Get<T>() where T : _UI
    {
        return (T) Instance.uis[typeof(T)];
    }

#if UNITY_EDITOR
    [Button(ButtonSizes.Medium)]
    public void FindControllers()
    {
        foreach (_UI ui in FindObjectsOfType<_UI>())
        {
            if (!uiPriorityList.Exists(x => x.GetType() == ui.GetType()))
            {
                uiPriorityList.Add(ui);
            }
            else
            {
                Debug.LogWarning($"An object with type {ui.GetType()} already exists in list. " +
                    $"There can only be one of each type.");
            }
        }
    }
#endif

}
