using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
#if USE_GAMIUM
#endif
    

public class KeycodeScene : MonoBehaviour
{
    public TextMeshProUGUI codeText;
    private void Update()
    {
        if (null == Keyboard.current)
        {
            return;
        }
        foreach (var code in Keyboard.current.allKeys)
        {
            if (code.isPressed)
            {
                codeText.text = code.keyCode.ToString();
            }
        }
    }
}