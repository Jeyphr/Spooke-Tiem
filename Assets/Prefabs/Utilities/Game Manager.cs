using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Pool;
using UnityEngine.XR;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private TerminalManager tm;

    [Header("Object References")]
    [SerializeField] private GameObject[] pots;

    [Header("Key Items")]
    [SerializeField] private GameObject wrongKey;
    [SerializeField] private GameObject rightKey;

    [Header("Pots")]
    [SerializeField] private int keyAmmountPerPot   = 5;
    [SerializeField] private int numberOfRightKeys  = 1;

    //refs
    public GameObject keyHolder;

    //vars


    //const

    #region Generic
    private void Awake()
    {
        checkSingleton();
    }

    private void Start()
    {
        tm = TerminalManager.FindAnyObjectByType<TerminalManager>();

        fillPots();
    }
    #endregion

    #region Game Management
    private void fillPots()
    {
        #region catches
        if (wrongKey == null || rightKey == null)
        {
            tm.createNotif("Keys are unassigned!");
        }
        #endregion

        Random rand             = new Random();
        GameObject _chosenPot   = pots[0];

        int _rightKeysDistributed   = numberOfRightKeys;
        int _keysDistributed        = keyAmmountPerPot;

        foreach (var p in pots)
        {
            #region catches
            if (p == null || !p.activeInHierarchy)      //we don't want to summon anything from a pot that doesn't exist or isn't active.
            {
                tm.createNotif(p.name + " is either null or not active.");
                break;
            }
            #endregion

            for (int i = _keysDistributed; i >= 0; i--)
            {
                if (_rightKeysDistributed > 0)
                {
                    _rightKeysDistributed--;
                    int randIndex = rand.Next(0, pots.Length);
                    _chosenPot = pots[randIndex];
                    summonHead(rightKey, _chosenPot.transform, keyHolder);
                }
                else
                {
                    summonHead(wrongKey, p.transform, keyHolder);
                }
            }
        }
    }

    private void summonHead(GameObject head, Transform potTrans, GameObject holder)
    {
        Random rand = new Random();
        float _randx = rand.Next(-1, 1);                                                //random x position
        float _randz = rand.Next(-1, 1);                                                //random z position
        float _summonDistance = (1f * rand.Next(1,2));                                                     //how far away the head spawns from the pot

        Vector3 up = potTrans.TransformDirection(_randx, _summonDistance, _randz);      //look up in a random direction 
        Ray ray = new Ray(potTrans.position, up);                                       //create a ray that looks up
        Vector3 summonPoint = ray.GetPoint(_summonDistance);                            //create a vector3 from the ray

        Instantiate(head, summonPoint, Quaternion.identity, holder.transform);          //instantiate an object from the summon point
    }
    #endregion

    #region Singleton Stuff
    private void checkSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
}
