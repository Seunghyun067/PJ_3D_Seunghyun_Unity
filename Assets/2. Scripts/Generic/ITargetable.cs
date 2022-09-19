using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    bool IsTarget();
    void OnTarget();
    void NonTarget();
}
