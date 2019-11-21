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

        var playerInputs = PlayerInput.all;
        
        var playerInput1 = playerInputs[0];
        var playerInput2 = playerInputs[1];
        
        if (gamepad == 2)
        {
            playerInput1.SwitchCurrentControlScheme($"Player1_Gamepad", Gamepad.current);
            playerInput2.SwitchCurrentControlScheme($"Player2_Gamepad", Gamepad.current);
        }
        else if (joysticks == 2)
        {
            playerInput1.SwitchCurrentControlScheme($"Player1_Joystick", Joystick.current);
            playerInput2.SwitchCurrentControlScheme($"Player2_Joystick", Joystick.current);
        }
        else if (gamepad == 1 && joysticks == 1)
        {
            playerInput1.SwitchCurrentControlScheme($"Player1_Gamepad", Gamepad.current);
            playerInput2.SwitchCurrentControlScheme($"Player2_Joystick", Joystick.current);
        }
        else if (playerInput1.currentControlScheme == null)
        {
            playerInput1.SwitchCurrentControlScheme($"Player1_Keyboard", Keyboard.current);
        }
        else if (playerInput2.currentControlScheme == null)
        {
            playerInput2.SwitchCurrentControlScheme($"Player2_Keyboard", Keyboard.current);
        }
        
        foreach (var actionEvent in playerInput1.actionEvents)
        {
            if (actionEvent.actionName == "Restart")
            {
                void Restart(InputAction.CallbackContext callback)
                {
                    PlayerManager.instance.InstantRestart();
                }

                actionEvent.AddListener(Restart);
            }

            if (actionEvent.actionName == "Quit")
            {
                void Quit(InputAction.CallbackContext call)
                {
                    PlayerManager.instance.Quit();
                }

                actionEvent.AddListener(Quit);
            }
        }
        
        foreach (var actionEvent in playerInput2.actionEvents)
        {
            if (actionEvent.actionName == "Restart")
            {
                void Restart(InputAction.CallbackContext callback)
                {
                    PlayerManager.instance.InstantRestart();
                }

                actionEvent.AddListener(Restart);
            }

            if (actionEvent.actionName == "Quit")
            {
                void Quit(InputAction.CallbackContext call)
                {
                    PlayerManager.instance.Quit();
                }

                actionEvent.AddListener(Quit);
            }
        }
    }
}