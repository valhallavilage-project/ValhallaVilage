# ДОКУМЕНТАЦИЯ КВЕСТОВОЙ СИСТЕМЫ ValhallaVilage

## ОБЗОР АРХИТЕКТУРЫ

Квестовая система построена на паттернах **Strategy** и **Service Locator** с использованием VContainer (DI) и UniTask (async).

---

## 1. ОСНОВНЫЕ КОМПОНЕНТЫ

### 1.1 QuestService - Главный сервис
**Путь:** `Assets/PROJECTS/CrossProject/Core/Quests/QuestService.cs`

| Метод | Описание |
|-------|----------|
| `TryLaunch(questId, stepIndex)` | Запуск квеста |
| `CanProceed(questId)` | Проверка условия победы текущего шага |
| `TryProceedStepsOf(questId)` | Переход на следующий шаг |
| `ForceWin(questId)` | Принудительная победа |
| `ForceLose(questId)` | Принудительный проигрыш |

**События:**
- `OnQuestLaunch` - квест запущен
- `OnQuestProceed(QuestId, int)` - шаг выполнен
- `OnQuestWin` - квест завершён успешно
- `OnQuestLose` - квест провален

**ВАЖНО:** Квесты НЕ появляются автоматически из конфига. Чтобы квест появился в игре, нужно:
1. Добавить `LaunchQuestAction` в winActions другого квеста
2. Или вызвать вручную через чит/консоль
3. Или добавить в `QuestsLogPart` при первом запуске

---

### 1.2 Конфигурация квестов

#### QuestConfig
**Путь:** `Assets/PROJECTS/CrossProject/Core/Quests/QuestConfig.cs`

```csharp
public class QuestConfig
{
    public string id;                    // Уникальный ID
    public bool proceedAfterLaunch;      // Авто-переход после запуска
    public SpawnPointId targetSpawnPoint;// Целевая точка на карте
    public IndicationTypeId questIndication; // Тип маркера (иконка на карте)

    public List<IActionConfig> launchActions;  // Действия при запуске
    public List<QuestStepConfig> steps;        // Шаги квеста
    public List<IActionConfig> winActions;     // Действия при победе
    public List<IActionConfig> loseActions;    // Действия при проигрыше
}
```

#### QuestStepConfig
**Путь:** `Assets/PROJECTS/CrossProject/Core/Quests/QuestStepConfig.cs`

```csharp
public class QuestStepConfig
{
    public IActionConfig stepAction;      // Действие на этом шаге

    public IConditionConfig winCondition; // Условие победы
    public List<IActionConfig> winActions;// Действия при победе шага

    public IConditionConfig loseCondition;// Условие проигрыша
    public List<IActionConfig> loseActions;
}
```

#### QuestSetConfig
**Путь:** `Assets/PROJECTS/L2Farm/Configs/QuestSetConfig.asset`

ScriptableObject со списком всех квестов игры.

---

## 2. LaunchQuest - ЗАПУСК КВЕСТОВ

### LaunchQuestAction
**Путь:** `Assets/PROJECTS/CrossProject/Core/Actions/Implementations/LaunchQuestAction/LaunchQuestAction.cs`

```csharp
public class LaunchQuestAction : Action<LaunchQuestActionConfig>
{
    public override async UniTask Execute()
    {
        await _questService.TryLaunch(config.questId);
    }
}
```

### LaunchQuestActionConfig
**Путь:** `Assets/PROJECTS/CrossProject/Core/Actions/Implementations/LaunchQuestAction/LaunchQuestActionConfig.cs`

```csharp
public class LaunchQuestActionConfig : IActionConfig
{
    public QuestId questId;  // ID квеста для запуска
}
```

### Где используется LaunchQuest:

| Место | Описание |
|-------|----------|
| `QuestConfig.launchActions` | Запуск вложенного квеста при старте |
| `QuestStepConfig.stepAction` | Запуск квеста на определённом шаге |
| `QuestStepConfig.winActions` | Запуск квеста при победе шага |
| `QuestConfig.winActions` | Запуск следующего квеста в цепочке |
| `QuestTimerCallback` | Отложенный запуск по таймеру |

