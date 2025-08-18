using System;
using System.Linq;
using System.Threading;
using CrossProject.Core.Characters;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace CrossProject.Core.Skins
{
    [DefaultExecutionOrder(-998)]
    public class SkinService : IInitializable
    {
        private readonly IPlayerSkinProvider _playerSkinProvider;
        private readonly GameStateManager _gameStateManager;
        private readonly AddressablesManager _addressablesManager;

        private SkinSetConfig _skinSetConfig;

        public event Action<SkinId> OnSkinObtained;
        public event Action<SkinId> OnSkinSelected;

        public bool IsInitialized { get; private set; }

        public SkinService(
            IPlayerSkinProvider playerSkinProvider,
            GameStateManager gameStateManager,
            AddressablesManager addressablesManager)
        {
            _playerSkinProvider = playerSkinProvider;
            _gameStateManager = gameStateManager;
            _addressablesManager = addressablesManager;
        }

        public async UniTask Initialize()
        {
            _skinSetConfig = await _addressablesManager.LoadAssetAsync<SkinSetConfig>();
            IsInitialized = true;
        }

        public void Obtain(SkinId skinId)
        {
            if (!_gameStateManager.State.TryGet<ObtainedSkinsPart>(out var part))
                part = _gameStateManager.State.Set(new ObtainedSkinsPart());

            if (part.IsObtained(skinId))
                return;

            var config = _skinSetConfig.items.First(x => new SkinId(x.id) == skinId);
            if (!part.obtainedSkins.ContainsKey(config.owner))
                part.obtainedSkins.Add(config.owner, new CharacterSkinState());
            part.obtainedSkins[config.owner].ids.Add(skinId);
            part.obtainedSkins[config.owner].currentSkinId = skinId;
            OnSkinObtained?.Invoke(skinId);
        }

        public async void Select(SkinId skinId)
        {
            var skinRoot = _playerSkinProvider.PlayerSkinRoot;
            var count = skinRoot.childCount;
            if (count > 0)
                for (int i = count - 1; i >= 0; i--)
                    Object.Destroy(skinRoot.GetChild(i).gameObject);

            var skinPrefab = await _addressablesManager.LoadAssetAsync<GameObject>(skinId.ToString());
            Object.Instantiate(skinPrefab, skinRoot);
            _gameStateManager.Save();
            OnSkinSelected?.Invoke(skinId);
        }

        public SkinId GetDefaultSkinFor(CharacterId characterId)
        {
            return new SkinId(_skinSetConfig.GetDefaultSkinFor(characterId).id);
        }
    }
}