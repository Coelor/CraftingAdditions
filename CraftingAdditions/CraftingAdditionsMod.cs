using Vintagestory.API.Common;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.MathTools;
using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using HarmonyLib;
using Vintagestory.Client.NoObf;

namespace CraftingAdditions {
    public class CraftingAdditionsMod : ModSystem {
        private ICoreAPI _api;
        private ICoreClientAPI _capi;
        private ICoreServerAPI _sapi;
        private string _ModID;

        private Harmony _harmony;
        public override void Start(ICoreAPI api) {
            this._ModID = Mod.Info.ModID;
            this._api = api;
            
            base.Start(this._api);
            if (this._api.Side == EnumAppSide.Server) {
                _sapi = this._api as ICoreServerAPI;
            } else {
                _capi = this._api as ICoreClientAPI;
            }
            // this.api.RegisterBlockClass("BlockAnvilNew", typeof(BlockAnvilNew));
            // this.api.RegisterBlockEntityClass("BlockEntityAnvilNew", typeof(BEAnvilNew));
            
            if(!Harmony.HasAnyPatches(_ModID))
            {
                _harmony = new Harmony(_ModID);
                _harmony.PatchAll();
            }
        }

        public override void Dispose()
        {
            _harmony?.UnpatchAll(_ModID);
        }
        
        //Test Patch
        [HarmonyPostfix]
        [HarmonyPatch(typeof(EntityItem), "ReceiveDamage")]
        public void ItemReceivedDamageAfter(EntityItem __instance, float damage) {
            this._api.Logger.Event("{0} took {1} damage!", __instance.Slot.Itemstack, damage);
            this._api.Logger.Event("Is it still alive? {0}", __instance.Alive);
        }
        
        
    }
}