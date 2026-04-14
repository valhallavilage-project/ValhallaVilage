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
        public string text;
    }

    [Serializable]
    public class TwoFaRequiredInfo
    {
        public string status;
        public string text;
        public string auth_token;
        public string login;
    }

    public class LogInScreen : MonoBehaviour
    {
        [SerializeField] private ServerService serverService;
        [SerializeField] private string launcherKey;
        [SerializeField] private TMP_InputField logInField;
        [SerializeField] private TMP_InputField passwordField;
        [SerializeField] private Button logInButton;
        [SerializeField] private Button skipButton;
        [SerializeField] private CanvasGroup logInGroup;
        [SerializeField] private TwoFactorAuthPopup twoFactorAuthPopup;
        [SerializeField] private TMP_Text statusText;

        public LogInData LogInData { get; private set; }
        public bool IsGuest { get; private set; }

        public event Action OnLoginedIn;

        private void Awake()
        {
            if (twoFactorAuthPopup == null)
                twoFactorAuthPopup = GetComponentInChildren<TwoFactorAuthPopup>(true);

            if (statusText == null)
            {
                var statusGo = transform.Find("Panel/StatusText");
                if (statusGo != null)
                    statusText = statusGo.GetComponent<TMP_Text>();
            }

            logInButton.onClick.AddListener(() =>
            {
                if (!string.IsNullOrEmpty(logInField.text) && !string.IsNullOrEmpty(passwordField.text))
                {
                    LogInData = new LogInData { email = logInField.text, password = passwordField.text, serverId = ServerService.ServerId };
                    logInGroup.interactable = false;
                    SetStatus("Вход...");
                }
            });

            skipButton.onClick.AddListener(() =>
            {
                IsGuest = true;
                LogInData = new LogInData { email = "guest", password = "guest", serverId = "guest" };
                logInGroup.interactable = false;
                SetStatus("Гостевой вход...");
            });
        }

        public async UniTask<bool> LogIn()
        {
            LogInData = await GetLogInData();
            if (IsGuest)
            {
                Close(true);
                serverService.Activate(true);
                return true;
            }

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
                SetStatus("Успешно!");
                PlayerPrefs.SetString("LogIn", LogInData.email);
                PlayerPrefs.SetString("Password", LogInData.password);
                PlayerPrefs.SetString("ServerId", LogInData.serverId);
                Close(true);
                serverService.Activate(IsGuest);
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
            SetStatus("");
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

        private void SetStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;

            if (!string.IsNullOrEmpty(message))
                Debug.Log($"[LogIn] {message}");
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

                SetStatus("Автовход...");
                Close(false);
            }

            return LogInData;
        }


        private async UniTask<bool> TryToLogIn()
        {
            SetStatus("Подключение к серверу...");

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
                SetStatus($"Ошибка сети: {request.error}");
                Debug.LogError($"Login request error: {request.error}");
                logInGroup.interactable = true;
                return false;
            }

            try
            {
                var responseText = request.downloadHandler.text;
                Debug.Log($"[LogIn] HTTP {request.responseCode} Response: {responseText}");
                SetStatus($"HTTP {request.responseCode}: {responseText.Substring(0, Math.Min(100, responseText.Length))}");

                // Записываем ответ в файл для диагностики
                var logPath = System.IO.Path.Combine(Application.persistentDataPath, "login_response.txt");
                System.IO.File.WriteAllText(logPath, $"HTTP {request.responseCode}\n{responseText}");
                Debug.Log($"[LogIn] Response saved to: {logPath}");

                var info = JsonUtility.FromJson<TwoFaRequiredInfo>(responseText);

                if (info == null || string.IsNullOrEmpty(info.status))
                {
                    SetStatus($"Ошибка: не удалось прочитать ответ");
                    Debug.LogError($"[LogIn] Failed to parse response: {responseText}");
                    logInGroup.interactable = true;
                    return false;
                }

                Debug.Log($"[LogIn] status={info.status} auth_token={info.auth_token}");

                if (info.status == "success")
                {
                    SetStatus("Вход выполнен!");
                    return true;
                }

                if (info.status == "2fa_required")
                {
                    SetStatus("Требуется 2FA подтверждение...");
                    return await Handle2FA(info.auth_token, "email");
                }

                var errorMessage = !string.IsNullOrEmpty(info.text) ? info.text : info.status;
                SetStatus($"Ошибка: {errorMessage}");
                logInGroup.interactable = true;
                return false;
            }
            catch (Exception e)
            {
                SetStatus("Ошибка обработки ответа");
                Debug.LogError($"Login parse error: {e.Message}");
                logInGroup.interactable = true;
                return false;
            }
        }

        private async UniTask<bool> Handle2FA(string authToken, string method)
        {
            if (twoFactorAuthPopup == null)
            {
                SetStatus("Ошибка: 2FA попап не привязан!");
                Debug.LogError("[LogIn] twoFactorAuthPopup is null! Assign it in Inspector.");
                logInGroup.interactable = true;
                return false;
            }

            SetStatus("Отправка кода на почту...");

            bool sent = await SendTwoFaCode(method);
            if (!sent)
            {
                SetStatus("Не удалось отправить 2FA код");
                logInGroup.interactable = true;
                return false;
            }

            twoFactorAuthPopup.OnResendRequested += () => SendTwoFaCode(method);

            var code = await twoFactorAuthPopup.GetCode();

            twoFactorAuthPopup.ClearResendHandler();

            if (string.IsNullOrEmpty(code))
            {
                SetStatus("2FA отменено");
                logInGroup.interactable = true;
                return false;
            }

            SetStatus("Проверка 2FA кода...");
            return await TryToLoginWith2FA(authToken, method, code);
        }

        private async UniTask<bool> SendTwoFaCode(string method)
        {
            WWWForm form = new WWWForm();
            form.AddField("launcher_key", launcherKey);
            form.AddField("login", LogInData.email);
            form.AddField("method", method);

            using var request = UnityWebRequest.Post(ServerService.MainURL + "send_2fa_confirm_code", form);
            request.SetRequestHeader("Accept", "application/json");

            await request.SendWebRequest().ToUniTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Send 2FA code error: {request.error}");
                return false;
            }

            try
            {
                var info = JsonUtility.FromJson<LogInInfo>(request.downloadHandler.text);
                Debug.Log($"[LogIn] Send 2FA: {info.status} - {info.text}");
                return info.status == "success";
            }
            catch
            {
                Debug.LogError("Send 2FA code parse error");
                return false;
            }
        }

        private async UniTask<bool> TryToLoginWith2FA(string authToken, string method, string code)
        {
            WWWForm form = new WWWForm();
            form.AddField("launcher_key", launcherKey);
            form.AddField("auth_token", authToken);
            form.AddField("login", LogInData.email);
            form.AddField("code", code);
            form.AddField("method", method);

            using var request = UnityWebRequest.Post(ServerService.MainURL + "login_with_2fa_code", form);
            request.SetRequestHeader("Accept", "application/json");

            await request.SendWebRequest().ToUniTask();

            if (request.result != UnityWebRequest.Result.Success)
            {
                var errorText = $"Ошибка сети: {request.error}";
                SetStatus(errorText);
                Debug.LogError($"2FA login error: {request.error}");
                logInGroup.interactable = true;
                return false;
            }

            try
            {
                var info = JsonUtility.FromJson<LogInInfo>(request.downloadHandler.text);
                Debug.Log($"[LogIn] 2FA result: {info.status} - {info.text}");

                if (info.status == "success")
                {
                    SetStatus("2FA подтверждено!");
                    return true;
                }

                var errorMessage = !string.IsNullOrEmpty(info.text) ? info.text : info.status;
                SetStatus($"Ошибка: {errorMessage}");
                logInGroup.interactable = true;
                return false;
            }
            catch
            {
                SetStatus("Ошибка обработки ответа");
                logInGroup.interactable = true;
                return false;
            }
        }
    }
}
