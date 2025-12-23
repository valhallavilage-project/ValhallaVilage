using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using CrossProject.Core;

namespace CrossProject.Ui.Core
{
    [Serializable]
    public class LogInData
    {
        public string serverId;
        public string email;
        public string password;
    }

    [Serializable]
    public class LogInInfo
    {
        public string status;
    }

    public class LogInScreen : MonoBehaviour
    {
        [SerializeField] private ServerService serverService;
        [SerializeField] private string launcherKey;
        [SerializeField] private TMP_InputField logInField;
        [SerializeField] private TMP_InputField passwordField;
        [SerializeField] private Button logInButton;
        [SerializeField] private CanvasGroup logInGroup;

        public LogInData LogInData { get; private set; }

        public event Action OnLoginedIn;

        private void Awake()
        {
            logInButton.onClick.AddListener(() =>
            {
                if (!string.IsNullOrEmpty(logInField.text) && !string.IsNullOrEmpty(passwordField.text))
                {
                    LogInData = new LogInData { email = logInField.text, password = passwordField.text, serverId = ServerService.ServerId };
                    logInGroup.interactable = false;
                }
            });
        }

        public async UniTask<bool> LogIn()
        {
            LogInData = await GetLogInData();
            bool success = await TryToLogIn();

            while(!success)
            {
                PlayerPrefs.DeleteKey("LogIn");
                PlayerPrefs.DeleteKey("Password");
                PlayerPrefs.DeleteKey("ServerId");

                LogInData = await GetLogInData();
                success = await TryToLogIn();
            }
            if(success)
            {
                PlayerPrefs.SetString("LogIn", LogInData.email);
                PlayerPrefs.SetString("Password", LogInData.password);
                PlayerPrefs.SetString("ServerId", LogInData.serverId);
                Close(true);
                serverService.Activate();
            }
            return success;
        }

        private void Open()
        {
            LogInData = null;
            logInField.text = "";
            passwordField.text = "";
            logInGroup.interactable = true;
            logInGroup.gameObject.SetActive(true);
        }

        private void Close(bool full)
        {
            if (full)
            {
                gameObject.SetActive(false);
            }
            else
            {
                logInGroup.gameObject.SetActive(false);
            }
        }


        private async UniTask<LogInData> GetLogInData()
        {
            if (!PlayerPrefs.HasKey("LogIn"))
            {
                Open();

                while (LogInData == null)
                {
                    await UniTask.Delay(1000);
                }
            }
            else
            {
                LogInData = new LogInData
                {
                    email = PlayerPrefs.GetString("LogIn"),
                    password = PlayerPrefs.GetString("Password"),
                    serverId = PlayerPrefs.GetString("ServerId")
                };

                Close(false);
            }

            return LogInData;
        }


        private async UniTask<bool> TryToLogIn()
        {
            WWWForm form = new WWWForm();
            form.AddField("launcher_key", launcherKey);
            form.AddField("type_login", "email");
            form.AddField("email", LogInData.email);
            form.AddField("password", LogInData.password);
            form.AddField("sid", LogInData.serverId);

            using var request = UnityWebRequest.Post(ServerService.MainURL + "signin", form);
            request.SetRequestHeader("Accept", "application/json");

            await request.SendWebRequest().ToUniTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Login request error: {request.error}");
                return false;
            }

            try
            {
                LogInInfo logInInfo = JsonUtility.FromJson<LogInInfo>(request.downloadHandler.text);
                Debug.Log("Login " + logInInfo.status);
                return logInInfo.status == "success";
            }
            catch
            {
                Debug.Log("Login not success");
                return false;
            }
        }
    }
}
