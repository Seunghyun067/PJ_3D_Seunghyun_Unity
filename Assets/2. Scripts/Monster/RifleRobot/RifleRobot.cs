using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RifleRobotState { IDLE, TRACE, ATTACK, HIT, DIE, NONE_STATE }

public partial class RifleRobot : Monster<RifleRobotState, RifleRobot>
{
    // Start is called before the first frame update
    [SerializeField] private Collider attackCollider;
    [SerializeField] private GameObject laserEffect;
    [SerializeField] private Transform shootPosition;
    private float moveWeight = 0f;

    public void ShootLaser()
    {
        var laser = ObjectPooling.Instance.PopObject("BlueLaser");
        laser.transform.position = shootPosition.position;
        laser.GetComponent<EGA_Laser>().LaserShoot(player.BodyPoint, bodyPoint);

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

    private void OnEnable()
    {
        StartCoroutine(DissolveEnable());
        stateMachine.ChangeState(RifleRobotState.IDLE);
        curHp = maxHP;
    }

    public override void TakeDamage(int damage)
    {
        if (maxHP <= 0)
            return;

        maxHP -= damage;
        attackCollider.enabled = false;
        if (maxHP > 0)
        {
            ChangeState(RifleRobotState.HIT);
            animator.SetTrigger("Hit");
        }
        else
        {
            StopAllCoroutines();
            animator.SetTrigger("Dead");
            targetedObject?.SetActive(false);
            isDead = true;
            ChangeState(RifleRobotState.DIE);
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
}
