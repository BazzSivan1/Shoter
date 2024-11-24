using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private FPMovementController movementController;
    private LookController lookController;
    private ShootController shootController;


    private void Start()
    {
        movementController = GetComponent<FPMovementController>();
        lookController = GetComponentInChildren<LookController>();
        shootController = GetComponentInChildren<ShootController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            movementController?.Jump();
        }

        if (Input.GetButton("Fire1"))
        {
            shootController?.Shoot();
        }

        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");
        movementController?.SetMoveDirection(new Vector3(xMovement, 0, zMovement));

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        lookController?.SetLookVector(new Vector2(mouseX, mouseY));
    }
}