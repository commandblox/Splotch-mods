using Splotch;
using Splotch.Event;
using HarmonyLib;
using BoplFixedMath;
using System.Reflection;

namespace Accelerate
{
    public class Accelerate : SplotchMod
    {
        public override void OnLoad()
        {
            Logger.Log("speedy bopl go brr");
            EventManager.RegisterEventListener(typeof(Events));
            Harmony.PatchAll(typeof(HarmonyPatches));
        }
    }

    public static class Events
    {

    }

    public static class HarmonyPatches
    {
        [HarmonyPatch(typeof(PlayerPhysics), nameof(PlayerPhysics.Init))]
        [HarmonyPrefix]
        public static void PatchAccel(ref PlayerPhysics __instance)
        {
            FieldInfo PlatformSlipperyness01 = typeof(PlayerPhysics).GetField("PlatformSlipperyness01", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo IcePlatformSlipperyness01 = typeof(PlayerPhysics).GetField("IcePlatformSlipperyness01", BindingFlags.Instance | BindingFlags.NonPublic);
            
            IcePlatformSlipperyness01.SetValue(__instance, (Fix) 1.01);
            PlatformSlipperyness01.SetValue(__instance, (Fix) 1.01);
        }
    }
}
  