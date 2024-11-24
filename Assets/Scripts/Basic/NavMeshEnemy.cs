using UnityEngine;
using UnityEngine.AI;

//NavMeshAgents són controladors de personatge basats en sistemes de pathfinding 3D
// els podem utilitzar per a crear de forma senzilla IA d'enemics o NPCs o fer el cotrolador 
// d'un personatge en un joc de "point and click"
public class NavMeshEnemy : MonoBehaviour
{
    [SerializeField]  private GameObject target;
    private NavMeshAgent navMeshAgent;
    

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
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
            Move(target.transform.position);
        }
        
    }

    private void Move(Vector3 position)
    {
        navMeshAgent.SetDestination(position);
    }
}
