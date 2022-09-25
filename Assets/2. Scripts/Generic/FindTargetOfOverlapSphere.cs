using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTargetOfOverlapSphere : MonoBehaviour
{
    [Header("Find Information")]
    [SerializeField] private Transform centerPosition;
    [SerializeField] private float findTargetDst = 5;
    [SerializeField] private LayerMask findTargetLayerMask;
    [Space]
    [Header("Gizmo Information")]
    [SerializeField] private Color gizmoColor = Color.red;

    public void Init()
    {
        if (!centerPosition) centerPosition = transform;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;

        if (centerPosition)
            Gizmos.DrawWireSphere(centerPosition.position, findTargetDst);
        else
            Gizmos.DrawWireSphere(transform.position, findTargetDst);

    }

    public Collider[] FindTarget()
    {
        return Physics.OverlapSphere(centerPosition.position, findTargetDst, findTargetLayerMask);
    }

    public bool FindTarget(ref Collider[] colliders)
    {
        colliders = Physics.OverlapSphere(centerPosition.position, findTargetDst, findTargetLayerMask);
        return colliders.Length != 0;
    }
}
