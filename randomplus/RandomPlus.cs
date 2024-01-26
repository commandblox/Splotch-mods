using Splotch;
using Splotch.Event;
using HarmonyLib;
using Splotch.Event.PlayerEvents;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RandomPlus
{
    public class RandomPlus : SplotchMod
    {
        public override void OnLoad()
        {
            Logger.Log("woo random");
            EventManager.RegisterEventListener(typeof(Events));
            Harmony.PatchAll(typeof(HarmonyPatches));
        }
    }

    public static class Events
    {
        [EventHandler]
        public static void onPlayerTick(PlayerTickEvent tickEvent)
        {
            try
            {
                FieldInfo gameSessionHandlerReferenceField = typeof(GameSessionHandler).GetField("selfRef", BindingFlags.Static | BindingFlags.NonPublic);
                GameSessionHandler gameSessionHandler = gameSessionHandlerReferenceField.GetValue(null) as GameSessionHandler;

                FieldInfo slimeControllersField = typeof(GameSessionHandler).GetField("slimeControllers", BindingFlags.Instance | BindingFlags.NonPublic);
                SlimeController[] slimeControllers = slimeControllersField.GetValue(gameSessionHandler) as SlimeController[];


                Player player = tickEvent.GetPlayer();


                for (int i = 0; i < player.Abilities.Count; i++)
                {
                    SlimeController playerSlimeController = slimeControllers[player.Id - 1];
                    RandomAbility randomAbility = player.Abilities[i].GetComponent<RandomAbility>();
                    if (randomAbility != null)
                    {
                        NamedSprite randomAbilityPrefab = RandomAbility.GetRandomAbilityPrefab(randomAbility.abilityIcons, randomAbility.abilityIcons_demo);
                        player.AbilityIcons[i] = randomAbilityPrefab.sprite;

                        GameObject randomAbilityGameObject = FixTransform.InstantiateFixed(randomAbilityPrefab.associatedGameObject, Vec2.zero);
                        randomAbilityGameObject.SetActive(value: false);
                        if (!player.CurrentAbilities[i].activeInHierarchy)
                        {
                            Updater.DestroyFix(player.CurrentAbilities[i]);
                            player.CurrentAbilities[i] = null;
                            Updater.DestroyFix(playerSlimeController.abilities[i]);
                            playerSlimeController.abilities[i] = null;
                        }
                        player.CurrentAbilities[i] = randomAbilityGameObject;

                        playerSlimeController.abilities[i] = randomAbilityGameObject.GetComponent<AbilityMonoBehaviour>();


                        playerSlimeController.AbilityReadyIndicators[i].SetSprite(player.AbilityIcons[i]);
                    }


                }
            } catch { }
        }
    }

    public static class HarmonyPatches
    {
        [HarmonyPatch(typeof(PhysicsBodyList<Circle>), "AddBody")]
        [HarmonyPrefix]
        public static void AddBodyIndexCheck(ref PhysicsBodyList<Circle> __instance)
        {
            FieldInfo collidersField = typeof(PhysicsBodyList<Circle>).GetField("nrOfColliders", BindingFlags.Instance | BindingFlags.NonPublic);
            if ((int) collidersField.GetValue(__instance) >= __instance.physicsBodies.Count())
            {
                collidersField.SetValue(__instance, __instance.physicsBodies.Count() - 1);
            }
        }

        [HarmonyPatch(typeof(PhysicsBodyList<Box>), "AddBody")]
        [HarmonyPrefix]
        public static void AddBodyIndexCheck(ref PhysicsBodyList<Box> __instance)
        {
            FieldInfo collidersField = typeof(PhysicsBodyList<Box>).GetField("nrOfColliders", BindingFlags.Instance | BindingFlags.NonPublic);
            if ((int)collidersField.GetValue(__instance) >= __instance.physicsBodies.Count())
            {
                collidersField.SetValue(__instance, __instance.physicsBodies.Count() - 1);
            }
        }

        [HarmonyPatch(typeof(PhysicsBodyList<RoundedRect>), "AddBody")]
        [HarmonyPrefix]
        public static void AddBodyIndexCheck(ref PhysicsBodyList<RoundedRect> __instance)
        {
            FieldInfo collidersField = typeof(PhysicsBodyList<RoundedRect>).GetField("nrOfColliders", BindingFlags.Instance | BindingFlags.NonPublic);
            if ((int)collidersField.GetValue(__instance) >= __instance.physicsBodies.Count())
            {
                collidersField.SetValue(__instance, __instance.physicsBodies.Count() - 1);
            }
        }
    }
}
  