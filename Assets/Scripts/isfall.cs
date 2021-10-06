using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isfall : MonoBehaviour
{
   
    [SerializeField]Transform spawnpoint; 
    
      private void OnTriggerEnter(Collider other){   
        other.gameObject.GetComponent<CharacterController>().enabled=false;
        other.gameObject.transform.position=spawnpoint.position;
        other.gameObject.GetComponent<CharacterController>().enabled=true;
        }

  

    
}
