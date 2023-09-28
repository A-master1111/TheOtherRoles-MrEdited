using System.Collections.Generic;
using TheOtherRoles.Utilities;
using UnityEngine;

namespace TheOtherRoles.Objects {
    public class BirthdayCake {
        public enum CakeType
		{
            Default,
            Yasuna,
            sizeof_CakeType,
		}

        public GameObject cakeObj { get; private set; }

        public BirthdayCake(Transform parent, CakeType cakeType, Vector3 position, Vector3 scale)
        {
            cakeObj = new GameObject("cake");
            cakeObj.transform.SetParent(parent);
            cakeObj.transform.localPosition = position;
            cakeObj.transform.localScale = scale;

            // Add base cake.
            var cakeRend = cakeObj.AddComponent<SpriteRenderer>();
            switch (cakeType)
            {
                case CakeType.Default:
                case CakeType.Yasuna:
                    cakeRend.sprite = getSprite(0);
                    break;
            }
            cakeRend.material = FastDestroyableSingleton<HatManager>.Instance.PlayerMaterial;
            cakeRendList.Add(cakeRend);

            // Add cake parts.
            switch (cakeType)
			{
				case CakeType.Yasuna:
					{
                        var cakeChildObj = new GameObject("cake_child");
                        cakeChildObj.transform.SetParent(cakeObj.transform);
                        cakeChildObj.transform.localPosition = Vector3.zero;
                        cakeChildObj.transform.localScale = Vector3.one;

                        var spriteRenderer2 = cakeChildObj.AddComponent<SpriteRenderer>();
                        spriteRenderer2.sprite = getSprite(1);
                    }
                    break;
            }
        }

        public void SetColorId(int colorId)
        {
            foreach (var r in cakeRendList)
                PlayerMaterial.SetColors(colorId, r);
        }

        public void SetColorId(PlayerControl p)
		{
            foreach (var r in cakeRendList)
                p.SetPlayerMaterialColors(r);
        }

        List<SpriteRenderer> cakeRendList = new List<SpriteRenderer>();

        static Sprite[] sprite = new Sprite[2];
        static Sprite getSprite(int idx)
        {
            if (idx >= sprite.Length) return null;
            if (sprite[idx]) return sprite[idx];
            switch (idx)
            {
                case 0:
                    sprite[idx] = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.BirthdayCake00.png", 300f);
                    break;
                case 1:
                    sprite[idx] = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.BirthdayCake01.png", 300f);
                    break;
            }
            return sprite[idx];
        }
    }
}