using System;

namespace CrossProject.Ui.Core
{
    public class HudButtonModel : HudElementModel
    {
        public Action OnClick { get; }

        public HudButtonModel(Action onClick)
        {
            OnClick = onClick;
        }
    }
}
