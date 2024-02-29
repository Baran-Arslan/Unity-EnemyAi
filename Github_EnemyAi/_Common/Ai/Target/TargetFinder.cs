using System.Collections;
using _Common.Ai.Soap;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Common.Ai.Target {
    [RequireComponent(typeof(ITarget))]
    public class TargetFinder : MonoBehaviour {
        [SerializeField] private ScriptableEventAi onAiSpawned;
        
        [ShowInInspector, ReadOnly]
        private ITarget _bestTarget;

        private ITarget _myTarget;
        
        [ShowInInspector, ReadOnly]
        private const float SEARCH_TIME = 1.5f;

        private void Awake() {
            _myTarget = GetComponent<ITarget>();
        }

        public ITarget Get() {
            return _bestTarget;
        }

        private void OnEnable() {
            StartCoroutine(SearchForTarget());
            onAiSpawned.OnRaised += OnAiSpawned;
        }

        private void OnAiSpawned(Ai obj) {
            SearchAction();
        }

        private void OnDisable() {
            StopAllCoroutines();
            onAiSpawned.OnRaised -= OnAiSpawned;
        }

        private IEnumerator SearchForTarget() {
            while (true) {
                SearchAction();
                yield return new WaitForSeconds(SEARCH_TIME);
            }
        }
        private void SearchAction() {
            _bestTarget = TargetManager.Instance.GetClosestTarget(transform.position, _myTarget.Faction);
        }
    }

}