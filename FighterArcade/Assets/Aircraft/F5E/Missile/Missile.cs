using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Missile : NetworkBehaviour
{
    //Missile 
    public GameObject missile;
    private bool isTrackingActive = false;
    public TrailRenderer missileTrail;
    
    public float distToTargets;
    private bool delay = false;


    private void Start()
    {
        StartCoroutine(MissileActivated());
    }

    private void FixedUpdate()
    {
        //Missile Engine Activated and adding force
        RpcFiredMissile();
        missile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 100);
    }

    
    void RpcFiredMissile()
    {
        if (missile != null && isTrackingActive)
        {


            //Loops through all targets and determines if it meets paramaters
            GameObject[] Targets = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < Targets.Length; i++)
            {
                //Calculate Distance from missile to target
                 distToTargets = Vector3.Distance(missile.transform.position, Targets[i].transform.position);
              

                //if inside paramaters track target
                if (distToTargets <= 1000 && delay)
                {
                    missile.transform.LookAt(Targets[i].transform.position);

                    //if close enough detonate
                    if (distToTargets < 7 && delay)
                    {
                        
                        Destroy(missile.gameObject);
                        Destroy(Targets[i]);
                    }


                }
            }
        }
    }

    IEnumerator MissileActivated()
    {
        missile.GetComponent<Rigidbody>().isKinematic = false;
     //   missile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.down * 20);
        yield return new WaitForSeconds(1);
        missileTrail.emitting = true;
        isTrackingActive = true;
        yield return new WaitForSeconds(1.5f);
        delay = true;
        yield return new WaitForSeconds(7);
        Destroy(missile);
        StopCoroutine(MissileActivated());

    }
}
