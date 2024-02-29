using System.Collections.Generic;
using _Common.Core;
using _Common.Core.Extensions;
using UnityEngine;

namespace _Common.Ai.Target {
    public class TargetManager : UnitySingeleton<TargetManager> {
        private readonly HashSet<ITarget> _playerTargets = new HashSet<ITarget>();
        private readonly HashSet<ITarget> _enemyTargets = new HashSet<ITarget>();

        public void Register(ITarget target) {
            switch (target.Faction) {
                case TargetFaction.EnemyTeam:
                    _playerTargets.Add(target);
                    break;
                case TargetFaction.PlayerTeam:
                    _enemyTargets.Add(target);
                    break;
            }
        }

        public void Deregister(ITarget target) {
            switch (target.Faction) {
                case TargetFaction.EnemyTeam:
                    _playerTargets.Remove(target);
                    break;
                case TargetFaction.PlayerTeam:
                    _enemyTargets.Remove(target);
                    break;
            }
        }

        public ITarget GetClosestTarget(Vector3 position, TargetFaction myFaction) {
            var targets = myFaction == TargetFaction.EnemyTeam ? _enemyTargets : _playerTargets;
            return targets.IsNullOrEmpty() ? null : FindClosestTarget(position, targets);
        }

        private ITarget FindClosestTarget(Vector3 referencePosition, HashSet<ITarget> targets) {
            var closestSqrDistance = Mathf.Infinity;
            ITarget closestTarget = null;

            foreach (var target in targets) {
                var sqrDistance = (target.GetTransform().position - referencePosition).sqrMagnitude;

                if (!(sqrDistance < closestSqrDistance)) continue;
                closestSqrDistance = sqrDistance;
                closestTarget = target;
            }

            return closestTarget;
        }
    }
}