using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using ModdingUtils.Extensions;
using UnboundLib;
using System;

[HarmonyPatch(typeof(CharacterStatModifiersModifier))]
public static class TemporaryModifiers_Patch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(CharacterStatModifiersModifier.RemoveCharacterStatModifiersModifier), new Type[] { typeof(CharacterStatModifiers), typeof(bool) })]
    public static void RemoveCharacterStatModifiers_Patch(CharacterStatModifiers characterStatModifiers)
    {
        characterStatModifiers.SetFieldValue("remainingRespawns", Mathf.Min((int)characterStatModifiers.GetFieldValue("remainingRespawns"), characterStatModifiers.respawns));
    }
}
