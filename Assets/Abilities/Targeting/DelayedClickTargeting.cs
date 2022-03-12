using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using InputSystem;

[CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "Abilities/Targeting/DelayedClickTargeting", order = 0)]

public class DelayedClickTargeting : TargetingStrategy
{
    [SerializeField] Texture2D cursorTexture;
    [SerializeField] Vector2 cursorHotspot;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int areaAffectRadius;
    [SerializeField] Transform targetingPrefab;

    Transform targetingPrefabInstance = null;

    public override void StartTargeting(AbilityData data, Action finished)
    {
        Inputs playerController = data.GetUser().GetComponent<Inputs>();
        StateMachine stateMachine = data.GetUser().GetComponent<StateMachine>();
        playerController.StartCoroutine(Targeting(data, playerController, finished, stateMachine));
    }

    private IEnumerator Targeting(AbilityData data, Inputs playerController, Action finished, StateMachine stateMachine)
    {
        stateMachine.CanMove = true;
        stateMachine.enabled = false;

        if (targetingPrefabInstance == null)
        {
            targetingPrefabInstance = Instantiate(targetingPrefab);
        }
        else
        {
            targetingPrefabInstance.gameObject.SetActive(true);
        }

        targetingPrefabInstance.localScale = new Vector3(areaAffectRadius * 2, 1, areaAffectRadius * 2);

        while (!data.IsCancelled())
        {
            //run every frame
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
            RaycastHit raycastHit;
            if (Physics.Raycast(Inputs.GetMouseRay(), out raycastHit, 1000, layerMask))
            {
                targetingPrefabInstance.position = raycastHit.point;
                if (Input.GetMouseButtonDown(0))
                {
                    // Absorb the whole mouse click
                    yield return new WaitWhile(() => Input.GetMouseButton(0));
                    data.SetTargetedPoint(raycastHit.point);
                    data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                    break;
                }
            }
            yield return null;
        }
        stateMachine.enabled = true;
        targetingPrefabInstance.gameObject.SetActive(false);
        finished();
    }

    private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
    {

        RaycastHit[] hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);
        foreach (var hit in hits)
        {
            yield return hit.collider.gameObject;
        }

    }
}

