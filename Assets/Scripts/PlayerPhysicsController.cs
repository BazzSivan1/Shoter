using UnityEngine;

public class PlayerPhysicsController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    //[SerializeField] private float rotateSpeed = 200f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravityMultiplier = 2;

    private Vector3 moveDirection;
    private Vector3 rotateDirection;

    private Rigidbody rb;
    private bool doJump = false;

    private Camera cam;

    void Start()
    {
        // obtenim el RigidBody del gameObject
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // ens mourem en les dues Axes
        float verticalAxes = Input.GetAxis("Vertical");
        float horizontalAxes = Input.GetAxis("Horizontal");

        transform.rotation = Quaternion.Euler(0, cam.gameObject.transform.eulerAngles.y, 0);

        // El Vector de direcció serà en el forward segons les Vertical Axes i el Right segons horizontal Axes
        moveDirection = transform.forward * verticalAxes + transform.right * horizontalAxes;

        // impedim que la magnitud del vector sigui més gran que 1
        //moveDirection = Vector3.ClampMagnitude(Quaternion.Euler(0, cam.gameObject.transform.eulerAngles.y, 0) * moveDirection, 1);
        moveDirection = Vector3.ClampMagnitude( moveDirection, 1);

        //rotateDirection = new Vector3(0f, horizontalAxes, 0);

        //transform.Rotate(rotateDirection * rotateSpeed * Time.deltaTime);

        // si pulsem un botó de botar i el transform està a terra activem el flag de botar
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            doJump = true;
        }

       
    }

    private void FixedUpdate()
    {
        // Movem canviant la velocitat del rigidBody enlloc de aplicar una força per a
        // tenir un moviment més Arcade però respectant físiques i colisions.
        // Notar que la velocitat vertical ha de ser la que té en aquest frame el RigidBody, ja que no la volem 
        // modificar sota cap concepte en aquest punt
        Vector3 horizontalVelocity = moveDirection * moveSpeed;

        //rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);

        // Simplement afegim una força adicional en la direcció del sistema de físiques
        // de Unity multiplicat per cert nombre
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);

        // Si el flag de botar està activat saltem amb una força del tipus VelocityChange
        // Després de botar tornem a posar el flag de botar a false per no botar a cada frame
        if (doJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            doJump = false;
        }
    }

    // mètode que ens retorna si el gameObject està tocant a terra. Utilitza la tècnica del 
    // RayCast per projectar un raig que pot impactar sobre qualsevol cosa.
    private bool IsGrounded()
    {
        // mètoda per representar el raycast en l'escena
        Debug.DrawRay(transform.position + Vector3.up * 0.2f, Vector3.down * 0.3f, Color.red);

        // El RayCast es projecta des d'una mica més amunt que la posició del nostre transform
        // i es projecte cap a baix la distància de 0.3
        return Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, 0.3f);

    }

}
