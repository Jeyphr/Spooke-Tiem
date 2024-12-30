using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NMEMovement : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private NavMeshAgent agent;


    public enum nmeState
    {
        Idle,
        Chasing,
        Stalking,
        Patrolling,
        Random
    }
    public nmeState state;

    //refs
    private Transform goal;
    private TerminalManager tm;

    //vars
    public bool isDebugMode;
    public bool isOmniscient;
    private bool hasSpottedPlayer;
    const float updateSpeed = 0.1f;


    #region Generic
    private void Awake()
    {

        
    }

    private void Start()
    {
        tm = TerminalManager.FindAnyObjectByType<TerminalManager>();

        try
        {
            agent = GetComponent<NavMeshAgent>();
            goal = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch (NullReferenceException e) { tm.createNotif("NME can't find its goal."); Debug.Log(e); }

        StartCoroutine(FollowGoal());
    }
    #endregion

    #region Navigation Methods
    private IEnumerator FollowGoal()
    {
        WaitForSeconds _wait = new WaitForSeconds(updateSpeed);
        while (enabled)
        {
            agent.SetDestination(goal.position);
            yield return _wait;
        }
    }
    #endregion
}
