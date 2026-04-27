using System;
using System.Collections;
using System.Collections.Generic;
using CrossProject.Core.Quests;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using VContainer;
using VContainer.Unity;


namespace CrossProject.Core
{
    [Serializable]
    public class DailyInfo
    {
        public DailyTasks[] daily_tasks;
        public ServerList[] server_list;
    }

    [Serializable]
    public class DailyTasks
    {
        public int id;
        public int key;
        public bool completed_today;
        public int[] server_ids;
    }

    [Serializable]
    public class ServerList
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class DailyIdToName
    {
        public int id;
        public string name;
    }
    [Serializable]
    public class DailyTaskComplitedInfo
    {
        public string text;
        public string status;
    }

    public class ServerService : MonoBehaviour, IInitializable
    {
        [SerializeField] private DailyServerScreen dailyServerScreen;
        [SerializeField] private string launcherKey;
        [SerializeField] private DailyIdToName[] dailyIdToNames;

        private static readonly string[] ApiCandidates =
        {
            "https://api.glb.vlh-app.net/app/",
            "https://api.vlh-app.net/app/",
            "https://api.wesper.in/app/",
            "https://site-1.valka.fans/app/"
        };

        private static string _mainUrl = ApiCandidates[0];
        private static bool _apiResolved;
        private static bool _apiResolving;

        public static string MainURL => _mainUrl;

        public const string ServerId = "13";

        private QuestService _questService;
        private PlayerDailyTaskCheck _playerDailyTaskCheck;

        public bool IsInitialized { get; private set; }
        public bool IsGuest { get; private set; }

        private bool isActive;

        [Inject]
        private void Construct(QuestService questService)
        {
            _questService = questService;
        }

        private void Awake()
        {
            dailyServerScreen.Close();
            DontDestroyOnLoad(this);
        }

        public void Activate(bool isGuest = false)
        {
            IsGuest = isGuest;
            _questService.IsGuest = isGuest;
            isActive = true;

            if (!isGuest)
                EnsureApiResolvedAsync().Forget();
        }

        public static async UniTask EnsureApiResolvedAsync()
        {
            if (_apiResolved) return;

            if (_apiResolving)
            {
                await UniTask.WaitUntil(() => _apiResolved);
                return;
            }

            _apiResolving = true;
            try
            {
                await ResolveMainUrlAsync();
            }
            finally
            {
                _apiResolved = true;
                _apiResolving = false;
            }
        }

        private static async UniTask ResolveMainUrlAsync()
        {
            foreach (var candidate in ApiCandidates)
            {
                if (await IsAlive(candidate))
                {
                    _mainUrl = candidate;
                    Debug.Log($"[ApiEndpoint] Selected: {candidate}");
                    return;
                }
                Debug.LogWarning($"[ApiEndpoint] Candidate unreachable: {candidate}");
            }

            _mainUrl = ApiCandidates[0];
            Debug.LogError($"[ApiEndpoint] All candidates failed, falling back to: {_mainUrl}");
        }

        private static async UniTask<bool> IsAlive(string baseUrl)
        {
            try
            {
                using var req = UnityWebRequest.Head(baseUrl);
                req.timeout = 5;
                await req.SendWebRequest().ToUniTask();
                return req.responseCode > 0;
            }
            catch
            {
                return false;
            }
        }

        public async UniTask Initialize()
        {
            IsInitialized = true;
            while (!isActive || !_questService.IsInitialized)
            {
                await UniTask.Yield();
            }

            if (IsGuest)
            {
                return;
            }
            
            _questService.OnQuestWin += (QuestId questId) =>
            {
                SendQuest(questId);
            };

            await LaunchQuests();

            await UniTask.Delay(1000);
            _playerDailyTaskCheck = FindAnyObjectByType<PlayerDailyTaskCheck>();
            while (!_playerDailyTaskCheck)
            {
                await UniTask.Yield();
                _playerDailyTaskCheck = FindAnyObjectByType<PlayerDailyTaskCheck>();
            }
            _playerDailyTaskCheck.OnHitDailyMaster += (string masterName) =>
            {
                foreach (var item in dailyIdToNames)
                {
                    if(item.name == masterName)
                    {
                        LaunchQuests();
                    }
                }
            };
        }

