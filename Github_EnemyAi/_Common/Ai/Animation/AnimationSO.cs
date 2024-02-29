using Sirenix.OdinInspector;
using UnityEngine;

namespace _Common.Ai.Animation {
    [CreateAssetMenu(menuName = "iCare/AiAnimation")]
    public class AnimationSO : ScriptableObject {
        public AnimationClip Idle;
        public AnimationClip Walk;
        public AnimationClip Run;
        [InfoBox("Max 5 attacks", InfoMessageType.Error, "AttacksExceeded")]
        public AnimationClip[] Attacks;
        
        private bool AttacksExceeded() => Attacks.Length > 5;
        public string AttackAnimationName => "Attack0" + Random.Range(1, Attacks.Length + 1);
        public const string IdleAnimationName = "Idle";
    }
}