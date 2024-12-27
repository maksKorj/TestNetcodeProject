using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.DirectionProviders
{
    [Serializable]
    public class KeyboardDirectionProcessor : IDirectionProvider
    {
        public Vector2 Direction { get; private set; }
        
        public void Tick()
        {
            var direction = Vector2.zero;
            
            if (Keyboard.current.aKey.isPressed)
            {
                direction.x += -1;
            }
            else if (Keyboard.current.dKey.isPressed)
            {
                direction.x += 1;
            }
            
            if (Keyboard.current.wKey.isPressed)
            {
                direction.y += 1;
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                direction.y -= 1;
            }
            
            Direction = direction;
        }
    }
}