using Unity.Netcode;
using UnityEngine;

public class PlayerMovementController : NetworkBehaviour
{
    [SerializeField] private PlayerCollisionController collisionController;
    public Vector3 moveDirection;
    public Vector3 lastInteractionDirection;

    [SerializeField] private float walkingSpeed = 8f;
    [SerializeField] private float rotationSpeed = 15f;

    private void Awake()
    {
        collisionController = GetComponent<PlayerCollisionController>();
    }
    public void HandleAllMovement()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = PlayerInputManager.Instance.GetMovementVectorNormalized();

        moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        HandleRotation(moveDirection);

        float moveDistance = walkingSpeed * Time.deltaTime;

        bool canMove = collisionController.HandleCollision(moveDirection, moveDistance);

        if (moveDirection != Vector3.zero)
        {
            lastInteractionDirection = moveDirection;
        }

        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = (moveDirection.x < -.5f || moveDirection.x > +.5f) && collisionController.HandleCollision(moveDirX, moveDistance);

            if (canMove)
            {
                moveDirection = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = (moveDirection.z < -.5f || moveDirection.z > +.5f) && collisionController.HandleCollision(moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDirection = moveDirZ;
                }
                else
                {

                }
            }
        }
        
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }
    }

    private void HandleRotation(Vector3 moveDirection)
    {
        if(moveDirection != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);   
        }
    }
}