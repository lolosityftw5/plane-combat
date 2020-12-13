using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class MouseLook : MonoBehaviour
{
   public GameObject CameraRig;
   public Camera camera;
   public float rotspeed;


   public GameObject Camera;
   public GameObject Freelook;


   

   private void Start()
   {
       Cursor.lockState = CursorLockMode.Locked;
       Freelook.SetActive(false);

   }

   private void Update()
   {

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
              camera.fieldOfView = 45;
          }
          else
          {
              camera.fieldOfView = 60;
          }
       
          
          
          
   }

}