---

## 3. СИСТЕМА ACTIONS (ДЕЙСТВИЯ)

### Архитектура
**Путь:** `Assets/PROJECTS/CrossProject/Core/Actions/ActionService.cs`

```
IActionConfig  →  ActionService.Execute()  →  IAction.Execute()
```

### Доступные Actions для квестов:

| Action | Путь | Описание |
|--------|------|----------|
| `LaunchQuestAction` | CrossProject/Core/Actions/Implementations/ | Запуск квеста |
| `LoseQuestAction` | CrossProject/Core/Actions/Implementations/ | Проигрыш квеста |
| `SpawnNPCAction` | L2Farm/Scripts/Actions/SpawnNPC/ | Спаун NPC |
| `DespawnNPCAction` | L2Farm/Scripts/Actions/DespawnNPC/ | Удаление NPC |
| `ShowMonologAction` | L2Farm/Scripts/Features/SimpleMonolog/ | Показ диалога |
| `GiveResourcesAction` | L2Farm/Scripts/Features/ResourceProduction/ | Выдача ресурсов |
| `SpendResourcesAction` | L2Farm/Scripts/Actions/SpendResources/ | Трата ресурсов |
| `StartProductionAction` | L2Farm/Features/ResourceProduction/Actions/ | Запуск производства |
| `FinishBuildingAction` | L2Farm/Features/Buildings/Actions/ | Завершение постройки |

---

## 4. СИСТЕМА CONDITIONS (УСЛОВИЯ)

### Архитектура
**Путь:** `Assets/PROJECTS/CrossProject/Core/Conditions/ConditionService.cs`

```
IConditionConfig  →  ConditionService.Check()  →  bool
```

### Доступные Conditions для квестов:

| Condition | Путь | Описание |
|-----------|------|----------|
| `TrueCondition` | CrossProject/Core/Conditions/ | Всегда true (простой диалог) |
| `FalseCondition` | CrossProject/Core/Conditions/ | Всегда false |
| `AndCondition` | CrossProject/Core/Conditions/ | Логическое И |
| `OrCondition` | CrossProject/Core/Conditions/ | Логическое ИЛИ |
| `QuestCompletedCondition` | L2Farm/Scripts/Conditions/ | Квест завершён |
| `QuestNotCompletedCondition` | L2Farm/Scripts/Conditions/ | Квест НЕ завершён |
| `HasEnoughResourcesCondition` | L2Farm/Scripts/Conditions/ | Достаточно ресурсов |
| `ProductionCompletedCondition` | L2Farm/Scripts/Conditions/ | Производство завершено |
| `GiveResourcesCondition` | L2Farm/Scripts/ | Показать награду ресурсами |

---

## 5. СОХРАНЕНИЕ СОСТОЯНИЯ

### QuestsLogPart
**Путь:** `Assets/PROJECTS/CrossProject/Core/Quests/QuestsLogPart.cs`

```csharp
public class QuestsLogPart : IGameStatePart
{
    public Dictionary<QuestId, int> launchedQuests;  // Активные квесты + шаг
    public HashSet<QuestId> wonQuests;               // Завершённые успешно
    public HashSet<QuestId> lostQuests;              // Проваленные
}
```

Сохраняется через `GameStateManager` в PlayerPrefs (JSON).

---

## 6. QUEST INDICATION (МАРКЕРЫ НА КАРТЕ)

### IndicationTypeSetConfig
**Путь:** `Assets/PROJECTS/L2Farm/Configs/IndicationTypeSetConfig.asset`

Доступные типы индикаторов:

| ID | Описание |
|----|----------|
| `Quest_Active` | Активный квест (белая иконка) |
| `Quest_Ready` | Квест готов к завершению (жёлтая иконка) |
| `Quest_Boss` | Босс-квест (красная иконка) |
| `Production_Wheat` | Производство пшеницы |
| `Production_Flour` | Производство муки |
| `Production_Planks` | Производство досок |
| `Production_Ore` | Производство руды |
| `Production_Meat` | Производство мяса |

