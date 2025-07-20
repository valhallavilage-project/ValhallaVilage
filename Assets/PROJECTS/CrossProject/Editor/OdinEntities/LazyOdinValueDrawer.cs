using System;
using PROJECTS.CrossProject.Editor;
using Sirenix.OdinInspector.Editor;

namespace CrossProject.Editor.OdinEntities
{
    public abstract class LazyOdinValueDrawer<T> : OdinValueDrawer<T>, IDisposable
    {
        protected override void Initialize()
        {
            AssetsWatcher.OnAssetsChanged += OnAssetsChanged;
            OnAssetsChanged();
        }

        protected abstract void OnAssetsChanged();

        public virtual void Dispose()
        {
            AssetsWatcher.OnAssetsChanged -= OnAssetsChanged;
        }
    }
}