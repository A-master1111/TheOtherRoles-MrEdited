using System.Linq;
using HarmonyLib;

namespace TheOtherRoles.Patches
{
    [HarmonyPatch(typeof(BurgerMinigame), nameof(BurgerMinigame.Begin))]
    public static class BurgerMinigameBeginPatch
    {
        public static BurgerMinigame instance = null;
        public const int DefaultBurgerLayers = 5;
        public const int MinBurgerLayers = 3;
        public const int MaxBurgerLayers = 30;
        static int layers = -1;

        public static void reset()
		{
            layers = -1;
        }

        public static void Postfix(BurgerMinigame __instance, [HarmonyArgument(0)] PlayerTask task)
        {
            if (layers == -1)
			{
                if (CustomOptionHolder.enabledTaskVsMode.getBool() && CustomOptionHolder.taskVsMode_EnabledBurgerMakeMode.getBool())
                {
                    layers = CustomOptionHolder.taskVsMode_BurgerMakeMode_BurgerLayers.getInt();
                }
                else
                {
                    int minLayers = CustomOptionHolder.burgerMinigameBurgerMinLayers.getInt();
                    int maxLayers = CustomOptionHolder.burgerMinigameBurgerMaxLayers.getInt();
                    if (minLayers > maxLayers) maxLayers = minLayers;
                    layers = minLayers + (minLayers < maxLayers ? UnityEngine.Random.Range(0, maxLayers - minLayers + 1) : 0);
                }
                layers = UnityEngine.Mathf.Clamp(layers, MinBurgerLayers, MaxBurgerLayers);
            }

            if (DefaultBurgerLayers == layers)
                return;
            MakeBurgers(__instance, layers);
        }

        static void MakeBurgers(BurgerMinigame __instance, int layers)
        {
            var expectedToppings = new Il2CppStructArray<BurgerToppingTypes>(layers + 1); // + 1(Plate)

            expectedToppings[0] = BurgerToppingTypes.Plate;
            bool isLettuceMode = false;
            if (BoolRange.Next(0.1f))
            {
                expectedToppings[1] = BurgerToppingTypes.Lettuce;
                expectedToppings[layers] = BurgerToppingTypes.Lettuce;
                isLettuceMode = true;
            }
            else
            {
                expectedToppings[1] = BurgerToppingTypes.BottomBun;
                expectedToppings[layers] = BurgerToppingTypes.TopBun;
            }

            int typeNum = isLettuceMode ? 3 : 4;
            int toppingTypeMin = isLettuceMode ? 3 : 2;
            int layers2 = layers - 2;
            int typeMax = 2;
            if (layers2 > typeNum * typeMax)
            {
                typeMax = layers2 / typeNum;
                if (layers2 % typeNum != 0)
                    ++typeMax;
            }

            for (int i = 2; i < layers; i++)
            {
                BurgerToppingTypes top = (BurgerToppingTypes)IntRange.Next(toppingTypeMin, 6);
                if (expectedToppings.Count((BurgerToppingTypes t) => t == top) >= typeMax)
                {
                    i--;
                }
                else
                {
                    expectedToppings[i] = top;
                }
            }

            __instance.ExpectedToppings = expectedToppings;

            if (BoolRange.Next(0.01f))
            {
                BurgerToppingTypes burgerToppingTypes = __instance.ExpectedToppings[layers];
                __instance.ExpectedToppings[layers] = __instance.ExpectedToppings[layers - 1];
                __instance.ExpectedToppings[layers - 1] = burgerToppingTypes;
            }

            var toppings = new Il2CppSystem.Collections.Generic.List<BurgerTopping>();
            for (int i = 0; i < __instance.Toppings.Length; ++i)
            {
                if (layers > 10)
                {
                    __instance.Toppings[i].Offset *= 0.5f;
                    __instance.Toppings[i].transform.localScale *= 0.5f;
                }
                toppings.Add(__instance.Toppings[i]);
            }
            var baseLettuce = __instance.Toppings.FirstOrDefault((t) => t.ToppingType == BurgerToppingTypes.Lettuce);
            var baseMeat = __instance.Toppings.FirstOrDefault((t) => t.ToppingType == BurgerToppingTypes.Meat);
            var baseOnion = __instance.Toppings.FirstOrDefault((t) => t.ToppingType == BurgerToppingTypes.Onion);
            var baseTomato = __instance.Toppings.FirstOrDefault((t) => t.ToppingType == BurgerToppingTypes.Tomato);

            int addNum = typeMax - 2;
            for (int i = 0; i < addNum; ++i)
            {
                var lettuce = UnityEngine.Object.Instantiate(baseLettuce, baseLettuce.transform.parent);
                toppings.Add(lettuce);
                var meat = UnityEngine.Object.Instantiate(baseMeat, baseMeat.transform.parent);
                toppings.Add(meat);
                var onion = UnityEngine.Object.Instantiate(baseOnion, baseOnion.transform.parent);
                toppings.Add(onion);
                var tomato = UnityEngine.Object.Instantiate(baseTomato, baseTomato.transform.parent);
                toppings.Add(tomato);
            }

            __instance.Toppings = (Il2CppReferenceArray<BurgerTopping>)toppings.ToArray();

            var paperSlots = new Il2CppSystem.Collections.Generic.List<UnityEngine.SpriteRenderer>();
            for (int i = 0; i < __instance.PaperSlots.Length; ++i)
            {
                paperSlots.Add(__instance.PaperSlots[i]);
                if (i >= layers)
                    __instance.PaperSlots[i].gameObject.SetActive(false);
            }

            for (int i = 0, n = expectedToppings.Length - paperSlots.Count - 1; i < n; ++i)
            {
                var slot = UnityEngine.Object.Instantiate(paperSlots[0], paperSlots[0].transform.parent);
                paperSlots.Add(slot);
            }

            __instance.PaperSlots = (Il2CppReferenceArray<UnityEngine.SpriteRenderer>)paperSlots.ToArray();

            for (int j = 1; j < __instance.ExpectedToppings.Length; j++)
                __instance.PaperSlots[j - 1].sprite = __instance.PaperToppings[(int)__instance.ExpectedToppings[j]];

            if (layers > DefaultBurgerLayers)
            {
                var baseX = __instance.PaperSlots[0].transform.localPosition.x + ((layers - 1) / 10) * -0.5f;
                var baseY = __instance.PaperSlots[0].transform.localPosition.y - 0.2f;
                for (int i = 0; i < __instance.PaperSlots.Length; ++i)
                {
                    var t = __instance.PaperSlots[i].transform;
                    t.localPosition = new UnityEngine.Vector3(baseX + 1.0f * (i / 10), baseY + 0.4f * (i % 10), t.localPosition.z);
                    t.localScale *= 0.5f;
                }
            }
        }
    }
}
