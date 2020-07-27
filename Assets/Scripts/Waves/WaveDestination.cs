using System;
using System.Collections;
using System.Collections.Generic;
using DapperDino.TD.Enemies;

using UnityEngine;

namespace DapperDino.TD.Waves
{
    public class WaveDestination : MonoBehaviour
    {
        public static event Action<EnemyData> OnEnemyReachedEnd;

        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponent<Enemy>(out var enemy))
            {
                return;
            }

            OnEnemyReachedEnd?.Invoke(enemy.EnemyData);

            Destroy(enemy.gameObject);
        }
    }
}

