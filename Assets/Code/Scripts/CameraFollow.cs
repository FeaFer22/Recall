using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
	private Vector3 velocity = Vector3.zero;
	[SerializeField] private float smoothTime = 0.75f;
    void Update() 
    {
        Vector3 pos = target.transform.position;
        transform.position = Vector3.Slerp(transform.position, pos, smoothTime);
    }
}
