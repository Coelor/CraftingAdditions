using Vintagestory.API.Common;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.MathTools;
using Vintagestory.API.Config;

namespace CraftingAdditions
{

    public class CraftingAdditionsMod : ModSystem
    {
        ICoreClientAPI capi;
        ICoreServerAPI sapi;
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            if (api.Side == EnumAppSide.Server)
            {
                sapi = api as ICoreServerAPI;
            }
            else
            {
                capi = api as ICoreClientAPI;
            }
            api.RegisterBlockClass("BlockAnvilNew", typeof(BlockAnvilNew));
            api.RegisterBlockEntityClass("BlockEntityAnvilNew", typeof(BEAnvilNew));
        }
    }
}