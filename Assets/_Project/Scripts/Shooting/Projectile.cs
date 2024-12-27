using Unity.Netcode;
using UnityEngine;

namespace _Project.Scripts.Shooting
{
    public class Projectile : NetworkBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _lifetime = 5f;

        private Vector3 _direction;
        private float _timeAlive;

        private readonly NetworkVariable<Vector3> _syncedPosition = new();

        public void Init(Vector3 startPosition, Vector3 shootDirection)
        {
            Initialize(startPosition, shootDirection);
            InitializeClientRpc(startPosition, shootDirection);
        }

        [ClientRpc]
        private void InitializeClientRpc(Vector3 startPosition, Vector3 shootDirection)
        {
            if (!IsClient) return;
            
            transform.position = startPosition;
            _direction = shootDirection.normalized;
            _timeAlive = 0f;
        }

        public void Initialize(Vector3 startPosition, Vector3 shootDirection)
        {
            if (!IsServer) return;
            
            transform.position = startPosition;
            _direction = shootDirection.normalized;
            _timeAlive = 0f;
            _syncedPosition.Value = startPosition;
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsClient)
            {
                transform.position = _syncedPosition.Value;
            }
        }

        private void Update()
        {
            if (IsServer)
            {
                HandleServerMovement();
            }
            else if (IsClient)
            {
                SyncClientPosition();
            }
        }

        private void HandleServerMovement()
        {
            transform.position += _direction * (_speed * Time.deltaTime);
            _syncedPosition.Value = transform.position; 

            _timeAlive += Time.deltaTime;
            if (_timeAlive >= _lifetime)
            {
                Despawn();
            }
        }

        private void SyncClientPosition()
        {
            transform.position = Vector3.Lerp(transform.position, _syncedPosition.Value, Time.deltaTime * 10f);
        }

        private void Despawn()
        {
            if (IsServer)
            {
                NetworkObject.Despawn();
                Destroy(gameObject);
            }
        }
    }
}
