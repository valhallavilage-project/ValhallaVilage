using UnityEngine;

namespace CrossProject.Core
{
    [CreateAssetMenu(fileName = "MobTransitionsConfig", menuName = "ScriptableObjects/Mob/MobTransitionsConfig", order = 2)]
    public class MobTransitionsConfig : BaseTransitionsConfig<MobState, MobTransition, MobStatesList, MobTransitionsList, MobStateToTransitionsDictionary, MobTransitionToStatesDictionary>
    {
    }
}