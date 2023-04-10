using UnityEngine;
using UnityEngine.AI;

public class EnemyAttackState : IState
{
    private readonly NavMeshAgent _agent;
    private readonly Transform _player;
    private readonly float _attackRange;

    private readonly int _maxEnemyAttackAtOnce;

    private float _pathUpdateDelay = 0;


    public EnemyAttackState(NavMeshAgent agent, Transform player, float attackRange, int maxEnemyAttackAtOnce)
    {
        _agent = agent;
        _player = player;
        _attackRange = attackRange;
        _maxEnemyAttackAtOnce = maxEnemyAttackAtOnce;
    }
    public void OnEnter()
    {
        if(EnemyManager.Instance.CurrentAttackingEnemyCount < _maxEnemyAttackAtOnce)//Increase the number of enemies in the playermanager script
            EnemyManager.Instance.CurrentAttackingEnemyCount++;
    }

    public void OnExit()
    {
        _agent.SetDestination(_agent.transform.position);

        if (EnemyManager.Instance.MaxEnemyAttackAtOnce > 0)//Reduce the number of enemies in the playermanager script
            EnemyManager.Instance.CurrentAttackingEnemyCount--;
    }

    public void Tick()
    {
        _pathUpdateDelay += Time.deltaTime;

        //Follow player until attack range
        if (_pathUpdateDelay > 0.4f)
        {
            if (!PlayerInAttackRange()) _agent.SetDestination(_player.position);
            else _agent.SetDestination(_agent.transform.position);

            _pathUpdateDelay = 0;

            Vector3 lookDirection = _player.position - _agent.transform.position;
            lookDirection.y = 0;
            _agent.transform.rotation = Quaternion.Slerp(_agent.transform.rotation, Quaternion.LookRotation(lookDirection), 2 * Time.deltaTime);
        }

    }

    //Change state when player is in attack range from EnemyAi script
    public bool PlayerInAttackRange() => (_player.position - _agent.transform.position).magnitude <= _attackRange;

}
