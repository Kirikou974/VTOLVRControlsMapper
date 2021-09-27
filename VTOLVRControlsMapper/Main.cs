using SharpDX.DirectInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VTOLVRControlsMapper
{
    public class Main : VTOLMOD
    {
        static readonly string _settingsFileFolder = @"VTOLVR_ModLoader\Mods\";
        static bool _updateControllers;
        public override void ModLoaded()
        {
            Log("Mod Loaded");
            StartCoroutine(ControlsHelper.LoadDeviceInstances());
            if (VTOLAPI.SceneLoaded == null)
            {
                VTOLAPI.SceneLoaded += Sceneloaded;
            }
            if (VTOLAPI.MissionReloaded == null)
            {
                VTOLAPI.MissionReloaded += MissionReloaded;
            }
            base.ModLoaded();
        }
        private void Sceneloaded(VTOLScenes scene)
        {
            switch (scene)
            {
                case VTOLScenes.ReadyRoom:
                    break;
                case VTOLScenes.Akutan:
                case VTOLScenes.CustomMapBase:
                    Log("Scene Loaded");
                    StartCoroutine(LoadModObjects());
                    break;
                case VTOLScenes.LoadingScene:
                    break;
            }
        }
        private void MissionReloaded()
        {
            StartCoroutine(LoadModObjects());
        }
        public void OnApplicationFocus(bool hasFocus)
        {
            Log("Game focus is : " + hasFocus);
            _updateControllers = hasFocus;
        }
        /// <summary>
        /// Called by Unity each frame
        /// </summary
        public void Update()
        {
            VTOLVehicles currentVehicle = VTOLAPI.GetPlayersVehicleEnum();
            if (Input.GetKeyDown(KeyCode.F8))
            {
                Log("Recreating mappings file");
                string filePath = ControlsHelper.GetMappingFilePath(_settingsFileFolder, name, currentVehicle.ToString());
                ControlsHelper.CreateMappingFile(filePath);
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Log("Reloading mappings");
                StartCoroutine(LoadModObjects());
            }
            if (ControlsHelper.MappingsLoaded && _updateControllers)
            {
                StartCoroutine(ControlsHelper.UpdateControllers());
            }
        }
        private IEnumerator LoadModObjects()
        {
            VTOLVehicles vehicle = VTOLAPI.GetPlayersVehicleEnum();
            Log("Controls loading for " + vehicle);
            while (!ControlsHelper.UnityObjectsLoaded(vehicle))
            {
                ControlsHelper.LoadUnityObjects();
                yield return new WaitForSeconds(2);
            }
            Log("Controls loaded for " + vehicle);

            Log("Mapping loading for " + vehicle);
            ControlsHelper.LoadMappings(_settingsFileFolder, name, vehicle.ToString());
            Log("Mapping loaded for " + vehicle);

            Log("Loading keyboard");
            ControlsHelper.LoadControllers<Keyboard>();
            Log("Loading joysticks");
            ControlsHelper.LoadControllers<Joystick>();
            Log("Controllers loaded");
            
            Log("Loading hands");
            while (!ControlsHelper.HandsLoaded)
            {
                ControlsHelper.LoadHands();
                yield return new WaitForSeconds(2);
            }
            Log("Hands loaded");

            yield return null;
        }
    }
}