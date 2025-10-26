namespace CrossProject.Core
{
    public class MainCharacterDamageInfoProvider : BaseDamageInfoProvider
    {
        private readonly IMainCharacterArmorSetsService _setsService;
        
        public MainCharacterDamageInfoProvider(IMainCharacterArmorSetsService setsService)
        {
            _setsService = setsService;

        }
        
        public override float Damage => _setsService.GetTotalDamage();
    }
}
