using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectScriptableObject scriptableObject;

    public KitchenObjectScriptableObject GetKitchenObject()
    {
        return scriptableObject;
    }
}
