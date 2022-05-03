using UnityEngine;

public class Marker : MonoBehaviour
{
    [HideInInspector] public Transform target;

    [HideInInspector] public Vector3 tempPos;

    [HideInInspector] public float dist;

    [HideInInspector] public Vector3 dir;

    [HideInInspector] public RaycastHit hit;

    public LayerMask layers;

    void Start()
    {
        tempPos = transform.position;
        if (GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().enabled = false;
        }
    }

    public Transform HitCheck()
    {
        target = null;
        dir = transform.position - tempPos;
        dist = Vector3.Distance(transform.position, tempPos);

        Debug.DrawRay(tempPos, dir, Color.white, 0.3f);


        if (Physics.Raycast(tempPos, dir, out hit, dist, layers))
        {
            target = hit.collider.transform;
            tempPos = transform.position;
            return target;
        }
        else
        {
            tempPos = transform.position;
            return null;
        }
    }
}