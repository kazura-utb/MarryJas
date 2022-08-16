using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace MarryJas
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private const string DATA_EXTRA_DIALOGUE = "Data/ExtraDialogue.ja-JP";
        private const string DATA_NPC_DISPOSITIONS = "Data/NPCDispositions.ja-JP";
        private const string MAPS_FARMHOUSE1_MARRIAGE = "Maps/FarmHouse1_marriage";
        private const string ASSETS_FARMHOUSE1_MARRIAGE_JAS = "assets/FarmHouse1_marriage_jas.tbin";
        private const string MAPS_FARMHOUSE2_MARRIAGE = "Maps/FarmHouse2_marriage";
        private const string ASSETS_FARMHOUSE2_MARRIAGE_JAS = "assets/FarmHouse2_marriage_jas.tbin";
        private const string DATA_CHARACTER_JAS = "Characters/Jas";
        private const string ASSETS_CHARACTER_JAS = "assets/Jas_add_marriage.png";
        private const string SUMMIT_EVENT_KEY_JAS = "SummitEvent_Dialogue3_Jas";
        private const string JAS_MESSAGE_STRING_M = "@お兄ちゃん…\"/pause 300/faceDirection Jas 3/pause 400/speak Jas \"大人になってもよろしくね！$h";
        private const string JAS_MESSAGE_STRING_F = "@お姉ちゃん…\"/pause 300/faceDirection Jas 3/pause 400/speak Jas \"大人になってもよろしくね！$h";

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.Content.AssetRequested += OnAssetRequested;
        }


        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            //Monitor.Log(e.Name.ToString(), LogLevel.Debug);

            if (e.Name.IsEquivalentTo(DATA_NPC_DISPOSITIONS))
            {
                ChangeJasDatable(e);
            }
            else if (e.Name.IsEquivalentTo(MAPS_FARMHOUSE1_MARRIAGE))
            {
                ChangeFarmHouseMapData(e, ASSETS_FARMHOUSE1_MARRIAGE_JAS);
            }
            else if (e.Name.IsEquivalentTo(MAPS_FARMHOUSE2_MARRIAGE))
            {
                ChangeFarmHouseMapData(e, ASSETS_FARMHOUSE2_MARRIAGE_JAS);
            }
            else if (e.Name.IsEquivalentTo(DATA_CHARACTER_JAS)) 
            {
                ChangeJasImageAsset(e);
            }
            else if (e.Name.IsEquivalentTo(DATA_EXTRA_DIALOGUE)) 
            {
                ChangePerfectionDialog(e);
            }
        }

        private void ChangeFarmHouseMapData(AssetRequestedEventArgs e, string mapAssetName)
        {
            NPC spouse = Game1.player.getSpouse();
            if (spouse == null || !spouse.name.Equals("Jas"))
            {
                Monitor.Log("You haven't marry Jas yet!", LogLevel.Debug);
                return;
            }

            e.Edit(asset =>
            {
                Monitor.Log("Change farm bed room for Jas.", LogLevel.Debug);
                xTile.Map mapData = Helper.ModContent.Load<xTile.Map>(mapAssetName);
                asset.AsMap().PatchMap(source: mapData, null, null, PatchMapMode.Replace);
            });
        }

        private void ChangeJasDatable(AssetRequestedEventArgs e) 
        {
            e.Edit(asset =>
            {
                Monitor.Log("Change Jas not-datable to datable.");
                var editor = asset.AsDictionary<string, string>();
                editor.Data["Jas"] = editor.Data["Jas"].Replace("not-datable", "datable");
            });
        }

        private void ChangeJasImageAsset(AssetRequestedEventArgs e) 
        {
            e.Edit(asset =>
            {
                Monitor.Log("Change Jas character asset.", LogLevel.Debug);
                IRawTextureData imgData = Helper.ModContent.Load<IRawTextureData>(ASSETS_CHARACTER_JAS);
                if(asset.AsImage().ExtendImage(64, 320) == true)
                {
                    asset.AsImage().PatchImage(imgData, null, null, PatchMode.Replace);
                }
            });
        }

        private void ChangePerfectionDialog(AssetRequestedEventArgs e)
        {
            NPC spouse = Game1.player.getSpouse();
            if (spouse == null || !spouse.name.Equals("Jas"))
            {
                Monitor.Log("You haven't marry Jas yet!", LogLevel.Debug);
                return;
            }

            e.Edit(asset =>
            {
                Monitor.Log("Add perfection event dialog message for Jas.", LogLevel.Debug);
                var editor = asset.AsDictionary<string, string>();

                if (Game1.player.IsMale == true) editor.Data.Add(SUMMIT_EVENT_KEY_JAS, JAS_MESSAGE_STRING_M);
                else editor.Data.Add(SUMMIT_EVENT_KEY_JAS, JAS_MESSAGE_STRING_F);
            });
        }

    }

}
