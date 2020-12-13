using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Mirror;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class SimpleFlightV2 : NetworkBehaviour
{
   //Aircrat Rigidbod
   public Rigidbody rb;

   //Settings
   public float speed;
   public float TurnRate;
   public float RollRate;
   public float RudderRate;
   public float missilecount;

   //Controls
   public float throttle;
   public float altitude;
   public float curspeed;
   public float Contoldrag = 1;

   //Flight Systems
   private float Pitch;
   private float Roll;
   private bool toggle;
   public GameObject Gear;

   //Cannon
   public GameObject gun;
   private float ammo = 999;
   public GameObject bullet;

   //UI
   public GameObject UIManager;
   public TextMeshProUGUI SPD;
   public TextMeshProUGUI ALT;
   public TextMeshProUGUI GUN;
   public TextMeshProUGUI MSL;

   //Missile 
   public GameObject missile;
   public GameObject missilerail;
   private bool istrackingactive = false;

   //Abilities
   public float timer;
   public float timer2;
   public GameObject SpeedIcon;
   public GameObject IncMnrIcon;

   public bool Ability = false;
   public bool Ability1 = false;

   public void Update()
   {
      //Functions
      ui();

      if (isLocalPlayer)
      {
      UIManager.SetActive(true);

         //Limits Manouverability at higher speeds
         if (curspeed > 10)
         {
            Contoldrag = curspeed / 17;
         }

         //Control axis
         Pitch = Input.GetAxis("Vertical");
         Roll = Input.GetAxis("Horizontal");

         //Rudder
         if (Input.GetKey(KeyCode.Q))
         {
            rb.AddRelativeTorque(Vector3.up * 100 * -RudderRate / Contoldrag);
         }

         if (Input.GetKey(KeyCode.E))
         {
            rb.AddRelativeTorque(Vector3.up * 100 * RudderRate / Contoldrag);
         }

         //Throttle
         if (Input.GetKey(KeyCode.LeftShift) && throttle < 100)
         {
            throttle += 1;
         }

         if (Input.GetKey(KeyCode.LeftControl) && throttle > 0)
         {
            throttle -= 1;
         }

         //Gear
         if (Input.GetKeyDown(KeyCode.G))
         {
            toggle = !toggle;

            if (toggle)
            {
               Gear.SetActive(false);
            }
            else
            {
               Gear.SetActive(true);
            }
         }
         
         //fire cannons
         
         if (Input.GetKey(KeyCode.Space) && ammo >= 1)
         {
            Cmdgu2n();
            ammo -= 2;

            if (ammo <= 0)
            {
               ammo = 0;
            }
         }


         //Ability
         if (Input.GetKeyDown(KeyCode.Alpha1) && timer < 0)
         {
            Ability = true;
         }

         //Ability1
         if (Input.GetKeyDown(KeyCode.Alpha2) && timer2 < 0)
         {
            Ability1 = true;
         }

         //firemissile
         if (Input.GetKeyDown(KeyCode.T) && missilecount > 0)
         {
            Cmdmissilefire();
            if (missilecount > 0)
            {
               missilecount--;
            }
            
            StartCoroutine(tag());
         }
         
         


         //Altitude
         RaycastHit hit;
         if (Physics.Raycast(transform.position, -Vector3.up * 10000, out hit))
         {
            altitude = (int) hit.distance;
         }
      }
   }

   IEnumerator tag()
   {
      this.gameObject.tag = "tgt";
      yield return new WaitForSeconds(9);
      this.gameObject.tag = "Player";
      StopCoroutine(tag());
      Debug.Log("Testing");
   }

   private void FixedUpdate()
   {
      //Functions
      lift();
      abilitie();

      if (this.gameObject == null)
      {
         Application.Quit();
      }
      
      //Applying forces
      curspeed = (int) rb.velocity.magnitude;
      rb.AddRelativeForce(Vector3.forward * throttle * speed * 150 / Contoldrag);
      rb.AddRelativeTorque(Vector3.right * Pitch * 500 * TurnRate / Contoldrag);
      rb.AddRelativeTorque(Vector3.back * Roll * 50 * RollRate / Contoldrag);

   }
   
   void ui()
   {
      if (isLocalPlayer)
      {
         //Displaying UI
         SPD.text = "SPD " + curspeed.ToString();
         ALT.text = "ALT " + altitude.ToString();
         GUN.text = "GUN " + ammo.ToString();
         MSL.text = "MSL " + missilecount.ToString();
      }
   }

   [Command]
   void Cmdmissilefire()
   {
      // Determines if the missile is fired and spawns it in
      if (missile != null)
      {
         Instantiate(missile, missilerail.transform.position, transform.rotation);
         Rpcmissilefire();
      }
   }

   [ClientRpc]
   void Rpcmissilefire()
   {
      Instantiate(missile, missilerail.transform.position, transform.rotation);
   }

   
   


   void lift()
   {
      if (isLocalPlayer)
      {
         // Faster aircraft moves the move lift 
         rb.AddForce(Vector3.up * 30 * curspeed);
      }
   }

   //Destroys Aircraft if it collides with ground
   private void OnCollisionEnter(Collision other)
   {
      // If Aircraft collides with ground it gets destroyed
      if (other.gameObject.tag == "ground")
      {
         Destroy(this.gameObject);
      }
   }

   [Command]
void Cmdgu2n()
   {
      //Fires Cannons
      Instantiate(bullet, gun.transform.position, transform.rotation);
      
   Rpcgu2n();
   }

[ClientRpc]
void Rpcgu2n()
{
   Instantiate(bullet, gun.transform.position, transform.rotation);
}

   void abilitie()
   {
      if (isLocalPlayer)
      {
         //Activates a speed boost
         timer -= 1 * Time.deltaTime;

         if (timer < 0)
         {
            SpeedIcon.SetActive(true);
         }
         else
         {
            SpeedIcon.SetActive(false);
         }

         //AccelerationBoost
         if (Ability && curspeed < 500)
         {
            rb.velocity = rb.velocity * 5;
            timer = 10;
            Ability = false;
         }

         ////////////////////////////////////////////////////////////////

         //Allows for increased manoverability

         timer2 -= 1 * Time.deltaTime;
         if (timer2 < 0)
         {
            IncMnrIcon.SetActive(true);
         }


         if (Ability1)
         {

            IncMnrIcon.SetActive(false);
            rb.velocity = rb.velocity * -10;
            timer2 = 15;
            Ability1 = false;
         }
      }

   }

}