**Паттерн использования:**
- Первые шаги квеста → `Production_*` (иконка ресурса)
- Финальный шаг → `Quest_Ready` (готов к завершению)

---

## 7. PRODUCTION СИСТЕМА

### ProductionSetConfig
**Путь:** `Assets/PROJECTS/L2Farm/Configs/ProductionSetConfig.asset`

```yaml
- id: Farm_Production_Meat
  timeToProduceInSeconds: 10
  buildingId: Farm
  finishQuest: Meat_Production_Finish  # ← Квест запускается автоматически!
```

**ВАЖНО:** Поле `finishQuest` связывает производство с квестом. Когда производство завершается, `TimerCreator.BindQuest()` автоматически запускает указанный квест.

### Связь Production → Quest

```
StartProductionAction(Farm_Production_Meat)
    ↓
TimerCreator.Launch().BindQuest(Meat_Production_Finish)
    ↓
(10 секунд)
    ↓
QuestService.TryLaunch(Meat_Production_Finish)
```

---

## 8. UI КВЕСТОВ

| Компонент | Путь | Описание |
|-----------|------|----------|
| `ActiveQuestsScreen` | L2Farm/Scripts/Features/QuestsScreen/ | Список активных квестов |
| `ActiveQuestsItem` | L2Farm/Scripts/Features/QuestsScreen/ | Элемент списка |
| `NpcQuestMarker` | L2Farm/Scripts/Features/NPC/ | Маркер над NPC (!/?) |
| `QuestIndication` | L2Farm/Scripts/Features/QuestIndication/ | Маркер цели на карте |

---

## 9. FLOW ВЫПОЛНЕНИЯ КВЕСТА

```
┌─────────────────────────────────────────────────────────┐
│  1. ЗАПУСК: LaunchQuestAction.Execute()                │
│     └─> QuestService.TryLaunch(questId)                │
│         ├─> Добавить в launchedQuests[questId] = 0     │
│         ├─> Выполнить launchActions (SpawnNPC и т.д.)  │
│         ├─> OnQuestLaunch событие                      │
│         └─> Если proceedAfterLaunch → TryProceedStepsOf│
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│  2. ВЫПОЛНЕНИЕ ШАГА: TryProceedStepsOf(questId)        │
│     ├─> Выполнить stepAction                           │
│     ├─> Проверить loseCondition → если true → ForceLose│
│     ├─> Проверить winCondition                         │
│     │   ├─> false → ждём (return false)                │
│     │   └─> true  → выполнить winActions, stepIndex++  │
│     └─> OnQuestProceed(questId, newStep)               │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│  3. ЗАВЕРШЕНИЕ: ForceWin(questId)                      │
│     ├─> Удалить из launchedQuests                      │
│     ├─> Добавить в wonQuests                           │
│     ├─> Выполнить winActions                           │
│     └─> OnQuestWin событие                             │
└─────────────────────────────────────────────────────────┘
```

---

## 10. ПРИМЕР: ЦЕПОЧКА ПРОИЗВОДСТВЕННЫХ КВЕСТОВ

### Meat_Production (пример)

