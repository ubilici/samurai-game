using TMPro;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ButtonText;

    private const float SingleKeyFontSize = 60;
    private const float MultipleKeyFontSize = 35;

    public void Initialize(KeyCode key)
    {
        var text = key.ToString();
        if (text.Length == 1)
        {
            ButtonText.fontSize = SingleKeyFontSize;
        }
        else
        {
            ButtonText.fontSize = MultipleKeyFontSize;

            if (text.Contains("Arrow"))
            {
                text = text.Substring(0, text.Length - 5);
            }
        }

        ButtonText.text = text;
    }
}