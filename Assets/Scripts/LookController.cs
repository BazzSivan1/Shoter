using UnityEngine;

public class LookController : MonoBehaviour
{
    [SerializeField] private float lookXSensitivity = 5;
    [SerializeField] private float lookYSensitivity = 5;

    [SerializeField] private Transform player;
    
    private Vector2 lookVector;
    private float xRotation;


    public void SetLookVector(Vector2 _lookVector)
    {
        lookVector = _lookVector;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        xRotation = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = lookVector.x * lookXSensitivity;
        float mouseY = lookVector.y * lookYSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        player.Rotate(Vector3.up * mouseX);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
