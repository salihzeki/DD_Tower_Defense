﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DapperDino.TD.Enemies;
using DapperDino.TD.Waves;
using System;

namespace DapperDino.TD.Waves
{
    public class WaveHandler : MonoBehaviour
    {
        [SerializeField] private int numberOfWaves = 1;
        [SerializeField] private int secondsBetweenWaves = 10;
        [SerializeField] private TMP_Text secondsRemainingText = null;
        [SerializeField] private WaveSpawner[] waveSpawners = new WaveSpawner[0];

        private int currentWave;
        private float secondsUntilNextWave;

        private readonly Dictionary<EnemyData, int> enemiesToKill = new Dictionary<EnemyData, int>();

        public static event Action OnPlayerWin;

        private void OnEnable()
        {
            WaveDestination.OnEnemyReachedEnd += HandleEnemyKilled;
            Enemy.OnKilled += HandleEnemyKilled;

        }

        private void Start()
        {
            GetNextWave();
            ResetTheCountdown();
        }

        private void OnDisable()
        {
            WaveDestination.OnEnemyReachedEnd -= HandleEnemyKilled;
            Enemy.OnKilled -= HandleEnemyKilled;
        }

        private void Update()
        {
             if(secondsUntilNextWave == 0f) { return; }

            secondsUntilNextWave -= Time.deltaTime;

            if (secondsUntilNextWave <= 0f)
            {
                secondsUntilNextWave = 0f;
                secondsRemainingText.enabled = false;

                StartNextWave();
            }
            secondsRemainingText.text = Mathf.Ceil(secondsUntilNextWave).ToString();

        }

        private void StartNextWave()
        {
            foreach (var spawner in waveSpawners)
            {
                spawner.StartWave(currentWave);
            }
        }

        private void HandleEnemyKilled(EnemyData enemyData)
        {
            if (enemiesToKill.ContainsKey(enemyData))
            {
                enemiesToKill[enemyData]--;

                if(enemiesToKill[enemyData] == 0)
                {
                    enemiesToKill.Remove(enemyData);
                }
            }

            if(enemiesToKill.Count==0)
            {
                currentWave++;

                if(currentWave == numberOfWaves)
                {
                    OnPlayerWin?.Invoke();
                    return;
                }

                GetNextWave();

                ResetTheCountdown();
            }
        }

        private void ResetTheCountdown()
        {
            secondsUntilNextWave = secondsBetweenWaves;
            secondsRemainingText.enabled = true;
        }

        private void GetNextWave()
        {
            foreach (var spawner in waveSpawners)
            {
                foreach (var newEnemy in spawner.GetWave(currentWave))
                {
                    if(enemiesToKill.ContainsKey(newEnemy.EnemyData))
                    {
                        enemiesToKill[newEnemy.EnemyData]++;
                    }
                    else
                    {
                        enemiesToKill.Add(newEnemy.EnemyData, 1);
                    }
                }
            }
        }
    }

}
