using UnityEngine;

namespace _Common.Ai {
    [CreateAssetMenu(menuName = "iCare/Ai")]
    public class AiSO : ScriptableObject {
        public AttackSO AttackStrategy;
        public float AttackRange = 2;
        public float AttackCooldown = 2;
        
        
        [SerializeField] private float startWalkingRange = 2;
        public float StartWalkingRange => startWalkingRange + AttackRange;
    }
}