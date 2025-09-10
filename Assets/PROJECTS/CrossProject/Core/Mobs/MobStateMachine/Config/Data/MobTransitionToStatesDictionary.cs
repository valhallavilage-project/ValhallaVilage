using System;
using AYellowpaper.SerializedCollections;

namespace CrossProject.Core
{
    [Serializable]
    public class MobTransitionToStatesDictionary : SerializedDictionary<MobTransition, MobStatesList>
    {
    }
}