using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NMEMovement : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private NavMeshAgent agent;

    [Header("Statistics")]
    [SerializeField] private Vector3 position;

    //refs
    private Transform goal;

    //vars
    const float updateSpeed = 0.1f;


    #region Generic
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    private void Start()
    {
        
    }
    #endregion

    #region Navigation Methods
    private IEnumerator FollowGoal()
    {
        WaitForSeconds _wait = new WaitForSeconds(updateSpeed);
        while (enabled)
        {
            agent.SetDestination(goal.position);
            yield return null;
        }
    }
    #endregion
}
