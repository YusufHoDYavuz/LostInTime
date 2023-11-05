using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCollactable : MonoBehaviour
{
   [SerializeField] private CollectObjects collectObjects;

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Collactable"))
      {
         collectObjects.SetAutoCollactable(true);
      }
   }

   private void OnTriggerStay(Collider other)
   {
      if (other.CompareTag("Collactable"))
      {
         if (Input.GetKeyDown(KeyCode.F))
         {
            collectObjects.AutoCollactable();
            Destroy(other.gameObject);
         }
      }
   }
   
   private void OnTriggerExit(Collider other)
   {
      if (other.CompareTag("Collactable"))
      {
            collectObjects.SetAutoCollactable(false);
      }
   }
}
