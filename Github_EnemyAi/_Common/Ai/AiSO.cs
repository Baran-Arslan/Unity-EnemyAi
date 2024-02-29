using _Common.Ai.Animation;
using _Common.Ai.AttackStrategys;
using _Common.Core.Vfx;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Common.Ai {
    [System.Serializable]
    public class MainSettings { 
        [PreviewField(110), AssetsOnly, Required]
        public GameObject CharacterPrefab;

        [Space(30)]

        public int MaxHealth = 1000;
        public float StartWalkingRange = 2;

        public VFX DeathVfx;
        public VFX HitVfx;
        public VFX DestroyVFX;
    }
    [System.Serializable]
    public class AttackSettings {
        [PreviewField(80), GUIColor(1, 0.1f, 0), AssetsOnly]
        public GameObject WeaponPrefab;
        [EnumToggleButtons] public WeaponHand WeaponHand;

        [Space(30)]
        [InlineEditor(InlineEditorModes.FullEditor, Expanded = true)]
        public AttackStrategy AttackStrategy;
        [Space(30)]
        public int AttackDamage = 200;
        public int RagdollPushForce = 100;
        [Range(1, 20)] public int AttackRange = 2;
        
        
        [SerializeField] private float minAttackCooldown = 2;
        [SerializeField] private float maxAttackCooldown = 3;
        public float AttackCooldown => Random.Range(minAttackCooldown, maxAttackCooldown);

    }

    [CreateAssetMenu(menuName = "iCare/Ai")]
    public class AiSO : ScriptableObject {
        [TabGroup("Main Settings"), HideLabel]
        public MainSettings MainSettings;
        
        [TabGroup("Attack Settings"), HideLabel]
        public AttackSettings AttackSettings;

        [TabGroup("Animation Settings"), HideLabel, Required, InlineEditor(InlineEditorModes.FullEditor, Expanded = true)]
        public AnimationSO AnimationSo;

        public float StartWalkingRange => MainSettings.StartWalkingRange + AttackSettings.AttackRange;
    }

    public enum WeaponHand {
        Right,
        Left,
        Both
    }

}