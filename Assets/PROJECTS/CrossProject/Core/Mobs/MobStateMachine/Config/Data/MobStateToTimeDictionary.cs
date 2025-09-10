using System;
using AYellowpaper.SerializedCollections;

namespace CrossProject.Core
{
    [Serializable]
    public class MobStateToTimeDictionary : SerializedDictionary<MobState, float>
    {
    }
}