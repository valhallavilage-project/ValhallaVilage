namespace CrossProject.Core
{
    public class MainCharacterDamageInfoProvider : BaseDamageInfoProvider
    {
        private readonly IMainCharacterClothesSetsService _setsService;
        
        public MainCharacterDamageInfoProvider(IMainCharacterClothesSetsService setsService)
        {
            _setsService = setsService;

        }
        
        public override float Damage => _setsService.GetTotalDamage();
    }
}
