using System;
using CrossProject.Ui.Core;
using UnityEngine;

namespace CrossProject.Ui.Implementations.InteractButton
{
    public class InteractButtonModel : HudElementModel
    {
        public Sprite InteractionIcon { get; }
        public Action Interaction { get; }


        public InteractButtonModel(Sprite sprite, Action interaction)
        {
            InteractionIcon = sprite;
            Interaction = interaction;
        }
    }
}