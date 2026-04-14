using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CrossProject.Ui.Core
{
    public class TwoFactorAuthPopup : MonoBehaviour
    {
        [SerializeField] private TMP_InputField codeField;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button resendButton;
        [SerializeField] private TMP_Text statusText;

        private string _result;
        private bool _isDone;

        public event Func<UniTask<bool>> OnResendRequested;

        public void ClearResendHandler()
        {
            OnResendRequested = null;
        }

        private void Awake()
        {
            var window = transform.Find("Window");
            if (window != null)
            {
                if (codeField == null)
                    codeField = window.Find("CodeField")?.GetComponent<TMP_InputField>();
                if (submitButton == null)
                    submitButton = window.Find("SubmitButton")?.GetComponent<Button>();
                if (cancelButton == null)
                    cancelButton = window.Find("CancelButton")?.GetComponent<Button>();
                if (resendButton == null)
                    resendButton = window.Find("ResendButton")?.GetComponent<Button>();
                if (statusText == null)
                    statusText = window.Find("StatusText")?.GetComponent<TMP_Text>();
            }

            submitButton.onClick.AddListener(() =>
            {
                if (!string.IsNullOrEmpty(codeField.text))
                {
                    _result = codeField.text;
                    _isDone = true;
                }
            });

            cancelButton.onClick.AddListener(() =>
            {
                _result = null;
                _isDone = true;
            });

            resendButton.onClick.AddListener(() => ResendCode().Forget());
        }

        public async UniTask<string> GetCode()
        {
            _result = null;
            _isDone = false;
            codeField.text = "";
            SetStatus("Код отправлен на вашу почту");
            gameObject.SetActive(true);

            while (!_isDone)
            {
                await UniTask.Delay(100);
            }

            gameObject.SetActive(false);
            return _result;
        }

        private async UniTask ResendCode()
        {
            if (OnResendRequested == null) return;

            resendButton.interactable = false;
            SetStatus("Отправка кода...");

            bool success = await OnResendRequested.Invoke();

            SetStatus(success ? "Код отправлен повторно" : "Ошибка отправки кода");
            resendButton.interactable = true;
        }

        private void SetStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;
        }
    }
}
