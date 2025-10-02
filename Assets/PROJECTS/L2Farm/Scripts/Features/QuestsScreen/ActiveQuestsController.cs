using System;
using System.Threading;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace L2Farm.Features
{
    public class ActiveQuestsController : IInitializable, IDisposable
    {
        private readonly UiService _uiService;
        private readonly GameStateManager _gameStateManager;
        private readonly QuestService _questService;
        private readonly CancellationTokenSource _disposeCts = new();

        private ActiveQuestsScreen _questsScreen;
        private QuestsButtonModel _buttonModel;

        public bool IsInitialized { get; private set; }

        public ActiveQuestsController(UiService uiService, GameStateManager gameStateManager, QuestService questService)
        {
            _uiService = uiService;
            _gameStateManager = gameStateManager;
            _questService = questService;
        }

        private async UniTask OpenPopup()
        {
            _questsScreen = await _uiService.TryOpen(new ActiveQuestsModel
            {
                GameStatePart = _gameStateManager.State.Get<QuestsLogPart>(),
                QuestService = _questService,
                Close = Close 
            }) as ActiveQuestsScreen;
        }

        public async UniTask Initialize()
        {
            _buttonModel = new QuestsButtonModel();
            
            _buttonModel.Clicked.Listen(OpenPopup, _disposeCts.Token);
            
            await _uiService.TryOpen(_buttonModel);
            
            IsInitialized = true;
        }

        private void Close()
        {
            _questsScreen.ClearItems();
            
            _uiService.Close(_questsScreen);
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
        }
    }
}
