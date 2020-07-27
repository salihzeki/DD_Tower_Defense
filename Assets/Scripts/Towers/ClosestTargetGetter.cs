using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DapperDino.TD.Enemies;

namespace DapperDino.TD.Towers
{
    public class ClosestTargetGetter : TargetGetter
    {
        protected override void FindTarget()
        {
            int colliderCount = Physics.OverlapSphereNonAlloc(transform.position, towerData.Range, colliderBuffer, layerMask); //Check collisions on a (center, radius, array to store, with objects on layerMask layer) returns the number of collusions.

            Enemy closestEnemy = null;
            float closestDistance = Mathf.Infinity; //System.Math uses doubles, UnityEngine.Mathf uses floats. Since Unity uses floats, it's generally better to use Mathf so you're not constantly converting floats/doubles.

            for (int i = 0; i < colliderCount; i++)
            {
                float distanceSquared = (colliderBuffer[i].transform.position - transform.position).sqrMagnitude;

                if(distanceSquared < closestDistance * closestDistance)
                {
                    if (colliderBuffer[i].TryGetComponent<Enemy>(out var enemy))
                    {
                        closestDistance = Mathf.Sqrt(distanceSquared);

                        closestEnemy = enemy;
                    }
                }
            }

            Target = closestEnemy;
        }
    }

}
