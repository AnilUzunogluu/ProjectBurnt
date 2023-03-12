using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Kitchen Object", menuName = "New Kitchen Object")]
public class KitchenObjectSO : ScriptableObject
{
    [SerializeField] private Transform prefab;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string objectName;


    public Transform Prefab => prefab;
    public Sprite Sprite => sprite;
    public string ObjectName => objectName;
} 
