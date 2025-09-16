namespace CrossProject.Core
{
    public interface IMobDamageInfoProvider : IDamageInfoProvider
    {
    }

    public class MobDamageInfoProvider : BaseDamageInfoProvider, IMobDamageInfoProvider
    {
        private readonly MobConfig _mobConfig;

        public override float Damage => _mobConfig.AttackDamage;

        public MobDamageInfoProvider(MobConfig mobConfig)
        {
            _mobConfig = mobConfig;
        }
    }
}
