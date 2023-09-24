using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SO_Interactable : ScriptableObject
{
    [field: SerializeField] public int index { get; set; }
    [field: SerializeField] public string actionName { get; set; }
    [field: SerializeField] public string actionSound { get; set; }

}

