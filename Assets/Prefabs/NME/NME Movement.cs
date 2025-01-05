using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public nmeState stateEnum;

    //refs
    private Transform       goal;
    public  Transform[]     pointsOfInterest;
    private TerminalManager tm;
    

    //vars
    public bool isDebugMode;
    public bool isOmniscient;
    public bool isPlayerSpotted;
    public int  agression, maximumAgression;

    private bool cantPatrol;

    //const
    const float updateSpeed = 0.1f;
    const float stalkSpeed = 5f;
    const float patrolSpeed = 10f;


    #region Generic
    private void Awake()
    {

        
    }

    private void Start()
    {
        tm = TerminalManager.FindAnyObjectByType<TerminalManager>();
        cantPatrol = false;

        try
        {
            agent = GetComponent<NavMeshAgent>();
            goal = GameObject.FindGameObjectWithTag("Player").transform;

            foreach (Transform t in pointsOfInterest)                               // check all points of interest to see if they're null or not active.
            {
                if (t == null || !t.gameObject.activeInHierarchy)
                {
                    tm.createNotif(t.name + " is either NULL or not active.");
                    cantPatrol = true;                                              //if ANY of the points are null or not active, don't allow the guy to patrol.
                }
            }

        }
        catch (NullReferenceException e) { tm.createNotif("NME can't find its goal."); Debug.LogWarning(e); }

        swapMoveState(nmeState.Patrolling);
        StartCoroutine(NME_Brain());
    }
    #endregion

    #region State Machine
    private void swapMoveState(nmeState moveState)
    {

        stopStates();
        switch (moveState)
        {
            case nmeState.Idle:
                if (checkState(moveState))
                {
                    stateEnum = moveState;
                    stopStates();
                    StartCoroutine(Idle());
                }
                break;

            case nmeState.Chasing:
                if (checkState(moveState))
                {
                    stateEnum = moveState;
                    stopStates();
                    StartCoroutine(Chasing());
                }
                break;

            case nmeState.Stalking:
                if (checkState(moveState))
                {
                    stateEnum = moveState;
                    stopStates();
                    StartCoroutine(Stalking());
                }
                break;

            case nmeState.Patrolling:
                if (checkState(moveState))
                {
                    stateEnum = moveState;
                    stopStates();
                    StartCoroutine(Patrolling());
                }
                break;

            case nmeState.Random:
                if (checkState(moveState))
                {
                    stateEnum = moveState;
                    stopStates();
                    StartCoroutine(Random());
                }
                break;
        }
    }

    private bool checkState(nmeState moveState)
    {
        if (moveState != stateEnum)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Navigation Methods
    private IEnumerator Chasing()
    {
        WaitForSeconds _wait = new WaitForSeconds(updateSpeed);
        while (enabled)
        {
            agent.SetDestination(goal.position);
            yield return _wait;
        }
    }

    private IEnumerator Patrolling()
    {

        #region catches
        if (cantPatrol)
        {
            swapMoveState(nmeState.Idle);
            yield break;
        }
        #endregion

        System.Random _rand = new System.Random();
        WaitForSeconds _wait = new WaitForSeconds(patrolSpeed);

        while (enabled)
        {
            //vars
            int randIndex = _rand.Next(pointsOfInterest.Length);
            Transform _patrolPoint = pointsOfInterest[randIndex];

            agent.SetDestination(_patrolPoint.position);
            yield return _wait;
        }


    }

    private IEnumerator Idle()
    {
        yield return null;
    }

    private IEnumerator Stalking()
    {
        yield return null;
    }

    private IEnumerator Random()
    {
        yield return null;
    }

    private IEnumerator NME_Brain()
    {
        //vars
        WaitForSeconds _wait = new WaitForSeconds(updateSpeed);
        agression = maximumAgression;
        while (enabled)
        {
            if (lineOfSight() && !isOmniscient)
                 { isPlayerSpotted = true; }

            if (isOmniscient)
            {
                swapMoveState(nmeState.Chasing);
            }

            if (isPlayerSpotted && !isOmniscient)
            {
                swapMoveState(nmeState.Chasing);
                if (lineOfSight() && agression > 0)
                {
                    agression = maximumAgression;
                }
                else if (!lineOfSight() && agression > 0)
                {
                    agression--;
                }
                else
                {
                    isPlayerSpotted = false;
                    agression = maximumAgression;


                    StopAllCoroutines();                    //bizzare bug makes it so I MUST STOP ALL COROUTINES!!!
                    swapMoveState(nmeState.Patrolling);     //This is temporary.
                    StartCoroutine(NME_Brain());
                }
            }

            yield return _wait;
        }
    }

    private void stopStates()
    {
        StopCoroutine(Chasing());
        StopCoroutine(Patrolling());
        StopCoroutine(Idle());
        StopCoroutine(Stalking());
        StopCoroutine(Random());
    }

    private bool lineOfSight()
    {
        //refs
        Transform t = transform;
        Transform g = goal;
        Vector3 dir = -1 * (t.position - g.position);

        //vars
        //float _viewDistance = 10f;

        //rays
        Ray ray = new Ray(t.position,dir);
        RaycastHit hit;

        if (isOmniscient)
        {
            return true;
        }

        Debug.DrawRay(t.position, dir, Color.red,updateSpeed);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    #endregion
}
