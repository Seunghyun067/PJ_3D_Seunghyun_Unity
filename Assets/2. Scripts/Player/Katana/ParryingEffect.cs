using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryingEffect : MonoBehaviour
{
    private float myScale = 0f;

    void Update()
    {
        myScale += Time.unscaledDeltaTime * 20f;
        transform.localScale = new Vector3(myScale, myScale, myScale);

        if (myScale >= 6f)
            Destroy(gameObject);
    }
}
