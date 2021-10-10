using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Animation_Events : MonoBehaviour
{
    public Marker_Manager MarkerManager;

    public void ClearTargets()
	{
		MarkerManager.ClearTargets ();
	}

	public void DisableMarkers ()
	{
		MarkerManager.DisableMarkers ();
	}
	public void  EnableMarkers()
	{
		MarkerManager.EnableMarkers ();
	}
}
