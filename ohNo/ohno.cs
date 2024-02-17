using Splotch;
using Splotch.Event;
using HarmonyLib;
using BoplFixedMath;
using Unity.Mathematics;
using System.Reflection;
using UnityEngine;
using System;
using UnityEngine.Rendering;

namespace ohno
{
    public class ohno : SplotchMod
    {
        public override void OnLoad()
        {
            Logger.Log("ohno");
            EventManager.RegisterEventListener(typeof(Events));
            Harmony.PatchAll(typeof(HarmonyPatches));

        }
    }

    public static class Events
    {

    }

    public static class HarmonyPatches
    {
        [HarmonyPatch(typeof(SlimeController), nameof(SlimeController.UpdateSim))]
        [HarmonyPrefix]
        public static void SlimeControllerUpdate(ref SlimeController __instance)
        {
            FieldInfo abilityCooldownTimersField = typeof(SlimeController).GetField("abilityCooldownTimers", BindingFlags.NonPublic | BindingFlags.Instance);
            Fix[] abilityCooldownTimers = abilityCooldownTimersField.GetValue(__instance) as Fix[];

            for (int i = 0; i < abilityCooldownTimers.Length; i++)
            {
                abilityCooldownTimers[i] = (Fix)10000L;
            }

            abilityCooldownTimersField.SetValue(__instance, abilityCooldownTimers);
        }

        [HarmonyPatch(typeof(Player), nameof(Player.Scale), MethodType.Setter)]
        [HarmonyPrefix]
        public static bool PlayerScaleSet(ref Player __instance, ref Fix value)
        {
            FieldInfo playerScaleField = typeof(Player).GetField("scale", BindingFlags.NonPublic | BindingFlags.Instance);
            playerScaleField.SetValue(__instance, value);
            return false;
        }

        [HarmonyPatch(typeof(DPhysicsBox), nameof(DPhysicsBox.Scale), MethodType.Setter)]
        [HarmonyPrefix]
        public static void BoxColliderScaleSet(ref DPhysicsBox __instance)
        {
            __instance.MinScale = Fix.MinValue;
            __instance.MaxScale = Fix.MaxValue;
        }

        [HarmonyPatch(typeof(DPhysicsCircle), nameof(DPhysicsCircle.Scale), MethodType.Setter)]
        [HarmonyPrefix]
        public static void CircleColliderScaleSet(ref DPhysicsCircle __instance)
        {
            __instance.MinScale = Fix.MinValue;
            __instance.MaxScale = Fix.MaxValue;
        }

        [HarmonyPatch(typeof(DPhysicsRoundedRect), nameof(DPhysicsRoundedRect.Scale), MethodType.Setter)]
        [HarmonyPrefix]
        public static void RectColliderScaleSet(ref DPhysicsRoundedRect __instance)
        {
            __instance.MinScale = Fix.MinValue;
            __instance.MaxScale = Fix.MaxValue;
        }

        [HarmonyPatch(typeof(PlayerCollision), nameof(PlayerCollision.UpdateSim))]
        [HarmonyPrefix]
        public static void PlayerCollisionUpdate(ref PlayerCollision __instance)
        {
            FieldInfo maxAllowedClonesAndBodiesField = typeof(PlayerCollision).GetField("maxAllowedClonesAndBodies", BindingFlags.Instance | BindingFlags.NonPublic);
            maxAllowedClonesAndBodiesField.SetValue(__instance, int.MaxValue);
        }
    }
}