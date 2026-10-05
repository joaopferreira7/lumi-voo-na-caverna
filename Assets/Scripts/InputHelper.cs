using UnityEngine;

/// <summary>
/// Centraliza a leitura de entradas (teclado, mouse e toque).
/// Funciona com o Input Manager antigo e com o novo Input System.
/// </summary>
public static class InputHelper
{
    public static bool FlapPressed()
    {
#if ENABLE_INPUT_SYSTEM
        var kb = UnityEngine.InputSystem.Keyboard.current;
        var mouse = UnityEngine.InputSystem.Mouse.current;
        var touch = UnityEngine.InputSystem.Touchscreen.current;
        return (kb != null && (kb.spaceKey.wasPressedThisFrame || kb.upArrowKey.wasPressedThisFrame || kb.wKey.wasPressedThisFrame))
            || (mouse != null && mouse.leftButton.wasPressedThisFrame)
            || (touch != null && touch.primaryTouch.press.wasPressedThisFrame);
#else
        return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)
            || Input.GetMouseButtonDown(0)
            || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
#endif
    }

    public static bool RestartPressed()
    {
#if ENABLE_INPUT_SYSTEM
        var kb = UnityEngine.InputSystem.Keyboard.current;
        return (kb != null && (kb.rKey.wasPressedThisFrame || kb.enterKey.wasPressedThisFrame)) || FlapPressed();
#else
        return Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Return) || FlapPressed();
#endif
    }
}
