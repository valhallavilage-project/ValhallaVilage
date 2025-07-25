using CrossProject.Core.Characters;
using VContainer.Unity;

namespace L2Farm.Scripts.CharacterHudElement
{
    public class CharacterHudElementController : IInitializable
    {
        private readonly CharactersService _charactersService;

        public CharacterHudElementController(CharactersService charactersService)
        {
            _charactersService = charactersService;
        }

        public void Initialize()
        {
            _charactersService.OnCharacterSelected += OnCharacterSelected;
        }

        private void OnCharacterSelected(CharacterId characterId)
        {
            
        }
    }
}
