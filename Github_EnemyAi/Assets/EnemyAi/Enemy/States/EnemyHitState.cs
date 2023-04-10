using UnityEngine;
using UnityEngine.AI;

public class EnemyHitState : IState
{
    private readonly Animator _animator;
    private readonly EnemyAi _enemyAi;
    private readonly NavMeshAgent _agent;
    private readonly Transform _player;
    private readonly int _maxEnemyAttackAtOnce;

    private float _timer;

    private int _triggerAttack = Animator.StringToHash("Attack"); //Change this string with your animation name

    public EnemyHitState(NavMeshAgent agent, Transform player, Animator animator, EnemyAi enemyAi, int maxEnemyAttackAtOnce)
    {
        _animator = animator;
        _enemyAi = enemyAi;
        _player = player;
        _agent = agent;
        _maxEnemyAttackAtOnce = maxEnemyAttackAtOnce;
    }

    public void OnEnter()
    {
        _animator.SetTrigger(_triggerAttack);
        _timer = 0;
        _enemyAi.HitFinished = false;

        if (EnemyManager.Instance.CurrentAttackingEnemyCount < _maxEnemyAttackAtOnce)//Increase the number of enemies in the playermanager script
            EnemyManager.Instance.CurrentAttackingEnemyCount++;
    }

    public void OnExit()
    {

        if (EnemyManager.Instance.MaxEnemyAttackAtOnce > 0)//Reduce the number of enemies in the playermanager script
            EnemyManager.Instance.CurrentAttackingEnemyCount--;
    }

    public void Tick()
    {
        //Rotate to player when hitting
        Vector3 lookDirection = _player.position - _agent.transform.position;
        lookDirection.y = 0;
        _agent.transform.rotation = Quaternion.Slerp(_agent.transform.rotation, Quaternion.LookRotation(lookDirection), 2 * Time.deltaTime);
    }

    public bool HasHitFinished()
    {
        _timer += Time.deltaTime;
        if( _timer > 0.5f && _enemyAi.HitFinished) return true;
        else return false;
    }


}
