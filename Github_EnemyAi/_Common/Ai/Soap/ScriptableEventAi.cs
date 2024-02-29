using Obvious.Soap;
using UnityEngine;

namespace _Common.Ai.Soap
{
    [CreateAssetMenu(fileName = "scriptable_event_" + nameof(Ai), menuName = "Soap/ScriptableEvents/"+ nameof(Ai))]
    public class ScriptableEventAi : ScriptableEvent<Ai>
    {
        
    }
}
