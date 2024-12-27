using _Project.Scripts.DirectionProviders;
using Unity.Netcode;
using UnityEngine;

namespace _Project.Scripts.Main
{
    public class ClientInputHandler : NetworkBehaviour
    {
        private IDirectionProvider _directionProvider;
        private ICharacterProcessor _characterProcessor;
    
        private Vector2 _lastInput;

        private void Awake()
        {
            _directionProvider = new KeyboardDirectionProcessor();
            _characterProcessor = GetComponent<ICharacterProcessor>();
        }

        private void Update()
        {
            _directionProvider.Tick();
            var currentInput = _directionProvider.Direction;

            if (currentInput == _lastInput) 
                return;
        
            SendInputToServerRpc(currentInput);
            _lastInput = currentInput;
        }

        [ServerRpc]
        private void SendInputToServerRpc(Vector2 input)
        {
            _characterProcessor.SetInput(input);
        }
    }
}