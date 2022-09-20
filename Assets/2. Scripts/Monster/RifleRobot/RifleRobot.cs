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
    private EGA_Laser laser;

    public void ShootLaser()
    {
        laser.dstPosition = player.BodyPoint;
        laser.LaserShoot();
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
        laser = GetComponentInChildren<EGA_Laser>();
    }

    

    public void DeadEffect()
    {
        StartCoroutine(DissolveDisable(0.3f));

    }

    private void OnEnable()
    {
        Debug.Log("asd");
        StartCoroutine(DissolveEnable());
    }

    public override void TakeDamage(int damage)
    {
        if (hp <= 0)
            return;

        hp -= damage;
        attackCollider.enabled = false;
        if (hp > 0)
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
        stateMachine.ChangeState(RifleRobotState.IDLE);
    }
}
