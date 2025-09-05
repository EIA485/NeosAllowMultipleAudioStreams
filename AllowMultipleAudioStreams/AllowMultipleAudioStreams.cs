using BepInEx;
using BepInEx.Logging;
using BepInEx.NET.Common;
using BepInExResoniteShim;
using FrooxEngine;
using HarmonyLib;
using System.Reflection;

namespace AllowMultipleAudioStreams
{
    [ResonitePlugin(PluginMetadata.GUID, PluginMetadata.NAME, PluginMetadata.VERSION, PluginMetadata.AUTHORS, PluginMetadata.REPOSITORY_URL)]
    [BepInDependency(BepInExResoniteShim.PluginMetadata.GUID)]
    public class AllowMultipleAudioStreams : BasePlugin
    {
        public override void Load() => HarmonyInstance.PatchAll();

        [HarmonyPatch]
        class AllowMultipleAudioStreamsPatch
        {
            static MethodBase TargetMethod() //anonymous method that deletes AudioStreamSpawner instance
            {
                foreach(var type in typeof(FrooxEngine.AudioStreamSpawner).GetNestedTypes(BindingFlags.NonPublic))
                {
                    var meth = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                        .FirstOrDefault(m => m.Name.Contains("OnStartStreaming") && m.ReturnType == typeof(void)
                        && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(AudioStreamController));
                    if (meth != null) return meth;
                }
                return null;
            }
            static bool Prefix() => false;
        }
    }
}