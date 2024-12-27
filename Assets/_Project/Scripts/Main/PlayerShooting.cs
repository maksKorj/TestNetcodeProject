using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;

    private float _timer = 1f;
    
    private void Update()
    {
        if(IsServer) return;
        if (!IsOwner) return;

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }
        
        _timer = Random.Range(0f, 0.2f);
        ShootServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShootServerRpc()
    {
        Vector3 playerPosition = transform.position;
        ShootProjectile(playerPosition, Vector3.forward);
        ShootProjectile(playerPosition, Vector3.back);
        ShootProjectile(playerPosition, Vector3.left);
        ShootProjectile(playerPosition, Vector3.right);
    }

    private void ShootProjectile(Vector3 position, Vector3 direction)
    {
        var projectileInstance = Instantiate(_projectilePrefab, position, Quaternion.identity);
        projectileInstance.GetComponent<NetworkObject>().Spawn();
        projectileInstance.Init(position, direction);
    }
}