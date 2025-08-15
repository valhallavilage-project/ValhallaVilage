using CrossProject.Core.Actions;
using CrossProject.Core.Characters;
using CrossProject.Core.Quests;
using CrossProject.Ui.Core;
using L2Farm.Features.SimpleMonolog;

namespace L2Farm.Scripts.Actions
{
    public class ShowMonologAction : Action<ShowMonologActionConfig>
    {
        private readonly UiService _uiService;
        private readonly CharactersService _charactersService;
        private readonly QuestService _questService;
        private readonly ActionService _actionService;

        private SimpleMonologPopupView _view;

        public ShowMonologAction(
            UiService uiService,
            CharactersService charactersService,
            QuestService questService,
            ActionService actionService)
        {
            _uiService = uiService;
            _charactersService = charactersService;
            _questService = questService;
            _actionService = actionService;
        }

        public override async void Execute()
        {
            var characterConfig = _charactersService.GetConfigFor(config.speaker);
            var model = new SimpleMonologPopupModel
            {
                portrait = characterConfig.portrait,
                personName = characterConfig.personName,
                message = config.message,
                hasEnoughResourcesConditionConfig = config.hasEnoughResourcesConditionConfig,
                close = () => _uiService.Close(_view),
                next = () =>
                {
                    if (config.hasEnoughResourcesConditionConfig != null)
                    {
                        _questService.ProceedAllQuests();
                        _uiService.Close(_view);
                    }
                    else
                    {
                        _actionService.Execute(config.onNextClickActionConfig);
                        _uiService.Close(_view);
                    }
                }
            };
            _view = await _uiService.TryOpen(model) as SimpleMonologPopupView;
        }
    }
}
