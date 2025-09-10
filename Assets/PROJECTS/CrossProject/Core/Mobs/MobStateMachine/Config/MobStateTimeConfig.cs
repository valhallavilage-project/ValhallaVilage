using System;
using UnityEngine;

namespace CrossProject.Core
{
    [Serializable, CreateAssetMenu(fileName = "MobStateTimeConfig", menuName = "ScriptableObjects/Mob/MobStateTimeConfig", order = 1)]
    public class MobStateTimeConfig : BaseStateTimeConfig<MobState, MobStateToTimeDictionary>
    {
    }
}