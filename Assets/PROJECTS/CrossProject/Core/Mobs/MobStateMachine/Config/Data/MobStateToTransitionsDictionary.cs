using System;
using AYellowpaper.SerializedCollections;

namespace CrossProject.Core
{
    [Serializable]
    public class MobStateToTransitionsDictionary : SerializedDictionary<MobState, MobTransitionsList>
    {
    }
}