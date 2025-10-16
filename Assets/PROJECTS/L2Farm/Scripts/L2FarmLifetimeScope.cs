using CrossProject.Core;
using CrossProject.Core.Actions;
using CrossProject.Core.Actions.Implementations;
using CrossProject.Core.Audio;
using CrossProject.Core.Camera;
using CrossProject.Core.Characters;
using CrossProject.Core.Cheats;
using CrossProject.Core.Conditions;
using CrossProject.Core.Conditions.ConditionsImplementations;
using CrossProject.Core.InGameResources;
using CrossProject.Core.PROJECTS.CrossProject.Core;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;
using CrossProject.Core.SimpleMovement;
using CrossProject.Core.Skins;
using CrossProject.Core.SpawnPoints;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using CrossProject.Ui.Implementations.InteractButton;
using CrossProject.Ui.Implementations.SettingsPopup;
using L2Farm.Features;
using L2Farm.Features.Buildings;
using L2Farm.Features.Buildings.Actions;
using L2Farm.Features.ClaimerResourcesHint;
using L2Farm.Features.DayNight;
using L2Farm.Features.InventoryScreen;
using L2Farm.Features.NPC;
using L2Farm.Features.QuestIndication;
using L2Farm.Features.ResourceProduction;
using L2Farm.Features.ResourceProduction.Actions;
using L2Farm.Features.ResourceProduction.GiveResources;
using L2Farm.Features.ShopScreen;
using L2Farm.Features.SimpleMonolog;
using L2Farm.Scripts.Actions;
using L2Farm.Scripts.Actions.SpendResources;
using L2Farm.Scripts.CharacterHudElement;
using L2Farm.Scripts.Conditions;
using L2Farm.Scripts.Conditions.QuestCompleted;
using L2Farm.Scripts.Conditions.QuestNotCompleted;
using PROJECTS.L2Farm.Scripts.CharacterSkinSelect;
using VContainer;
using VContainer.Unity;

namespace L2Farm.Scripts
{
    public class L2FarmLifetimeScope : LifetimeScope
    {
        private void RegisterCheats(IContainerBuilder builder)
        {
            builder.Register<CameraCheatOptions>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<JoystickCheatOptions>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<SimpleMovementCheatOptions>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<BlockableCheatPanelReaction>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<DayNightCheatOptions>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<PotionsCheatOptions>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }

        private void RegisterConditionsAndActions(IContainerBuilder builder)
        {
            builder.Register<HasEnoughResourcesCondition>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<NotEnoughResourcesCondition>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<QuestCompletedCondition>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<QuestNotCompletedCondition>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<FalseCondition>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<TrueCondition>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<ProductionCompletedCondition>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<GiveResourcesCondition>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();

            builder.Register<ConditionService>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<GiveResourcesAction>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<StartProductionAction>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<SpendResourcesAction>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<StartBuildingAction>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<FinishBuildingAction>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<SpawnNPCAction>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<DespawnNPCAction>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<ShowMonologAction>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<LaunchQuestAction>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<LoseQuestAction>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();

            builder.Register<ActionService>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<EmptyEntryPoint>()
                .AsSelf();

            builder.Register<L2FarmJsonSerializerSettingsProvider>(Lifetime.Singleton)
                .AsImplementedInterfaces();

            builder.Register<SpawnPointService>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterComponentInHierarchy<DayNightService>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<BuildingService>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<NPCService>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterComponentInHierarchy<QuestIndication>()
                .AsSelf();

            builder.Register<ResourcesService>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<QuestService>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterComponentInHierarchy<Injector>()
                .AsSelf();

            builder.Register<AddressablesManager>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<CharactersService>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<SkinService>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<NoInternetScreenController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            //TODO : VM : Remote Config
            //TODO : VM : GameMigrate - prepare here, on gamestate load/create - migrate

            builder.Register<GameStateManager>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            //TODO : VM : InApp
            //TODO : VM : Ads
            //TODO : VM : ContentService

            builder.Register<ScenesService>(Lifetime.Singleton)
                .AsSelf();

            builder.RegisterComponentInHierarchy<CameraService>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterComponentInHierarchy<UiService>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterComponentInHierarchy<AudioManager>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<L2FarmGameLoader>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<SettingsPopupController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<JoystickController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterComponentInHierarchy<SimpleMovementController>()
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<InteractionHandler>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<InteractButtonController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<CharacterSkinSelectScreenController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<CharacterHudElementController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<InventoryScreenController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<ActiveQuestsScreenController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<ShopScreenController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();

            builder.Register<ClaimedResourcesHintController>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();
            
            builder.Register<ProductionService>(Lifetime.Singleton)
                .AsSelf()
                .AsImplementedInterfaces();
            
            builder.Register<LocalTimeService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ResourceConditionService>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<MainCharacterDeathScreenController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ConsumablesHudElementController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ConfirmPopupController>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<MainCharacterFacade>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<MainCharacterGlobalExperienceGainHandler>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<MainCharacterGlobalReviveHandler>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<MainCharacterGlobalPotionConsumeHandler>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ConfirmPopupOpenHandler>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<TimerCreator>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TimersHandler>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TimePotionConsumeHandler>(Lifetime.Singleton).AsImplementedInterfaces();

            RegisterConditionsAndActions(builder);
            RegisterCheats(builder);
        }
    }
}
