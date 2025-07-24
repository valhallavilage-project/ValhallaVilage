using System;
using System.Threading;
using CrossProject.Core.Characters;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace CrossProject.Core.Skins
{
    public class SkinService : IAsyncStartable
    {
        private readonly IPlayerSkinProvider _playerSkinProvider;
        private readonly GameStateManager _gameStateManager;
        private readonly AddressablesManager _addressablesManager;

        private CharacterSetConfig _characterSetConfig;

        public event Action<SkinId> OnSkinObtained;
        public event Action<SkinId> OnSkinSelected;

        public SkinService(
            IPlayerSkinProvider playerSkinProvider,
            GameStateManager gameStateManager,
            AddressablesManager addressablesManager)
        {
            _playerSkinProvider = playerSkinProvider;
            _gameStateManager = gameStateManager;
            _addressablesManager = addressablesManager;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            _characterSetConfig = await _addressablesManager.LoadAssetAsync<CharacterSetConfig>();
        }

        public void Obtain(SkinId skinId)
        {
            if (!_gameStateManager.State.TryGet<ObtainedSkinsPart>(out var part))
                part = _gameStateManager.State.Set(new ObtainedSkinsPart());

            if (part.IsObtained(skinId))
                return;

            var characterId = _characterSetConfig.GetOwnerOf(skinId);
            if (!part.obtainedSkins.ContainsKey(characterId))
                part.obtainedSkins.Add(characterId, new CharacterSkinState());
            part.obtainedSkins[characterId].ids.Add(skinId);
            part.obtainedSkins[characterId].currentSkinId = skinId;
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
    }
}