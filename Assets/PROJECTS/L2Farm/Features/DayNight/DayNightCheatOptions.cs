using System.ComponentModel;
using CrossProject.Core.Cheats;

namespace L2Farm.Features.DayNight
{
    public class DayNightCheatOptions : ICheatOptions
    {
        private readonly DayNightService _dayNightService;

        public DayNightCheatOptions(DayNightService dayNightService)
        {
            _dayNightService = dayNightService;
        }

        [Category("DayNight")]
        [DisplayName("Day")]
        public void Day()
        {
            _dayNightService.Set(1);
        }

        [Category("DayNight")]
        [DisplayName("Morning")]
        public void Morning()
        {
            _dayNightService.Set(0.5f);
        }

        [Category("DayNight")]
        [DisplayName("Midnight")]
        public void Midnight()
        {
            _dayNightService.Set(0);
        }
    }
}
