using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RifleRobotState { IDLE, TRACE, ATTACK, HIT, DIE, NONE_STATE }

public partial class RifleRobot : Monster<RifleRobotState, RifleRobot>
{
    public enum AudioTag { CHARGING, DIE, LASER }
    // Start is called before the first frame update
    [SerializeField] private Collider attackCollider;
    [SerializeField] private Transform shootPosition;
    
    private float moveWeight = 0f;
    private GameObject chargingEffect;
    public void ShootLaser()
    {
        var laser = ObjectPooling.Instance.PopObject("BlueLaser");
        laser.transform.position = shootPosition.position;
        laser.GetComponent<EGA_Laser>().LaserShoot(player.BodyPoint, bodyPoint);
    }

    private void Update()
    {
        if (chargingEffect)
            chargingEffect.transform.position = shootPosition.position;
    }


    public Vector3 ShootPosition
    {
        get { return shootPosition.position; }
    }

    public void AttackColliderActive(bool isActive)
    {
        attackCollider.enabled = isActive;
    }

    private void Awake()
    {
        Initialize();

        if (myRenderer.Length == 0)
            myRenderer = GetComponentsInChildren<Renderer>();
        StateInit();
    }

    

    public void DeadEffect()
    {
        StartCoroutine(DissolveDisable(0.3f));

    }
    private bool isFirst = true;
    private void OnEnable()
    {
        if(isFirst)
        {
            isFirst = false;
            return;
        }
        StartCoroutine(DissolveEnable());
        stateMachine.ChangeState(RifleRobotState.IDLE);
        curHp = maxHP;
    }

    public override void TakeDamage(int damage, Transform transform)
    {
        if (curHp <= 0)
            return;

        curHp -= damage;
        if (curHp > 0)
        {
            ChangeState(RifleRobotState.HIT);
            animator.SetTrigger("Hit");
            
        }
        else
        {
            SoundPlay((int)AudioTag.DIE);
            StopAllCoroutines();
            animator.SetTrigger("Dead");
            targetedObject?.SetActive(false);
            isDead = true;
            ChangeState(RifleRobotState.DIE);
            if (chargingEffect)
                ObjectPooling.Instance.PushObject(chargingEffect);
        }
            
    }

    private void StateInit()
    {
        stateMachine = new StateMachine<RifleRobotState, RifleRobot>(this);
        stateMachine.AddState(RifleRobotState.IDLE, new IdleState());
        stateMachine.AddState(RifleRobotState.TRACE, new TraceState());
        stateMachine.AddState(RifleRobotState.HIT, new HitState());
        stateMachine.AddState(RifleRobotState.ATTACK, new AttackState());
        stateMachine.AddState(RifleRobotState.DIE, new DieState());
    }

    public override MonoBehaviour GetMonoBehavior()
    {
        return this;
    }

    public override void PoolingEnable()
    {
        throw new System.NotImplementedException();
    }

    public override void PoolingDisable()
    {
        throw new System.NotImplementedException();
    }
}
