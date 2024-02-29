using Obvious.Soap;
using UnityEngine;
using UnityEngine.Events;

namespace _Common.Ai.Soap
{
    [AddComponentMenu("Soap/EventListeners/EventListener"+nameof(Ai))]
    public class EventListenerAi : EventListenerGeneric<Ai>
    {
        [SerializeField] private EventResponse[] _eventResponses = null;
        protected override EventResponse<Ai>[] EventResponses => _eventResponses;

        [System.Serializable]
        public class EventResponse : EventResponse<Ai>
        {
            [SerializeField] private ScriptableEventAi _scriptableEvent = null;
            public override ScriptableEvent<Ai> ScriptableEvent => _scriptableEvent;

            [SerializeField] private AiUnityEvent _response = null;
            public override UnityEvent<Ai> Response => _response;
        }

        [System.Serializable]
        public class AiUnityEvent : UnityEvent<Ai>
        {
            
        }
    }
}