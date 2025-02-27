using UnityEngine;
using BepInEx;
using HarmonyLib;
using Sonigon;
using ClassesManagerReborn;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using ClassesManagerReborn.Util;
using UnboundLib;
using ModdingUtils.Utils;
using BepInEx.Configuration;
using UnboundLib.Utils.UI;

namespace Shade
{

    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("Root.Gun.bodyRecoil.Patch", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.rounds.willuwontu.gunchargepatch", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("root.classes.manager.reborn", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, modName, version)]
    [BepInProcess("Rounds.exe")]
    public class ShadeCards : BaseUnityPlugin
    {
        internal const string ModId = "shade.plugin.trinkets";
        internal const string modName = "Shade's Trinkets";
        internal const string version = "0.1.6";

        internal static string modInitials = "ST";
        internal static AssetBundle assets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("shadecards", typeof(ShadeCards).Assembly);

        public static ConfigEntry<bool> DEBUG;
        //internal static bool debug = true;
        // Start is called before the first frame update
        void Start()
        {
            Unbound.RegisterMenu(modName, delegate () { }, new Action<GameObject>(this.NewGUI), null, false);
        }

        void Awake()
        {
            DEBUG = base.Config.Bind<bool>(ModId, "Debug", false, "Enable to turn on console info from this mod.");

            assets.LoadAsset<GameObject>("_ModCards").GetComponent<CardHolder>().RegisterCards();
            //load the classes separately for organization
            assets.LoadAsset<GameObject>("_SC_ClassCards").GetComponent<CardHolder>().RegisterCards();
            new Harmony("Shade's Trinkets").PatchAll();
        }

        // Update is called once per frame
        void Update()
        {
             
        }

        public static void playSoundFromAsset(string AssetName, Transform where, float volume = .3f)
        {
            SoundEvent sound = null;
            SoundParameterIntensity soundParameterIntensity = new SoundParameterIntensity(volume);
            SoundParameterPitchRatio soundParameterPitchRatio = new SoundParameterPitchRatio(.9f + UnityEngine.Random.Range(0f, .2f));
            AudioClip audioClip = assets.LoadAsset<AudioClip>(AssetName);
            SoundContainer soundContainer = ScriptableObject.CreateInstance<SoundContainer>();
            soundContainer.audioClip[0] = audioClip;
            soundContainer.setting.volumeIntensityEnable = true;
            sound = ScriptableObject.CreateInstance<SoundEvent>();
            sound.soundContainerArray[0] = soundContainer;

            // add an option to lower the sounds?
            // soundParameterIntensity.intensity = base.transform.localScale.x * Optionshandler.vol_Master * Optionshandler.vol_Sfx / 1.2f * CR.globalVolMute.Value;
            SoundManager.Instance.Play(sound, where, new Sonigon.Internal.SoundParameterBase[]
            {
                soundParameterIntensity
            });
        }
    
        private void NewGUI(GameObject menu)
        {
            MenuHandler.CreateText(modName, menu, out _, 60, false, null, null, null, null);

            //example from class manager reborn

            //MenuHandler.CreateToggle(Force_Class.Value, "Enable Force Classes", menu, value => Force_Class.Value = value);
            //MenuHandler.CreateToggle(Ignore_Blacklist.Value, "Allow more then one class and subclass per player", menu, value => Ignore_Blacklist.Value = value);
            //MenuHandler.CreateToggle(Ensure_Class_Card.Value, "Guarantee players in a class will draw a card for that class if able", menu, value => Ensure_Class_Card.Value = value);
            //MenuHandler.CreateToggle(Class_War.Value, "Prevent players from having the same class", menu, value => Class_War.Value = value);
            //MenuHandler.CreateSlider("Increase the chances of a class restricted card to show up (Intended for large packs)", menu, 30, 1, 100, Class_Odds.Value, value => Class_Odds.Value = value, out UnityEngine.UI.Slider _);


            //MenuHandler.CreateText("", menu, out _);
            MenuHandler.CreateToggle(DEBUG.Value, "Debug Mode", menu, value => DEBUG.Value = value, 50, false, Color.red, null, null, null);
        }
    }

    public static class Debug
    {
        public static void Log(object msg, bool important = false)
        {
            if (important || ShadeCards.DEBUG.Value) UnityEngine.Debug.Log($"{ShadeCards.ModId}=>{msg}");
        }
    }
}