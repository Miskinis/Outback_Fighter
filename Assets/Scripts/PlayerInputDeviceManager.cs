using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputDeviceManager : MonoBehaviour
{
    public static PlayerInputDeviceManager main;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
            ReassignDevices();
        }
    }

    public static void ReassignDevices()
    {
        int gamepad = 0;
        int joysticks = 0;

        foreach (var playerInput in FindObjectsOfType<PlayerInput>())
        {
            foreach (var inputDevice in InputSystem.devices)
            {
                if (inputDevice == Gamepad.current)
                {
                    gamepad++;
                }

                if (inputDevice == Joystick.current)
                {
                    joysticks++;
                }
            }

            if (gamepad == 2)
            {
                playerInput.SwitchCurrentControlScheme($"Player{playerInput.playerIndex + 1}_Gamepad", Gamepad.current);
            }
            else if (joysticks == 2)
            {
                playerInput.SwitchCurrentControlScheme($"Player{playerInput.playerIndex + 1}_Joystick", Joystick.current);
            }
            else if (playerInput.currentControlScheme == null)
            {
                playerInput.SwitchCurrentControlScheme($"Player{playerInput.playerIndex + 1}_Keyboard", Keyboard.current);
            }
        }
    }
}