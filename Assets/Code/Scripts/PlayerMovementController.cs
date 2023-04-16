using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerInputManager inputManager;
    Vector3 moveDirection;

    private Rigidbody _rigidbody;

    public float walkingSpeed = 6f;

    public float rotationSpeed = 10f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<PlayerInputManager>();  
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        moveDirection = new Vector3(inputManager.horizontalInput, 0, inputManager.verticalInput);
        //moveDirection = Quaternion.Euler(0,45,0) * moveDirection;
        moveDirection.Normalize();
        moveDirection.y = 0;

        moveDirection = moveDirection * walkingSpeed;

        Vector3 movementVelocity = moveDirection;
        _rigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        if(moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);   
        }
    }
}