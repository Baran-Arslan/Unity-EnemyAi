using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolingState : IState
{

    private readonly NavMeshAgent _agent;
    private readonly Transform _patrolPlaceCenter;

    private readonly float _patrolPlaceRadius;
    private readonly float _minIdleTime, _maxIdleTime;

    private readonly float _maxEnemySpeed;
    private readonly float _minEnemySpeed;


    private float _idleTime = 0;
    private float _timer;


    public EnemyPatrolingState(NavMeshAgent agent, float midIdleTimer, float maxIdleTimer, float minEnemySpeed, float maxEnemySpeed)
    {
        _patrolPlaceRadius = EnemyManager.Instance.PatrolPlaceRadius;
        _patrolPlaceCenter = EnemyManager.Instance.PatrolPlaceCenter;

        _agent = agent;
        _minIdleTime = midIdleTimer;
        _maxIdleTime = maxIdleTimer;
        _minEnemySpeed = minEnemySpeed; 
        _maxEnemySpeed = maxEnemySpeed;
    }

    public void OnEnter()
    {
        _idleTime = 0;
    }
    public void OnExit()
    {
        _agent.SetDestination(_agent.transform.position);
    }

    public void Tick()
    {
        MoveRandomly();
    }

    private void MoveRandomly()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance) 
        {
            _timer += Time.deltaTime;

            Vector3 newPosition;
            //Set destination to new random position after waiting _waitTime seconds
            if (_idleTime <= _timer && FindRandomPoint(_patrolPlaceCenter.position, _patrolPlaceRadius, out newPosition))
            {
                _idleTime = Random.Range(_minIdleTime, _maxIdleTime);
                _agent.speed = Random.Range(_minEnemySpeed, _maxEnemySpeed);
                _timer = 0;

                Debug.DrawRay(newPosition, Vector3.up, Color.green, 3.0f);
                _agent.SetDestination(newPosition);
            }
        }
    }
    // This hits raycast to find new random move point.
    private bool FindRandomPoint(Vector3 center, float range, out Vector3 result)
    {
         Vector3 randomPoint = center + Random.insideUnitSphere * range;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit _hit, 1.0f, NavMesh.AllAreas))
        {
            result = _hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