        private async UniTask SendQuest(QuestId questId)
        {
            DailyInfo dailyInfo = await TryToGetDailyTasks();
            if (dailyInfo != null && dailyInfo.daily_tasks != null)
            {
                DailyTasks[] dailyTasks = dailyInfo.daily_tasks;

                foreach (var dailyTask in dailyTasks)
                {
                    foreach (var item in dailyIdToNames)
                    {
                        if (dailyTask.id == item.id && questId.ToString() == item.name)
                        {
                            dailyServerScreen.Open();
                            int index = await dailyServerScreen.GetIndex(dailyInfo.server_list);
                            await TryToPostDailyTasks(dailyInfo, dailyTask, dailyInfo.server_list[index].id);
                            dailyServerScreen.Close();
                            break;
                        }
                    }
                }
            }
        }

        private async UniTask LaunchQuests()
        {
            DailyInfo dailyInfo = await TryToGetDailyTasks();
            if (dailyInfo != null && dailyInfo.daily_tasks != null)
            {
                DailyTasks[] dailyTasks = dailyInfo.daily_tasks;
                Debug.Log($"[DailyDebug] Server returned {dailyTasks.Length} daily tasks");
                foreach (var dailyTask in dailyTasks)
                {
                    Debug.Log($"[DailyDebug]   id={dailyTask.id} key={dailyTask.key} completed_today={dailyTask.completed_today}");
                    if (!dailyTask.completed_today)
                    {
                        foreach (var item in dailyIdToNames)
                        {
                            if (dailyTask.id == item.id)
                            {
                                Debug.Log($"[DailyDebug]   -> mapping to quest {item.name}, ForceLose({item.name}_Early) + TryLaunch({item.name})");
                                await _questService.ForceLose(new QuestId(item.name + "_Early"));
                                var launched = await _questService.TryLaunch(new QuestId(item.name));
                                Debug.Log($"[DailyDebug]   TryLaunch({item.name}) returned {launched}");
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning("[DailyDebug] DailyInfo empty or daily_tasks null.");
            }
        }

        private async UniTask TryToPostDailyTasks(DailyInfo dailyInfo, DailyTasks dailyTask, int serverId)
        {
            if (dailyInfo.server_list.Length > 0)
            {
                await EnsureApiResolvedAsync();

                WWWForm form = new WWWForm();
                form.AddField("launcher_key", launcherKey);
                form.AddField("id", dailyTask.id);
                form.AddField("value", dailyTask.key);
                form.AddField("server_id", serverId);


                using var request = UnityWebRequest.Post(MainURL + "daily_tasks", form);
                request.SetRequestHeader("Accept", "application/json");

                await request.SendWebRequest().ToUniTask();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Login request error: {request.error}");
                }

                try
                {
                    DailyTaskComplitedInfo info = JsonUtility.FromJson<DailyTaskComplitedInfo>(request.downloadHandler.text);

                    Debug.Log(info.text + " " + info.status);
                }
                catch
                {
                    Debug.Log("Daily task not success " + request.error);
                }
            }
        }

        private async UniTask<DailyInfo> TryToGetDailyTasks()
        {
            await EnsureApiResolvedAsync();

            using var request = UnityWebRequest.Get(MainURL + "daily_tasks?launcher_key=" + launcherKey);
            request.SetRequestHeader("Accept", "application/json");

            await request.SendWebRequest().ToUniTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Login request error: {request.error}");
                return new DailyInfo();
            }

            try
            {
                DailyInfo dailyInfo = JsonUtility.FromJson<DailyInfo>(request.downloadHandler.text);
                return dailyInfo;
            }
            catch
            {
                Debug.Log("Daily not success " + request.error);
                return new DailyInfo();
            }
        }
    }
}

