using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable MemberCanBePrivate.Global

namespace ModuleLightBulb
{
    public class ModuleLightBulb : ModuleLight
    {
        [KSPField(guiActive = true, guiName = "#autoLOC_6001402", isPersistant = true)]
        [UI_FloatRange(maxValue = 1, minValue = 0, scene = UI_Scene.Flight, stepIncrement = 0.01f)]
        protected float red = 0.72f;

        [KSPField(guiActive = true, guiName = "#autoLOC_6001403", isPersistant = true)]
        [UI_FloatRange(maxValue = 1, minValue = 0, scene = UI_Scene.Flight, stepIncrement = 0.01f)]
        protected float green = 0.35f;

        [KSPField(guiActive = true, guiName = "#autoLOC_6001404", isPersistant = true)]
        [UI_FloatRange(maxValue = 1, minValue = 0, scene = UI_Scene.Flight, stepIncrement = 0.01f)]
        protected float blue = 0.0f;

        [KSPField(guiActive = true, guiName = "Emissive Multiplier", isPersistant = true, guiActiveEditor = true)]
        [UI_FloatRange(maxValue = 3, minValue = 0, stepIncrement = 0.01f)]
        protected float emissiveMultipier = 1.73f;

        [KSPField(guiActive = true, guiName = "Emissive Brightness", isPersistant = true, guiActiveEditor = true)]
        [UI_FloatRange(stepIncrement = 0.01f, maxValue = 1f, minValue = 0f)]
        public float lensBrightness = 0.0f;
        
        public override void OnInitialize()
        {
            base.OnInitialize();
            lightR = red;
            lightG = green;
            lightB = blue;
            UpdateLightTextureColor();
        }

        public override void OnAwake()
        {
            base.OnAwake();
            Fields["lensBrightness"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["emissiveMultipier"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["green"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["blue"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["red"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["lightG"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["lightB"].OnValueModified += (x => UpdateLightTextureColor());
            Fields["lightR"].OnValueModified += (x => UpdateLightTextureColor());
        }

        public virtual void UpdateLightTextureColor()
        {
            if (HighLogic.LoadedSceneIsEditor)
            {
                red = lightR;
                green = lightG;
                blue = lightB;
            }
            part.FindModelComponents<Light>().ToList().ForEach(r =>
            {
                r.color = new Color(red, green, blue, 1);
            });
            part.FindModelComponents<Renderer>()
                .Where(r => r.material.HasProperty("_EmissiveColor"))
                .ToList()
                .ForEach(r => r.material.SetColor("_EmissiveColor", GetEmissiveTextureColor()));
            if (HighLogic.LoadedSceneIsEditor)
            {
                emissiveMultipier += 0.01f;
                emissiveMultipier -= 0.01f;
            }
        }

        protected virtual Color GetEmissiveTextureColor()
        {
            return new Color((red * (1.0f - lensBrightness) + lensBrightness) * emissiveMultipier,
                (green * (1.0f - lensBrightness) + lensBrightness) * emissiveMultipier,
                (blue * (1.0f - lensBrightness) + lensBrightness) * emissiveMultipier);
        }
    }
}