```
Build_Farm_Finish ✓
    └─> winActions: LaunchQuest(Meat_Production)

Meat_Production
    ├─> launchActions: SpawnNPC(Farmer)
    ├─> steps[0]:
    │   ├─> stepAction: ShowMonolog("Bring me wheat...")
    │   ├─> winCondition: HasEnoughResources(1x Wheat)
    │   └─> winActions: SpendResources(1x Wheat)
    └─> winActions: LaunchQuest(Meat_Production_Start)

Meat_Production_Start
    ├─> launchActions:
    │   ├─> StartProduction(Farm_Production_Meat)  ← 10 сек
    │   └─> SpawnNPC(Farmer)
    ├─> steps[0]:
    │   ├─> stepAction: ShowMonolog("Wait a moment...")
    │   └─> winCondition: ProductionCompleted(Farm_Production_Meat)
    └─> winActions: [] (пусто, переход через TimerCreator)

    ↓ (через ProductionSetConfig.finishQuest)

Meat_Production_Finish
    ├─> launchActions:
    │   ├─> SpawnNPC(Farmer)
    │   └─> LoseQuest(Meat_Production_Start)  ← закрыть предыдущий
    ├─> steps[0]:
    │   ├─> stepAction: ShowMonolog("Here's your meat!")
    │   ├─> winCondition: GiveResourcesCondition(10x Meat)
    │   └─> winActions: GiveResource(10x Meat)
    └─> winActions: LaunchQuest(Meat_Production)  ← ЦИКЛ!
```

---

## 11. КАК ДОБАВИТЬ НОВЫЙ КВЕСТ

### Шаг 1: Проверить/добавить ресурсы
- `Assets/PROJECTS/L2Farm/Configs/ResourceSetConfig.asset`

### Шаг 2: Добавить индикатор (если нужен новый)
- `Assets/PROJECTS/L2Farm/Configs/IndicationTypeSetConfig.asset`

### Шаг 3: Добавить производство (для production-квестов)
- `Assets/PROJECTS/L2Farm/Configs/ProductionSetConfig.asset`
- Указать `finishQuest` для автозапуска финального квеста

### Шаг 4: Добавить квесты в QuestSetConfig
- `Assets/PROJECTS/L2Farm/Configs/QuestSetConfig.asset`

**Структура YAML:**
1. Добавить rid в секцию `items:`
2. Добавить данные в секцию `RefIds:`

### Шаг 5: Связать с существующим квестом
- Добавить `LaunchQuestActionConfig` в winActions родительского квеста

---

## 12. СПИСОК ФАЙЛОВ

### Core (CrossProject):
```
Assets/PROJECTS/CrossProject/Core/
├── Quests/
│   ├── QuestService.cs
│   ├── QuestConfig.cs
│   ├── QuestSetConfig.cs
│   ├── QuestStepConfig.cs
│   ├── QuestId.cs
│   ├── QuestsLogPart.cs
│   └── IndicationTypeSetConfig.cs
├── Actions/
│   ├── ActionService.cs
│   ├── Action.cs
│   └── Implementations/LaunchQuestAction/
└── Conditions/
    ├── ConditionService.cs
    ├── Condition.cs
    └── ConditionsImplementations/
```

### L2Farm (игровая логика):
```
Assets/PROJECTS/L2Farm/
├── Configs/
│   ├── QuestSetConfig.asset
│   ├── ProductionSetConfig.asset
│   ├── ResourceSetConfig.asset
│   └── IndicationTypeSetConfig.asset
└── Scripts/
    ├── L2FarmLifetimeScope.cs
    ├── Actions/ (SpawnNPC, DespawnNPC, SpendResources)
    ├── Conditions/ (QuestCompleted, HasEnoughResources)
    └── Features/
        ├── QuestsScreen/
        ├── QuestIndication/
        ├── NPC/NpcQuestMarker.cs
        └── SimpleMonolog/
```

---

## 13. ВАЖНЫЕ ЗАМЕЧАНИЯ

### rid в QuestSetConfig
- `rid` (Reference ID) — уникальный идентификатор для Unity SerializeReference
- Генерируется Unity автоматически
- При ручном добавлении использовать числа рядом с последним существующим rid

### Типичные ошибки
1. **Забыли добавить LaunchQuest** — квест не появится в игре
2. **Неправильный questIndication** — маркер не отобразится на карте
3. **Забыли finishQuest в ProductionSetConfig** — production-квест не продолжится
4. **Не закрыли предыдущий квест** — используйте `LoseQuestAction` в launchActions финального квеста

### Цикличные квесты
Для бесконечного повторения добавьте в `winActions` финального квеста:
```yaml
- type: LaunchQuestActionConfig
  questId: <первый_квест_цепочки>
```
