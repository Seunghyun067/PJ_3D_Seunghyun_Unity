using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamable
{
    public void TakeDamage(int damage, Transform transform = null);
}
