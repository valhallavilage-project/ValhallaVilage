using System;
using System.Collections.Generic;
using CrossProject.Core;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace L2Farm.Scripts
{
    public class L2FarmGameLoader : AbstractGameLoader, IInitializable
    {
        private readonly UiService _uiService;
        private readonly ScenesService _scenesService;

        public L2FarmGameLoader(
            UiService uiService,
            ScenesService scenesService)
        {
            _uiService = uiService;
            _scenesService = scenesService;
        }

        public override List<UniTask> PrepareGameLoad()
        {
            return new List<UniTask>
            {
                UniTask.Create(async () => await UniTask.Delay(TimeSpan.FromSeconds(1))),
                UniTask.Create(async () => await UniTask.Delay(TimeSpan.FromSeconds(2))),
                UniTask.Create(async () => await UniTask.Delay(TimeSpan.FromSeconds(3))),
                _scenesService.LoadScene("L2Farm_FirstTown")
            };
        }

        public void Initialize()
        {
            _uiService.Load(PrepareGameLoad()).Forget();
        }
    }
}