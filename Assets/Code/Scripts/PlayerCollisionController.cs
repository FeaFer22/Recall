using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCollisionController : MonoBehaviour
{
    [SerializeField] private float playerRadius = .55f;
    [SerializeField] private float playerHeight = 2f;

    public bool HandleCollision(Vector3 moveDir, float moveDistance)
    {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
    }
}
