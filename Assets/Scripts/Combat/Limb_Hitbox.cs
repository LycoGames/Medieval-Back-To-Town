using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limb_Hitbox : MonoBehaviour
{
  [Tooltip("For this limb to work, all you need to do is to attach any collider to it (set as NOT trigger!). Fill in the below variable and it's good to go! Just remember to adjust the Tag and Layer of this limb to be readable as a Target for your desired weapons!    [This is an Info bool - it does nothing, just holds the Tooltip.]")]
	public bool Info;
	[Tooltip("What is this Limb attached to? Select the desired BS Health script which stores the Health of this Object (for example in one of the limb's parents.")]
	public Health health;
	[Tooltip("Should this Limb's collider be disabled twhen the MainHealth dies?")]
	public bool DisableColliderOnDeath = true;


	void Start()
	{
		if (health == null) {
			Debug.LogError("Excuse me, but it looks like the object called "+transform.name+" is missing it's Main Health parent! Please, assign it before we can continue!");
		}
	}
	void LateUpdate()
	{
		if (health.GetHealthPoints() <= 0) {
			if(DisableColliderOnDeath)
			{
				GetComponent<Collider>().enabled = false;
			}
			enabled = false;
		}
	}
}
