using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    NavMeshAgent nm;
    Target target_access;

    public Transform target;
    public float hostileRange;

    public enum AIState { idle,hostile};

    public AIState aiState = AIState.idle;
    // Start is called before the first frame update
    void Start()
    {
        nm = GetComponent<NavMeshAgent>();
        target_access = GetComponent<Target>();
        StartCoroutine(Think());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Think()
    {
        while (true)
        {
            switch (aiState)
            {
                case AIState.idle:
                    //if zombie and player are close to each other, the zombie will become hostile and chase the player, otherwise it will be idle
                    float dist = Vector3.Distance(target.position, transform.position);
                    if(dist < hostileRange)
                    {
                        aiState = AIState.hostile;
                    }
                    if (target_access.health > 0)
                    {
                        nm.SetDestination(target.position);
                    }
                    
                    break;
                case AIState.hostile:
                    dist = Vector3.Distance(target.position, transform.position);
                    //if zombie and player are too far from each other, the zombie will become idle until the player is close again
                    if (dist > hostileRange)
                    {
                        aiState = AIState.idle;
                    }
                    if(target_access.health > 0)
                    {
                        nm.SetDestination(target.position);
                    }
                    
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(.5f);
        }
    }
}
