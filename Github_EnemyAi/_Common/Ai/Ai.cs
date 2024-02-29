using System.Collections;
using System.Collections.Generic;
using _Common.Ai.Soap;
using _Common.Ai.Target;
using _Common.Core.Extensions;
using _Common.Core.Vfx;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace _Common.Ai {
    [RequireComponent(typeof(AiBrain))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Ai : MonoBehaviour {
        [SerializeField, Required] private ScriptableEventAi onAiSpawned;
        [SerializeField, Required] private ScriptableEventAi onAiDead;
        [SerializeField, Required] private ScriptableEventAi onAiDestroyed;
        
        [SerializeField] private SetType setType = SetType.ViaOtherScript;
        
        [ShowIf("setType", SetType.Manuel)]
        public AiSO Data;
        [ShowIf("setType", SetType.Manuel)]
        [SerializeField] private TargetFaction myFaction;

        [ShowInInspector, ReadOnly]
        private const string LEFT_WEAPON_HOLDER_NAME = "LeftWeaponHolder";

        [ShowInInspector, ReadOnly]
        private const string RIGHT_WEAPON_HOLDER_NAME = "RightWeaponHolder";

        [ShowInInspector, ReadOnly]
        private const string HEAD_NAME = "Head";

        [ShowInInspector, ReadOnly]
        private const string BODY_NAME = "Spine_01";


        private readonly List<GameObject> _items = new();
        private GameObject _character;
        private Transform _bodyTransform;
        
        private AiBrain _brain;
        private Animator _animator;
        private Health _health;
        private NavMeshAgent _navMeshAgent;


        private void Awake() {
            _brain = GetComponent<AiBrain>();
            _animator = GetComponent<Animator>();
            _health = GetComponent<Health>();
            
            if (setType == SetType.ViaOtherScript) return;
            SpawnAi(Data ,myFaction);
        }

        public void SpawnAi(AiSO aiData, TargetFaction faction = TargetFaction.EnemyTeam) {
            Data = aiData;
            _brain.AiData = Data;
            SetupCharacter();
            SetupAnimations();
            SetupHealth(faction);
            SetupWeapon();
            TargetManager.Instance.Register(_health);
            onAiSpawned.Raise(this);
        }


        private void OnEnable() {
            _health.onDead.AddListener(OnAiDead);
        }

        private void OnDisable() {
            _health.onDead.RemoveListener(OnAiDead);
        }


        private void SetupCharacter() {
            if (_character != null) ClearCharacter();

            gameObject.SetActive(false); //TODO - FIND ANOTHER SOLUTION FOR BUILDING ANIMATOR 
            _character = LeanPool.Spawn(Data.MainSettings.CharacterPrefab, transform);
            _character.transform.localPosition = Vector3.zero;
            _character.transform.localRotation = Quaternion.identity;
            gameObject.SetActive(true);
            
            _bodyTransform = _character.transform.FindDeepChildren(BODY_NAME);
        }

        private void SetupHealth(TargetFaction faction) {
            _health.DeathVFX = Data.MainSettings.DeathVfx;
            _health.HitVFX = Data.MainSettings.HitVfx;
            _health.SetHealth(Data.MainSettings.MaxHealth, Data.MainSettings.MaxHealth);
            _health.Faction = faction;
            _health.MyBloodSpawnTransform = _bodyTransform;
        }

        private void SetupWeapon() {
            if (!_items.IsNullOrEmpty()) ClearItems();


            var weapon = Data.AttackSettings.WeaponPrefab;
            var leftWeaponHolder = transform.FindDeepChildren(LEFT_WEAPON_HOLDER_NAME);
            var rightWeaponHolder = transform.FindDeepChildren(RIGHT_WEAPON_HOLDER_NAME);

            switch (Data.AttackSettings.WeaponHand) {
                case WeaponHand.Left:
                    _items.Add(LeanPool.Spawn(weapon, leftWeaponHolder));
                    _brain.AttackHand = leftWeaponHolder;
                    break;
                case WeaponHand.Right:
                    _items.Add(LeanPool.Spawn(weapon, rightWeaponHolder));
                    _brain.AttackHand = rightWeaponHolder;
                    break;
                case WeaponHand.Both:
                    _items.Add(LeanPool.Spawn(weapon, leftWeaponHolder));
                    _items.Add(LeanPool.Spawn(weapon, rightWeaponHolder));
                    _brain.AttackHand = transform.FindDeepChildren(HEAD_NAME);
                    break;
            }
        }

        private void SetupAnimations() {
            var animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            _animator.runtimeAnimatorController = animatorOverrideController;
            var clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
            animatorOverrideController.GetOverrides(clipOverrides);

            var animData = Data.AnimationSo;
            clipOverrides["Idle"] = animData.Idle;
            clipOverrides["Walk"] = animData.Walk;
            clipOverrides["Run"] = animData.Run;

            for (var i = 0; i < animData.Attacks.Length; i++) {
                clipOverrides["Attack0" + (i + 1)] = animData.Attacks[i];
            }

            animatorOverrideController.ApplyOverrides(clipOverrides);
        }


        private void OnAiDead(Transform arg0, int i) {
            onAiDead.Raise(this);
            TargetManager.Instance.Deregister(_health);
            StartCoroutine(ReleaseAiCoroutine());
        }

        private IEnumerator ReleaseAiCoroutine() {
            yield return new WaitForSeconds(4);
            Data.MainSettings.DestroyVFX.PlayAndRelease(_bodyTransform.position);
            onAiDestroyed.Raise(this);
            ClearPool();
        }

        private void ClearItems() {
            foreach (var item in _items) LeanPool.Despawn(item);
            _items.Clear();
        }

        private void ClearCharacter() {
            LeanPool.Despawn(_character);
            _character = null;
        }

        private void ClearPool() {
            ClearItems();
            ClearCharacter();
        }
    }
}