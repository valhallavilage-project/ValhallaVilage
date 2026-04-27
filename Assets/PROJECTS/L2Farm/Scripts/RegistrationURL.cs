using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OpenValkaRegistration : MonoBehaviour
{
    private static readonly string[] RegistrationHosts =
    {
        "https://site-1.valka.fans",
        "https://site-2.valka.fans",
        "https://site-3.valka.fans"
    };

    private const string RegistrationPath = "/ru/sign-up";

    [Header("Appearance")]
    [SerializeField] private bool changeButtonText = true;
    [SerializeField] private string customButtonText = "Регистрация";
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(0.9f, 0.9f, 0.9f, 1f);

    private Button button;
    private static string _resolvedUrl;
    private static bool _opening;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("[Registration] Button component missing on: " + gameObject.name);
            enabled = false;
            return;
        }

        button.onClick.AddListener(OnClicked);

        if (changeButtonText)
        {
            var textComponent = button.GetComponentInChildren<Text>();
            if (textComponent != null)
                textComponent.text = string.IsNullOrEmpty(customButtonText) ? "Регистрация" : customButtonText;
        }

        var colors = button.colors;
        colors.normalColor = normalColor;
        colors.highlightedColor = hoverColor;
        button.colors = colors;
    }

    private void OnClicked()
    {
        if (_opening) return;
        OpenRegistrationAsync().Forget();
    }

    private static async UniTask OpenRegistrationAsync()
    {
        _opening = true;
        try
        {
            if (!string.IsNullOrEmpty(_resolvedUrl))
            {
                Debug.Log($"[Registration] Opening cached URL: {_resolvedUrl}");
                Application.OpenURL(_resolvedUrl);
                return;
            }

            foreach (var host in RegistrationHosts)
            {
                if (await IsAlive(host))
                {
                    _resolvedUrl = host + RegistrationPath;
                    Debug.Log($"[Registration] Selected: {_resolvedUrl}");
                    Application.OpenURL(_resolvedUrl);
                    return;
                }
                Debug.LogWarning($"[Registration] Host unreachable: {host}");
            }

            _resolvedUrl = RegistrationHosts[0] + RegistrationPath;
            Debug.LogError($"[Registration] All hosts failed, falling back to: {_resolvedUrl}");
            Application.OpenURL(_resolvedUrl);
        }
        finally
        {
            _opening = false;
        }
    }

    private static async UniTask<bool> IsAlive(string url)
    {
        try
        {
            using var req = UnityWebRequest.Head(url);
            req.timeout = 5;
            await req.SendWebRequest().ToUniTask();
            return req.responseCode > 0;
        }
        catch
        {
            return false;
        }
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(OnClicked);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (button == null)
            button = GetComponent<Button>();
    }
#endif
}
