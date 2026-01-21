using UnityEngine;
using UnityEngine.UI;

public class OpenValkaRegistration : MonoBehaviour
{
    [Header("Ссылка для перехода")]
    [Tooltip("Ссылка на страницу регистрации")]
    [SerializeField] private string targetURL = "https://site-1.valka.fans/ru/sign-up";

    [Header("Настройки визуала")]
    [SerializeField] private bool changeButtonText = true;
    [SerializeField] private string customButtonText = "Присоединиться";
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = new Color(0.9f, 0.9f, 0.9f, 1f);

    private Button button;

    void Awake()
    {
        InitializeButton();
    }

    void InitializeButton()
    {
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("Нет компонента Button на объекте: " + gameObject.name);
            enabled = false;
            return;
        }

        // Настраиваем обработчик клика
        button.onClick.AddListener(OpenRegistrationPage);

        // Настраиваем текст кнопки
        if (changeButtonText)
        {
            Text textComponent = button.GetComponentInChildren<Text>();
            if (textComponent != null)
            {
                textComponent.text = string.IsNullOrEmpty(customButtonText)
                    ? "Регистрация"
                    : customButtonText;
            }
        }

        // Настраиваем цвета (если это обычная кнопка)
        ColorBlock colors = button.colors;
        colors.normalColor = normalColor;
        colors.highlightedColor = hoverColor;
        button.colors = colors;

        Debug.Log($"Кнопка инициализирована. URL: {targetURL}");
    }

    void OpenRegistrationPage()
    {
        if (string.IsNullOrEmpty(targetURL))
        {
            Debug.LogError("URL не установлен!");
            return;
        }

        // Проверяем, что ссылка начинается с http:// или https://
        if (!targetURL.StartsWith("http://") && !targetURL.StartsWith("https://"))
        {
            Debug.LogWarning("URL может быть неполным. Добавляю https://");
            targetURL = "https://" + targetURL;
        }

        Debug.Log($"Переход по ссылке: {targetURL}");
        Application.OpenURL(targetURL);
    }

    // Метод для смены URL во время выполнения (опционально)
    public void SetNewURL(string newURL)
    {
        targetURL = newURL;
        Debug.Log($"URL изменен на: {newURL}");
    }

    void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OpenRegistrationPage);
        }
    }

    // Для отладки в редакторе
#if UNITY_EDITOR
    void OnValidate()
    {
        if (button == null)
            button = GetComponent<Button>();
    }
#endif
}