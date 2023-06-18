using Assets.Code.Scripts.KitchenLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounterVisual : MonoBehaviour
{
    private const string TRASH = "Trashed";

    [SerializeField] private TrashCounterController trashCounter;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        trashCounter.OnTrashed += TrashCounter_OnTrashed;
    }

    private void TrashCounter_OnTrashed(object sender, System.EventArgs e)
    {
        animator.SetTrigger(TRASH);
    }
}
