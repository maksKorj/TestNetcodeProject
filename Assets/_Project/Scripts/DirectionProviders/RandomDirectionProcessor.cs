using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.DirectionProviders
{
    [Serializable]
    public class RandomDirectionProcessor : IDirectionProvider
    {
        public Vector2 Direction { get; private set; }
        
        private float _timer;
        
        public void Tick()
        {
            if (_timer >= 0)
            {
                _timer -= Time.deltaTime;
                return;
            }

            _timer = 1;
            
            var direction = Vector2.zero;
            direction.x += GetRandomValue();
            direction.x -= GetRandomValue();
            direction.y += GetRandomValue();
            direction.y -= GetRandomValue();
            
            Direction = direction;
        }

        private int GetRandomValue()
        {
            return Random.Range(0, 100) >= 50 ? 0 : 1;
        }
    }
}