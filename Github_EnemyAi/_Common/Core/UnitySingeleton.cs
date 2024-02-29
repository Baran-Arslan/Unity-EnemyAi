using UnityEngine;

namespace _Common.Core {
    public class ProtectedSingeleton<T> : MonoBehaviour where T : Component {
        private static T _instance;

        protected static T Instance {
            get {
                if (_instance != null) return _instance;
                var objs = FindObjectsOfType(typeof(T)) as T[];
                if (objs.Length > 0)
                    _instance = objs[0];
                if (objs.Length > 1) {
                    Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                }

                if (_instance != null) return _instance;
                var obj = new GameObject {
                    hideFlags = HideFlags.HideAndDontSave
                };
                _instance = obj.AddComponent<T>();

                return _instance;
            }
        }
    }

    public class UnitySingeleton<T> : MonoBehaviour where T : Component {
        private static T _instance;

        public static T Instance {
            get {
                if (_instance != null) return _instance;
                var objs = FindObjectsOfType(typeof(T)) as T[];
                if (objs.Length > 0)
                    _instance = objs[0];
                if (objs.Length > 1) {
                    Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                }

                if (_instance != null) return _instance;
                var obj = new GameObject {
                    hideFlags = HideFlags.HideAndDontSave
                };
                _instance = obj.AddComponent<T>();

                return _instance;
            }
        }
    }



    public class UnitySingletonPersistent<T> : MonoBehaviour where T : Component {
        public static T Instance { get; private set; }

        public virtual void Awake() {
            if (Instance == null) {
                Instance = this as T;
                DontDestroyOnLoad(this);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}