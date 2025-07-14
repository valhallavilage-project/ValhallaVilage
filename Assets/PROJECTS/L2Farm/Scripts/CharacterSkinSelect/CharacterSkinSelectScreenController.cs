using CrossProject.Ui.Core;
using RUNNER;
using Unity.VisualScripting;

public class CharacterSkinSelectScreenController: IInitializable
{
    private readonly UiService _uiService;
    public CharacterSkinSelectScreenController(UiService uiService)
    {
        _uiService = uiService;
    }

    public void Initialize()
    {
        _uiService.TryOpen(new CharacterSkinSelectScreenModel()).Forget();
    }
}