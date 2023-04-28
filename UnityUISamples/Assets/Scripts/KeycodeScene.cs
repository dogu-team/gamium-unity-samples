using System;
using UnityEngine;
using UnityEngine.UI;
#if USE_GAMIUM
using Input = Gamium.Input;
#endif
    

public class KeycodeScene : MonoBehaviour
{
    public Text codeText;
    private void Update()
    {
        var codes = Enum.GetValues(typeof(KeyCode)) as KeyCode[];
        foreach (var code in codes)
        {
            if (Input.GetKey(code))
            {
                codeText.text = code.ToString();
            }
        }
    }
}
