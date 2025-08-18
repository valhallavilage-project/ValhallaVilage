using System;
using System.Collections.Generic;
using CrossProject.Core;
using CrossProject.Core.Cheats;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using VContainer;
using VContainer.Unity;

namespace L2Farm.Scripts
{
    public class L2FarmGameLoader : AbstractGameLoader, IInitializable
    {
        private readonly UiService _uiService;
        private readonly ScenesService _scenesService;
        private readonly IObjectResolver _resolver;

        public bool IsInitialized { get; private set; }

        public L2FarmGameLoader(
            UiService uiService,
            ScenesService scenesService,
            IObjectResolver resolver)
        {
            _uiService = uiService;
            _scenesService = scenesService;
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
            #if !DISABLE_SRDEBUGGER
            _resolver
                .Resolve<IEnumerable<ICheatOptions>>()
                .ForEach(x => SRDebug.Instance.AddOptionContainer(x));
            #endif

            _uiService.Load(PrepareGameLoad()).Forget();
            IsInitialized = true;
        }
    }
}