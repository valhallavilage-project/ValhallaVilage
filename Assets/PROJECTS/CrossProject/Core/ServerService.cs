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

        public const string MainURL = "https://site-1.valka.fans/app/";

        public const string ServerId = "13";

        private QuestService _questService;

        public bool IsInitialized { get; private set; }

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

        public void Activate()
        {
            isActive = true;
        }

        public async UniTask Initialize()
        {
            IsInitialized = true;
            while (!isActive || !_questService.IsInitialized)
            {
                await UniTask.Yield();
            }

            _questService.OnQuestWin += (QuestId questId) =>
            {
                SendQuest(questId);
            };

            await LaunchQuests();
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
                foreach (var dailyTask in dailyTasks)
                {
                    if (!dailyTask.completed_today)
                    {
                        foreach (var item in dailyIdToNames)
                        {
                            if (dailyTask.id == item.id)
                            {
                                await _questService.ForceLose(new QuestId(item.name + "_Early"));
                                await _questService.TryLaunch(new QuestId(item.name));
                                break;
                            }
                        }
                    }
                }
            }
        }

        private async UniTask TryToPostDailyTasks(DailyInfo dailyInfo, DailyTasks dailyTask, int serverId)
        {
            if (dailyInfo.server_list.Length > 0)
            {
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

