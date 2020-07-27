﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DapperDino.TD.Towers;
using DapperDino.TD.Enemies;

namespace DapperDino.TD.Combat
{
    public class LaserAttack : MonoBehaviour
    {
        [SerializeField] private float fireRate = 0.5f;
        [SerializeField] private TowerData towerData = null;
        [SerializeField] private Transform spawnPoint = null;
        [SerializeField] private LineRenderer lineRenderer = null;

        private float timer;

        private TargetGetter targetGetter;

        private void Start() => targetGetter = GetComponent<TargetGetter>();

        private void Update()
        {
            Enemy target = targetGetter.Target;

             if (target != null)
            {
                lineRenderer.positionCount = 2;

                lineRenderer.SetPositions(new Vector3[] {
                    spawnPoint.position,
                    target.transform.position
                });
            }
             else
            {
                lineRenderer.positionCount = 0;
            }

            timer -= Time.deltaTime;

            if (timer > 0f) { return; }

            if (target != null)
            {
                target.DealDamage(Mathf.CeilToInt(towerData.DPS * fireRate));
                Debug.Log("fireRate:" + fireRate + "\nDPS: " + towerData.DPS + "Damage: " + Mathf.CeilToInt(towerData.DPS * fireRate));
                timer = fireRate;
            }


        }


    }

}
