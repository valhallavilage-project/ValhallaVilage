using System;
using System.Collections.Generic;
using CrossProject.Core;
using CrossProject.Core.Cheats;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace L2Farm.Scripts
{
    public class L2FarmGameLoader : AbstractGameLoader, IInitializable
    {
        private readonly UiService _uiService;
        private readonly ScenesService _scenesService;
        private readonly PROJECTS.L2Farm.Scripts.CharacterSkinSelect.CharacterSkinSelectScreenController _characterSkinSelectScreenController;
        private readonly IObjectResolver _resolver;

        public bool IsInitialized { get; private set; }

        public L2FarmGameLoader(
            UiService uiService,
            ScenesService scenesService,
            PROJECTS.L2Farm.Scripts.CharacterSkinSelect.CharacterSkinSelectScreenController characterSkinSelectScreenController,
            IObjectResolver resolver)
        {
            _uiService = uiService;
            _scenesService = scenesService;
            _characterSkinSelectScreenController = characterSkinSelectScreenController;
            _resolver = resolver;
        }

        public override List<UniTask> PrepareGameLoad()
        {
            return new List<UniTask>
            {
                UniTask.Create(async () => await UniTask.Delay(TimeSpan.FromSeconds(1))),
                _scenesService.LoadScene("L2Farm_FirstTown"),
                //TODO : VM : _scenesService.UnloadScene("L2Farm_Preloader")
            };
        }

        public async UniTask Initialize()
        {
            Debug.Log("[SkipFlow] L2FarmGameLoader.Initialize: start");
            #if !DISABLE_SRDEBUGGER
            _resolver
                .Resolve<IEnumerable<ICheatOptions>>()
                .ForEach(x => SRDebug.Instance.AddOptionContainer(x));
            #endif

            Debug.Log("[SkipFlow] L2FarmGameLoader: awaiting _uiService.LogIn()");
            bool success = await _uiService.LogIn();
            Debug.Log($"[SkipFlow] L2FarmGameLoader: LogIn returned {success}");

            Debug.Log("[SkipFlow] L2FarmGameLoader: waiting for CharacterSkinSelectScreenController.IsInitialized");
            await UniTask.WaitUntil(() => _characterSkinSelectScreenController.IsInitialized);
            Debug.Log("[SkipFlow] L2FarmGameLoader: CharacterSkinSelectScreenController initialized, kicking off Load");

            _uiService.Load(PrepareGameLoad()).Forget();
            IsInitialized = true;
            Debug.Log("[SkipFlow] L2FarmGameLoader.Initialize: done");
        }
    }
}