using System;
using System.Linq;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace L2Farm.Features.Garden
{
    public interface IFertilizerConsumeHandler
    {
        bool HasActiveGrowingBed();
        bool TryApply(TimeSpan reduceBy);
    }

    public class FertilizerConsumeHandler : IFertilizerConsumeHandler, IInitializable
    {
        private readonly GameStateManager _gameStateManager;

        public bool IsInitialized { get; private set; }

        public FertilizerConsumeHandler(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public UniTask Initialize()
        {
            IsInitialized = true;
            return UniTask.CompletedTask;
        }

        public bool HasActiveGrowingBed()
        {
            return FindFirstGrowing() != null;
        }

        public bool TryApply(TimeSpan reduceBy)
        {
            var bed = FindFirstGrowing();
            if (bed == null)
                return false;

            var reducedTicks = bed.FinishTimeTicks - reduceBy.Ticks;
            var nowTicks = DateTime.UtcNow.Ticks;
            bed.FinishTimeTicks = reducedTicks < nowTicks ? nowTicks : reducedTicks;

            _gameStateManager.Save();
            return true;
        }

        private GardenBedState FindFirstGrowing()
        {
            var part = _gameStateManager.State.Get<GardenStatePart>();
            return part.DetailedStates.Values
                .Where(s => s.State == GardenBedStateType.Growing)
                .OrderBy(s => s.StartTimeTicks)
                .FirstOrDefault();
        }
    }
}
