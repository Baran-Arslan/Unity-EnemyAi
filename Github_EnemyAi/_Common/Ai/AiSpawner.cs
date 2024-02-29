using _Common.Ai;
using _Common.Ai.Soap;
using _Common.Ai.Target;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project._Scripts.Spawners {
    public class AiSpawner : MonoBehaviour {
        [SerializeField, Required]
        private ScriptableEventAi onAiDestroyed;

        [SerializeField] private Ai basePrefab;

        [SerializeField] private Transform playerSpawnTransform;
        [SerializeField] private Transform enemySpawnTransform;

        [SerializeField] private AiSO testData;
        private void OnEnable() {
            onAiDestroyed.OnRaised += DespawnAi;
        }
        private void OnDisable() {
            onAiDestroyed.OnRaised -= DespawnAi;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                SpawnAi(testData, TargetFaction.PlayerTeam);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                SpawnAi(testData, TargetFaction.EnemyTeam);
            }
        }


        public void SpawnAi(AiSO data, TargetFaction aiTeam) {
            var spawnTransform = aiTeam == TargetFaction.PlayerTeam ? playerSpawnTransform : enemySpawnTransform;
            var ai = LeanPool.Spawn(basePrefab, spawnTransform);
            ai.SpawnAi(data, aiTeam);
        }

        private static void DespawnAi(Ai obj) {
            LeanPool.Despawn(obj);
        }
    }
}