using UnityEngine;

namespace CrossProject.Ui.Core
{
    public abstract class HudElementModel : UiModel
    {
        public virtual bool UseSizeDeltaInsteadOfAnchors => false;

        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Vector2 pivot;
        public Vector2 sizeDelta;
    }
}