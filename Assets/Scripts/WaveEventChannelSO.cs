using System;
using UnityEngine;

namespace AzizStuff
{
    [CreateAssetMenu(menuName = "Aziz/Events/Wave Event Channel", fileName = "WaveEventChannel_")]
    public class WaveEventChannelSO : ScriptableObject
    {
        public event Action<int> OnRaised;

        public void Raise(int waveIndex) => OnRaised?.Invoke(waveIndex);
    }
}
