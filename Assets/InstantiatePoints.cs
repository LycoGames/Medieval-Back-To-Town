using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePoints : MonoBehaviour
{

    [SerializeField] private Transform rightHandTransform = null;
    public Transform GetRightHandTransform() { return rightHandTransform; }
    [SerializeField] private Transform leftHandTransform = null;
    public Transform GetLeftHandTransform() { return leftHandTransform; }
    [SerializeField] private Transform head = null;
    public Transform GetHeadTransform() { return head; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //print("head" + head.transform.position);
    }
}
