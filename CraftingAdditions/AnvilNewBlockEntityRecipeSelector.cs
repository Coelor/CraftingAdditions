using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace CraftingAdditions
{
public class AnvilNewBlockEntityRecipeSelector : GuiDialogGeneric
{
BlockPos blockEntityPos;
int prevSlotOver = -1;
List<SkillItem> skillItems;
bool didSelect = false;
Action<int> onSelectedRecipe;
Action onCancelSelect;

    private GuiDialogRecipeDetails recipeDetailsDialog;

    public AnvilNewBlockEntityRecipeSelector(string dialogTitle, ItemStack[] recipeOutputs, Action<int> onSelectedRecipe, Action onCancelSelect, BlockPos blockEntityPos, ICoreClientAPI capi) : base(dialogTitle, capi)
    {
        this.blockEntityPos = blockEntityPos;
        this.onSelectedRecipe = onSelectedRecipe;
        this.onCancelSelect = onCancelSelect;

        skillItems = new List<SkillItem>();
        recipeDetailsDialog = new GuiDialogRecipeDetails(capi);

        double size = GuiElementPassiveItemSlot.unscaledSlotSize + GuiElementItemSlotGrid.unscaledSlotPadding;

        for (int i = 0; i < recipeOutputs.Length; i++)
        {
            ItemStack stack = recipeOutputs[i];
            ItemSlot dummySlot = new DummySlot(stack);

            string key = GetCraftDescKey(stack);
            string desc = Lang.GetMatching(key);
            if (desc == key) desc = "";

            skillItems.Add(new SkillItem()
            {
                Code = stack.Collectible.Code.Clone(),
                Name = stack.GetName(),
                Description = desc,
                RenderHandler = (AssetLocation code, float dt, double posX, double posY) => {
                    double scsize = GuiElement.scaled(size - 5);
                    capi.Render.RenderItemstackToGui(dummySlot, posX + scsize / 2, posY + scsize / 2, 100, (float)GuiElement.scaled(GuiElementPassiveItemSlot.unscaledItemSize), ColorUtil.WhiteArgb);
                }
            });
        }

        SetupDialog();
    }

    public string GetCraftDescKey(ItemStack stack)
    {
        string name = "";
        string type = stack.Class.Name();
        name = stack.Collectible.Code?.Domain + AssetLocation.LocationSeparator + type + "craftdesc-" + stack.Collectible.Code?.Path;
        return name;
    }

    void SetupDialog()
    {
        int cnt = Math.Max(1, skillItems.Count);
        int cols = Math.Min(cnt, 7);
        int rows = (int)Math.Ceiling(cnt / (float)cols);

        double size = GuiElementPassiveItemSlot.unscaledSlotSize + GuiElementItemSlotGrid.unscaledSlotPadding;
        double innerWidth = Math.Max(300, cols * size);
        ElementBounds skillGridBounds = ElementBounds.Fixed(0, 30, innerWidth, rows * size);

        ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
        bgBounds.BothSizing = ElementSizing.FitToChildren;

        SingleComposer =
            capi.Gui
                .CreateCompo("toolmodeselect" + blockEntityPos, ElementStdBounds.AutosizedMainDialog)
                .AddShadedDialogBG(bgBounds, true)
                .AddDialogTitleBar(Lang.Get("Select Recipe"), OnTitleBarClose)
                .BeginChildElements(bgBounds)
                .AddSkillItemGrid(skillItems, cols, rows, OnSlotClick, skillGridBounds, "skillitemgrid")
                .EndChildElements()
                .Compose();

        SingleComposer.GetSkillItemGrid("skillitemgrid").OnSlotOver = OnSlotOver;
    }

    private void OnSlotOver(int num)
    {
        if (num >= skillItems.Count) return;

        if (num != prevSlotOver)
        {
            prevSlotOver = num;
            string name = skillItems[num].Name;
            string description = skillItems[num].Description;
            recipeDetailsDialog.UpdateRecipeDetails(name, description);
            if (!recipeDetailsDialog.IsOpened()) recipeDetailsDialog.TryOpen();
        }
    }

    private void OnSlotClick(int num)
    {
        onSelectedRecipe(num);
        didSelect = true;
        TryClose();
    }

    public override void OnGuiClosed()
    {
        base.OnGuiClosed();
        if (!didSelect)
        {
            onCancelSelect();
        }
        recipeDetailsDialog.TryClose();
    }

    public override bool TryClose()
    {
        return base.TryClose();
    }

    public override bool TryOpen()
    {
        return base.TryOpen();
    }

    private void OnTitleBarClose()
    {
        TryClose();
    }

    private readonly double floatyDialogPosition = 0.5;
    private readonly double floatyDialogAlign = 0.75;

    public override bool PrefersUngrabbedMouse => false;

    public override void OnRenderGUI(float deltaTime)
    {
        if (capi.Settings.Bool["immersiveMouseMode"])
        {
            Vec3d aboveHeadPos = new Vec3d(blockEntityPos.X + 0.5, blockEntityPos.Y + floatyDialogPosition, blockEntityPos.Z + 0.5);
            Vec3d pos = MatrixToolsd.Project(aboveHeadPos, capi.Render.PerspectiveProjectionMat, capi.Render.PerspectiveViewMat, capi.Render.FrameWidth, capi.Render.FrameHeight);
            if (pos.Z < 0) return;

            SingleComposer.Bounds.Alignment = EnumDialogArea.None;
            SingleComposer.Bounds.fixedOffsetX = 0;
            SingleComposer.Bounds.fixedOffsetY = 0;
            SingleComposer.Bounds.absFixedX = pos.X - SingleComposer.Bounds.OuterWidth / 2;
            SingleComposer.Bounds.absFixedY = capi.Render.FrameHeight - pos.Y - SingleComposer.Bounds.OuterHeight * floatyDialogAlign;
            SingleComposer.Bounds.absMarginX = 0;
            SingleComposer.Bounds.absMarginY = 0;
        }

        base.OnRenderGUI(deltaTime);
    }
}

}