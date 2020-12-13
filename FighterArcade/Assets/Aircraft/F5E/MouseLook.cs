using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class MouseLook : NetworkBehaviour
{
   public GameObject CameraRig;
   public Camera camera;
   public float rotspeed;


   public GameObject Camera;
   public GameObject Freelook;


   

   private void Start()
   {
       Freelook.SetActive(false);
       CameraRig.SetActive(false);

   }

   private void Update()
   {
       if (isLocalPlayer)
       {
           CameraRig.SetActive(true);

           if (Input.GetKeyDown(KeyCode.X))
           {
               Cursor.lockState = CursorLockMode.None;
           }
       }

       if (Input.GetKey(KeyCode.C))
       {
           Camera.SetActive(false);
           Freelook.SetActive(true);
           
          
           
           
           
           float h = Input.GetAxis("Mouse X") * rotspeed;
           float v = Input.GetAxis("Mouse Y") * rotspeed;

           CameraRig.transform.Rotate(-v,h,0);
           float z = CameraRig.transform.eulerAngles.z;
           CameraRig.transform.Rotate(0,0,-z);
       }
       else
       {
           CameraRig.transform.rotation = Quaternion.Lerp(CameraRig.transform.rotation, transform.rotation, Time.deltaTime * 5);
           Camera.SetActive(true);
           Freelook.SetActive(false);
       }
       
          if (Input.GetMouseButton(1))
          {
              camera.fieldOfView = 30;
          }
          else
          {
              camera.fieldOfView = 60;
          }
       
          
          
          
   }

}
