using UnityEngine;
using Shade.Extensions;

public class Present : MonoBehaviour
{
    Player player;
    Gun gun;
    private const float variableDamage = 1f;

    void Start()
    {
        player = GetComponentInParent<Player>();
        gun = player.data.weaponHandler.gun;
        gun.GenAdditionalData().variableDamage = variableDamage;
    }

    void OnDestroy()
    {
        gun.GenAdditionalData().variableDamage = 0f;
    }
}
