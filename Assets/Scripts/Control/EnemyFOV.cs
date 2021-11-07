using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    [SerializeField] float radius;
    [Range(0, 360)]
    [SerializeField] float angle;
    [SerializeField] float delayUpdateForOptimize = 0.2f;

    public GameObject targetPlayer;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVUpdater());
    }

    private IEnumerator FOVUpdater()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayUpdateForOptimize);
            FOVChecker();
        }
    }

    private void FOVChecker()
    { //https://docs.unity3d.com/ScriptReference/Physics.OverlapSphere.html
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, targetMask); // targetMask'e verdigimiz layer ile carpısanları alıyorum.
        if (hitColliders.Length != 0)
        {
            foreach (Collider hit in hitColliders)
            {
                Transform target = hit.transform; //vuran hitlerin transformu Transform olan target adlı degiskene attım.
                                                  // https://docs.unity3d.com/ScriptReference/Vector3-normalized.html
                Vector3 directionToTarget = (target.position - transform.position).normalized; //targetin transform posi. ile enemyinin mesafesini cıkarıp 0-1 arası normalize ettim.
                                                                                               //https://docs.unity3d.com/ScriptReference/Vector3.Angle.html
                if (Vector3.Angle(transform.forward, directionToTarget) < angle) //ileriye dogru olan açı ile direction target arasındaki açıyı derece cinsinden alıp verdigimiz angle degerinden kücüklük sorgusu.
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position); // enemy ile player arasındaki mesafe
                    //https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))//engel var mı sorgusu raycast obstructionMask'taki layer ile carpısmıyorsa.
                    {
                        canSeePlayer = true;
                    }

                    else canSeePlayer = false;

                }
                else canSeePlayer = false;

            }

        }
        else if (canSeePlayer) canSeePlayer = false; // artık fovda degilsem cıkmıssım fovdan false yap.

    }

    public bool GetCanSeePlayer()
    {
        return canSeePlayer;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 viewAngle01 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + viewAngle01 * radius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngle02 * radius);
    }
    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
