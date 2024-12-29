using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] GameObject target;
    [SerializeField] GameObject gibs;
    [SerializeField] Material baseMaterial, hurtMaterial;


    [Header("Statistics")]
    [SerializeField] float maxHealth;
    [SerializeField] float health;
    [SerializeField] float invulnerabilityDuration;

    //refs
    private MeshRenderer targetMR;
    private Transform targetTR;

    //vars
    public  bool isDebugMode;
    public  bool gibOnDeath     = true;
    private bool invulnerable   = false;
    private bool isDead         = false;

    #region Generic
    private void Start()
    {
        targetMR = target.GetComponent<MeshRenderer>();
        targetTR = target.GetComponent<Transform>();

        targetMR.material = baseMaterial;
    }
    private void Update()
    {
        if (isPressingDebugKey(KeyCode.L, isDebugMode))
        {
            takeDamage(1);
        }
    }
    #endregion

    #region Health Manipulation
    public void takeDamage(float damageTaken)
    {
        #region catches
        if (target == null)
        {
            return;
        }

        if (damageTaken <= 0)
        {
            return;
        }

        if (damageTaken >= maxHealth)
        {
            Die();
        }

        if (health - damageTaken <= 0)
        {
            health = 0;
            Die();
            return;
        }
        #endregion

        //ok now actually do the damage calculation
        StartCoroutine(Ouch(damageTaken));
    }

    private IEnumerator Ouch(float damageTaken)
    {
        float _tInvulnerabilityDuration = invulnerabilityDuration;
        bool  _flicker = false;

        if (!invulnerable)
        {
            health -= damageTaken;
            invulnerable = true;                    //we need this because we wanna make sure that you can only take damage once

            while (_tInvulnerabilityDuration >= 0)
            {
                _tInvulnerabilityDuration --;

                //code for flickering
                if (_flicker == true)
                {
                    targetMR.material = baseMaterial;
                    _flicker = false;
                }
                else
                {
                    targetMR.material = hurtMaterial;
                    _flicker = true;
                }
                yield return null;                  //wait one frame
            }

            targetMR.material = baseMaterial;
            invulnerable = false;
        }
    }
    #endregion

    #region Death
    public void Die()
    {
        if (!isDead)
        {
            targetMR.material = hurtMaterial;
            Destroy(target);
            Debug.Log("Ack!");
        }

    }
    #endregion

    #region Debug
    public bool isPressingDebugKey(KeyCode key, bool isDebugMode)
    {
        if (Input.GetKeyDown(key) && isDebugMode)
            return true;
        return false;
    }
    #endregion
}
