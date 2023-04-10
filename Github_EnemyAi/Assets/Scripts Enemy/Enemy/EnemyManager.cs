using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DefaultExecutionOrder(0)]
public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager Instance
    {
        get { return _instance; }
        private set { _instance = value; } 
    }

    public Transform Player;

    [Header("Patrol Place Settings")]
    [Tooltip("Assign the center of the map to calculate random patroling positions")]
    public Transform PatrolPlaceCenter;
    public float PatrolPlaceRadius;

    [Header("Surround Player Settings")]
    [SerializeField] private float SurroundRadius = 0.5f;
    [HideInInspector] public List<NavMeshAgent> chasingEnemys = new List<NavMeshAgent>();

    [Header("Attack Player Settings")]
    [Range(1, 8)] public int MaxEnemyAttackAtOnce = 2;
    [HideInInspector] public int CurrentAttackingEnemyCount;

    [Header("Path Update Delay")]
    [SerializeField] private float PathUpdateDelay = 0.5f;
    private float _currentTime = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(gameObject);
    }


    private void Update()
    {
        _currentTime += Time.deltaTime;

        if(_currentTime >= PathUpdateDelay)
        {
            SurroundThePlayer();
            _currentTime = 0f;  
        }
    }


    public void SurroundThePlayer()
    {
        for (int i = 0; i < chasingEnemys.Count; i++) //Follow player until SurroundRadius
        {
            if ((chasingEnemys[i].transform.position - Player.transform.position).magnitude > SurroundRadius+0.5f)
            {
                chasingEnemys[i].SetDestination(Player.transform.position);
            }
            else //Surround player
            {
                chasingEnemys[i].SetDestination(new Vector3(
                    Player.position.x + SurroundRadius * Mathf.Cos(2 * Mathf.PI * i / chasingEnemys.Count),
                    Player.position.y,   
                    Player.position.z + SurroundRadius * Mathf.Sin(2 * Mathf.PI * i / chasingEnemys.Count)
                    ));
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (PatrolPlaceCenter != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(PatrolPlaceCenter.position, PatrolPlaceRadius);
        }
    }
}
