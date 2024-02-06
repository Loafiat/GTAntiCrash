using System.Reflection;
using HarmonyLib;

namespace AntiCrash
{
	/// <summary>
	/// This class handles applying harmony patches to the game.
	/// You should not need to modify this class.
	/// </summary>
	/// yeah I'm using modding template lol
	public class HarmonyPatches
	{
		private static Harmony instance;

		public static bool IsPatched { get; private set; }
		public const string InstanceId = "Lofiat.AntiCrash";

		internal static void ApplyHarmonyPatches()
		{
			if (!IsPatched)
			{
				if (instance == null)
				{
					instance = new Harmony(InstanceId);
				}

				instance.PatchAll(Assembly.GetExecutingAssembly());
				IsPatched = true;
			}
		}

		internal static void RemoveHarmonyPatches()
		{
			if (instance != null && IsPatched)
			{
				instance.UnpatchSelf();
				IsPatched = false;
			}
		}
	}
}