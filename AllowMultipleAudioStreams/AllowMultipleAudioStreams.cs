using HarmonyLib;
using NeosModLoader;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace AllowMultipleAudioStreams
{
    public class AllowMultipleAudioStreams : NeosMod
    {
        public override string Name => "AllowMultipleAudioStreams";
        public override string Author => "eia485";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/EIA485/NeosAllowMultipleAudioStreams/";
        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("net.eia485.AllowMultipleAudioStreams");
            harmony.PatchAll();
        }

        [HarmonyPatch]
        class AllowMultipleAudioStreamsPatch
        {
            static MethodBase TargetMethod()
            {
                return AccessTools.Method(AccessTools.TypeByName("FrooxEngine.AudioStreamSpawner+<>c__DisplayClass11_0"), "<OnStartStreaming>b__0");
            }
            static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = new List<CodeInstruction>(instructions);
                for (int i = 0; i < codes.Count - 22; i++)
                {
                    if (codes[i].opcode == OpCodes.Ldarg_0 &
                        codes[i + 1].opcode == OpCodes.Ldfld &
                        codes[i + 2].opcode == OpCodes.Ldsfld &
                        codes[i + 3].opcode == OpCodes.Dup &
                        codes[i + 4].opcode == OpCodes.Brtrue_S &
                        codes[i + 5].opcode == OpCodes.Pop &
                        codes[i + 6].opcode == OpCodes.Ldsfld &
                        codes[i + 7].opcode == OpCodes.Ldftn &
                        codes[i + 8].opcode == OpCodes.Newobj &
                        codes[i + 9].opcode == OpCodes.Dup &
                        codes[i + 10].opcode == OpCodes.Stsfld)
                    {
                        for (int si = i; si < i + 22; si++)
                        {
                            codes[si].opcode = OpCodes.Nop;
                        }
                        break;
                    }
                }
                return codes;
            }
        }
    }
}