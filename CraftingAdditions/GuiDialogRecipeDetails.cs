using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace CraftingAdditions
{
    public class GuiDialogRecipeDetails : GuiDialogGeneric
    {
        private string recipeName = "";
        private string recipeDescription = "";

        public GuiDialogRecipeDetails(ICoreClientAPI capi) : base("Recipe Details", capi)
        {
            SetupDialog();
        }

        private void SetupDialog()
        {
            ElementBounds textBounds = ElementBounds.Fixed(10, 30, 300, 40);
            ElementBounds descBounds = textBounds.BelowCopy(0, 10, 0, 0);

            ElementBounds bgBounds = ElementBounds.Fill.WithFixedPadding(GuiStyle.ElementToDialogPadding);
            bgBounds.BothSizing = ElementSizing.FitToChildren;

            SingleComposer =
                capi.Gui
                    .CreateCompo("recipedetails", ElementStdBounds.AutosizedMainDialog)
                    .AddShadedDialogBG(bgBounds, true)
                    .AddDialogTitleBar("Recipe Details", OnTitleBarClose)
                    .BeginChildElements(bgBounds)
                    .AddDynamicText(recipeName, CairoFont.WhiteSmallishText(), textBounds, "name")
                    .AddDynamicText(recipeDescription, CairoFont.WhiteDetailText(), descBounds, "desc")
                    .EndChildElements()
                    .Compose();
        }

        public void UpdateRecipeDetails(string name, string description)
        {
            recipeName = name;
            recipeDescription = description;
            SingleComposer.GetDynamicText("name").SetNewText(recipeName);
            SingleComposer.GetDynamicText("desc").SetNewText(recipeDescription);
        }

        private void OnTitleBarClose()
        {
            TryClose();
        }
    }
}