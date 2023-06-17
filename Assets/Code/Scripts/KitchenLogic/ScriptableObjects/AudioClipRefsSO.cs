using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AudioClipRefsSO : ScriptableObject
{
    public AudioClip[] objectPickup;
    public AudioClip[] objectPlace;
    public AudioClip[] footSteps;
    public AudioClip[] cuttingBoardChop;
    public AudioClip panSizzle;
    public AudioClip[] trashBin;
    public AudioClip[] warning;
    public AudioClip[] deliveryFail;
    public AudioClip[] deliverySuccess;
}
