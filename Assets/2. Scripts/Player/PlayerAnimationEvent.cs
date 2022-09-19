using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    //public EffectInfo Effects;
    //
    //[System.Serializable]
    //
    //public class EffectInfo
    //{
    //    public GameObject Effect;
    //    public Transform StartPositionRotation;
    //    public float DestroyAfter = 10;
    //    public bool UseLocalPosition = true;
    //}

    private PlayerController player;
    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    public GameObject Effect;
    public void SwordTrailOn()
    {
        player.katana.KatanaTrailActive(true);
        player.katana.AttackColliderActive(true);

        ////var instance = Instantiate(Effects.Effect, Effects.StartPositionRotation.position, Effects.StartPositionRotation.rotation);
        //
        //if (Effects.UseLocalPosition)
        //{
        //    instance.transform.parent = Effects.StartPositionRotation.transform;
        //    instance.transform.localPosition = Vector3.zero;
        //    instance.transform.localRotation = new Quaternion();
        //}
    }
    void SwordTrailOff()
    {
        player.katana.KatanaTrailActive(false);
        player.katana.AttackColliderActive(false);
    }
}
