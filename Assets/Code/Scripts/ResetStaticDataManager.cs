using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        PlayerInteractionController.ResetStaticData();
        //.ResetStaticData()
        /*
        public static void ResetStaticData()
        {
            eventName = null;
        }

         */
    }
}
