using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    private static List<GameObject> activeBullets = new List<GameObject>();

    public static void RegisterBullet(GameObject bullet)
    {
        activeBullets.Add(bullet);
    }

    public static void UnregisterBullet(GameObject bullet)
    {
        activeBullets.Remove(bullet);
    }

    public static void ClearAllBullets()
    {
        foreach (var bullet in activeBullets.ToList())
        {
            if (bullet != null)
            {
                Destroy(bullet);
            }
        }
        activeBullets.Clear();
    }
}
