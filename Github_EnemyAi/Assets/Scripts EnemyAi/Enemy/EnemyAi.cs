using System;
using UnityEngine;
using UnityEngine.AI;
 
[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
[DefaultExecutionOrder(1)]
public class EnemyAi : MonoBehaviour
{
    [Header("Attack")]
    public float AttackRange = 1;
    public float TimeBetweenAttacks;
    private int _maxEnemyAttackAtOnce;
    [HideInInspector] public bool HitFinished = true;

    [Header("Chase")]
    public float ChaseRange = 20;

    [Header("Speed")]
    public float MaxEnemySpeed;
    public float MinEnemySpeed;

    [Header("Idle Wait Times")]
    [Tooltip("Determines how long the enemy will wait after reaching each random point")]
    public float MinIdleTimer;
    [Tooltip("Determines how long the enemy will wait after reaching each random point")]
    public float MaxIdleTimer;


    //Referances
    private StateMachine _stateMachine;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Transform _player;

    //Animation IDs
    private int _parametreSpeed = Animator.StringToHash("Speed");


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _player = EnemyManager.Instance.Player;
        _maxEnemyAttackAtOnce = EnemyManager.Instance.MaxEnemyAttackAtOnce;

        _stateMachine = new StateMachine();

        //States
        var patrolingState = new EnemyPatrolingState(_agent, MinIdleTimer, MaxIdleTimer, MaxEnemySpeed, MinEnemySpeed);
        var chaseState = new EnemyChaseState(_agent, _player);
        var attackState = new EnemyAttackState(_agent, _player, AttackRange, _maxEnemyAttackAtOnce);
        var hitState = new EnemyHitState(_agent, _player, _animator, this, _maxEnemyAttackAtOnce);
        var delayState = new EnemyDelayState(TimeBetweenAttacks, _maxEnemyAttackAtOnce);


        //Transations
        At(patrolingState, chaseState, () => PlayerInSightRange());
        At(chaseState, patrolingState, () => !PlayerInSightRange());
        At(chaseState, attackState, () => CanAttackPlayer());
        At(attackState, patrolingState, () => !PlayerInSightRange());
        At(attackState, hitState, () => attackState.PlayerInAttackRange());
        At(hitState, delayState, () => hitState.HasHitFinished());
        At(delayState, attackState, () => HitFinished && delayState.IsDone()); 


        //Start State
        _stateMachine.SetState(patrolingState);


        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
        //void Any(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);

    }

    private void Update() 
    { 
        _stateMachine.Tick();

        UpdateAnimations();
    }
    private void UpdateAnimations()
    {
        float animSpeed = Mathf.InverseLerp(MinEnemySpeed, MaxEnemySpeed, _agent.velocity.magnitude);
        _animator.SetFloat(_parametreSpeed, animSpeed, 0.1f, Time.deltaTime * 10);
    }


    private bool PlayerInSightRange() => (_agent.transform.position - _player.transform.position).magnitude < ChaseRange;
    private bool CanAttackPlayer() => EnemyManager.Instance.CurrentAttackingEnemyCount < _maxEnemyAttackAtOnce &&
        _agent.remainingDistance < 0.4f &&
        _agent.velocity.magnitude < 0.3f;
    public void AnimationEvent_ResetHit() => HitFinished = true;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ChaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }


}
