using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEnemiesKilledCondition : MonoBehaviour, ILockableCondition
{
    bool ILockableCondition.Evaluate()
    {
        if (transform.childCount == 0)
        {
            return false;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                return false;
            }
        }
        return true;
    }
}
