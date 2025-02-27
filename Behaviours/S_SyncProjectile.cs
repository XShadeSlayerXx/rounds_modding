using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class S_SyncProjectile : MonoBehaviour
{
    SyncProjectile sync = null;
    void Start()
    {
        sync = GetComponentInParent<SyncProjectile>();
        sync.active = true;
    }
}
