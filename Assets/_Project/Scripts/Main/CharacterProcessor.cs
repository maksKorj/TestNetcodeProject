using Unity.Multiplayer.Center.NetcodeForGameObjectsExample;
using Unity.Netcode;
using UnityEngine;

namespace _Project.Scripts.Main
{
    public class CharacterProcessor : NetworkBehaviour, ICharacterProcessor
    {
        private readonly float _force = 12f;
        private readonly float _maxVelocity = 8f;

        private Vector3 _moveDirection;
    
        private Rigidbody _rb;

        private readonly NetworkVariable<Vector3> _syncedPosition = new();
        
        public void SetInput(Vector2 input)
        {
            _moveDirection = new Vector3(input.x, 0, input.y).normalized;
        }
    
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                Destroy(GetComponentInChildren<SetColorBasedOnOwnerId>());
                Destroy(GetComponentInChildren<MeshFilter>());
                Destroy(GetComponentInChildren<MeshRenderer>());
            }
        
            if (IsClient)
            {
                Destroy(_rb);
                Destroy(GetComponentInChildren<BoxCollider>());
            
                if(IsOwner == false)
                    Destroy(GetComponent<ClientInputHandler>());
            }
        }

        private void FixedUpdate()
        {
            if (IsServer)
            {
                HandleServerPhysics();
            }
        }

        private void Update()
        {
            if (IsServer)
                return;

            if (IsClient)
                InterpolateMovement();
        }

        private void HandleServerPhysics()
        {
            _rb.AddForce(_moveDirection * _force);

            var velocity = _rb.linearVelocity;
            var horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);

            if (horizontalVelocity.magnitude > _maxVelocity)
                horizontalVelocity = horizontalVelocity.normalized * _maxVelocity;

            _rb.linearVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);

            if (_syncedPosition.Value != _rb.position)
                _syncedPosition.Value = _rb.position;
        }

        private void InterpolateMovement()
        {
            transform.position = Vector3.Lerp(transform.position, _syncedPosition.Value, Time.deltaTime * 10f);
        }
    }
}
