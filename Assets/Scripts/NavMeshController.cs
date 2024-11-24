using UnityEngine;
using UnityEngine.AI;

//NavMeshAgents són controladors de personatge basats en sistemes de pathfinding 3D
// els podem utilitzar per a crear de forma senzilla IA d'enemics o NPCs o fer el cotrolador 
// d'un personatge en un joc de "point and click"
public class NavMeshController : MonoBehaviour
{
    [SerializeField] private string targetName;
    [SerializeField] private LayerMask groundLayer;

    private GameObject target;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;

    private bool checkeable = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        target = GameObject.Find(targetName);

        if(TryGetComponent(out Rigidbody rigidbody))
        {
            rb = rigidbody;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Move(hit.point);
                }
            }
        } else
        {
            if (rb is null)
                return;
            if (rb.isKinematic && navMeshAgent.enabled)
                Move(target.transform.position);

            if (rb != null && Input.GetKeyDown(KeyCode.Space))
            {
                navMeshAgent.enabled = false;
                rb.isKinematic = false;
                rb.AddExplosionForce(1000f, target.transform.position, 10f, 10f);
                Invoke(nameof(SetCheckeable), .5f);
            }
        }

        RBCheck();
    }

    private void Move(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }

    private void SetCheckeable()
    {
        checkeable = true;
    }

    private void RBCheck()
    {
        if (checkeable && !rb.isKinematic && rb.velocity.magnitude < 0.1 && IsGrounded())
        {
            navMeshAgent.enabled = true;
            rb.isKinematic = true;
            checkeable = false;
        }
    }

    private bool IsGrounded()
    {
        // mètoda per representar el raycast en l'escena
        Debug.DrawRay(transform.position + Vector3.up * 0.2f, Vector3.down * .8f, Color.red);

        // El RayCast es projecta des d'una mica més amunt que la posició del nostre transform
        // i es projecte cap a baix la distància de 0.2
        return Physics.Raycast(transform.position + Vector3.up * 0.2f, Vector3.down, .8f, groundLayer);

    }
}
