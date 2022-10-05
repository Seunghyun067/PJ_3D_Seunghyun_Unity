using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolingEnable
{
    MonoBehaviour GetMonoBehavior();
    GameObject GetGameObject();
    void PoolingEnable();
    void PoolingDisable();
}