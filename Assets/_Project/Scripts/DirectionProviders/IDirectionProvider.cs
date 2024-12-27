using UnityEngine;

namespace _Project.Scripts.DirectionProviders
{
    public interface IDirectionProvider
    {
        public Vector2 Direction { get; }

        public void Tick();
    }
}