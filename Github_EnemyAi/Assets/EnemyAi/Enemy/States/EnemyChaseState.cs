using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : IState
{
    private readonly NavMeshAgent _agent;
    private readonly Transform _player;


    public EnemyChaseState(NavMeshAgent agent,Transform player)
    {
        _agent = agent;
        _player = player;
    }

    public void OnEnter()
    {
        _agent.SetDestination(_agent.transform.position);
        EnemyManager.Instance.chasingEnemys.Add(_agent);//It will automatically surround the player as we add it to the list

    }

    public void OnExit()
    {
        EnemyManager.Instance.chasingEnemys.Remove(_agent);
        _agent.SetDestination(_agent.transform.position);
    }

    public void Tick()
    {
        //if it is within the surround radius rotate to the player
        if (_agent.remainingDistance < 0.6f && _agent.velocity.magnitude < 0.4f) 
        {
            Vector3 lookDirection = _player.position - _agent.transform.position;
            lookDirection.y = 0;
            _agent.transform.rotation = Quaternion.Slerp(_agent.transform.rotation, Quaternion.LookRotation(lookDirection), 2 * Time.deltaTime);
        }
    }
}