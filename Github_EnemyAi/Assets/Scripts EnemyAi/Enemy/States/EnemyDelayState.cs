using UnityEngine;

public class EnemyDelayState : IState
{
    private readonly int _maxEnemyAttackAtOnce;
    private readonly float _waitTime;

    private float _deadline;

    public EnemyDelayState(float waitTime, int maxEnemyAttackAtOnce)
    {
        _waitTime = waitTime;
        _maxEnemyAttackAtOnce = maxEnemyAttackAtOnce;
    }

    public void OnEnter()
    {
        _deadline = Time.time + _waitTime;

        EnemyManager.Instance.AddAttackingEnemy();
    }
    public void OnExit()
    {

        EnemyManager.Instance.RemoveAttackingEnemy();

    }
    public void Tick()
    {
    }
    public bool IsDone()
    {
        return Time.time > _deadline;
    }
}
