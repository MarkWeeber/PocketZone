using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action<Vector2> MovementEvent;
    public event Action<bool> FireEvent;
    public event Action InventoryOpenEvent;
    public event Action EscapeEvent;

    private Controls controls;
    private bool inventoryOpen;
    private bool escapePress;

    private void OnEnable()
    {
        if (controls == null)
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
        }
        controls.Player.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            FireEvent?.Invoke(true);
        }
        else if (context.canceled)
        {
            FireEvent?.Invoke(false);
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(!inventoryOpen)
            {
                InventoryOpenEvent?.Invoke();
                inventoryOpen = true;
            }
        }
        else if (context.canceled)
        {
            if(inventoryOpen)
            {
                inventoryOpen = false;
            }
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!escapePress)
            {
                EscapeEvent?.Invoke();
                escapePress = false;
            }
            
        }
        else if (context.canceled)
        {
            if (escapePress)
            {
                escapePress = false;
            };
        }
    }
}
