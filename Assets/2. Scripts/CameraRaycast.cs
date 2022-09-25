using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    [SerializeField] Transform playerBody;

    private ObjsticleForCamera curTarget;
    private ObjsticleForCamera prevTarget;

    void Update()
    {

        float Distance = Vector3.Distance(transform.position, playerBody.position);

        Vector3 Direction = (playerBody.position - transform.position).normalized;

        RaycastHit hit;

        if (!Physics.Raycast(transform.position, Direction, out hit, Distance))
            goto RETURN_FALSE;

        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            goto RETURN_FALSE;

        if (!(curTarget = hit.transform.GetComponent<ObjsticleForCamera>()))
            goto RETURN_FALSE;

        if (curTarget != prevTarget)
        {
            Debug.Log("Target Change");
            curTarget.Hit();
            prevTarget?.NonHit();
            prevTarget = curTarget;
            return;
        }
        else
            return;

    RETURN_FALSE:
        prevTarget?.NonHit();
        prevTarget = null;

    }

}

