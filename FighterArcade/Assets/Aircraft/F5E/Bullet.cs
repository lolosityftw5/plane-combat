using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour
{
    public Rigidbody rb;

    public bool spawndelay = false;

    private void Start()
    {
        StartCoroutine(DestroyTimer());
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * 200 * Time.deltaTime);
        
    }


    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(.2f);
        spawndelay = true;
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }

 
    private void OnCollisionEnter(Collision other)
    {
        if (spawndelay && other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
       
    }

    
}