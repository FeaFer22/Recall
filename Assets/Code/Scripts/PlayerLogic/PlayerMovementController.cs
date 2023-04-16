using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private PlayerInputManager inputManager;
    [SerializeField] private PlayerCollisionController collisionController;
    public Vector3 moveDirection;
    public Vector3 lastInteractionDirection;

    [SerializeField] private float walkingSpeed = 6f;
    [SerializeField] private float rotationSpeed = 10f;

    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();  
        collisionController = GetComponent<PlayerCollisionController>();
    }
    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    private Vector3 HandleMovement()
    {
        moveDirection = new Vector3(inputManager.horizontalInput, 0, inputManager.verticalInput);

        float moveDistance = walkingSpeed * Time.deltaTime;

        bool canMove = collisionController.HandleCollision(moveDirection, moveDistance);

        if (moveDirection != Vector3.zero)
        {
            lastInteractionDirection = moveDirection;
        }

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = collisionController.HandleCollision(moveDirX, moveDistance);

            if (canMove)
            {
                moveDirection = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = collisionController.HandleCollision(moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDirection = moveDirZ;
                }
                else
                {
                    moveDirection = Vector3.zero;
                }
            }
        }
        
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }
        return moveDirection;
    }

    private void HandleRotation()
    {
        if(moveDirection != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);   
        }
    }
}