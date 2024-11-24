using UnityEngine;

// SCRIPT VALIDATED. GOOD PRACTICES. APROVED.
public class FPMovementController : MonoBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpHeight = 4f;
    [SerializeField] private float gravityMultiplier = 2f;
    [SerializeField] private int maxJumps = 2;

    private Vector3 moveDirection;
    private Vector3 jumpVelocity;
    private float gravityApplied;
    private int remainingJumps;

    private void Start()
    {
        gravityApplied = Physics.gravity.y * gravityMultiplier;

        remainingJumps = maxJumps;

    }

    //public method to set the movement direction make input system independnet
    public void SetMoveDirection(Vector3 direction)
    {
        moveDirection = direction;
    }

    public void Jump()
    {
        if (cc.isGrounded || (!cc.isGrounded && remainingJumps > 0))
        {
            jumpVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityApplied);
            remainingJumps--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        ApplyGravity();
    }

    void Move() {
        Vector3 movement = transform.forward * moveDirection.z + transform.right * moveDirection.x;

        //clamp the magnitude of this movement vector fixes de probLem with diagonal velocity and avoids 
        // the strange behavior that apears mixing GetAxis and Normalizing vectors.
        movement = Vector3.ClampMagnitude(movement, 1f);

        cc.Move(movement * speed * Time.deltaTime);
    }

    void ApplyGravity()
    {
        cc.Move(jumpVelocity * Time.deltaTime);

        if (cc.isGrounded && jumpVelocity.y < 0f)
        {
            remainingJumps = maxJumps;
            jumpVelocity.y = -1f;
        }
        else
        {
            jumpVelocity.y += gravityApplied * Time.deltaTime;
        }
        
    }
}
