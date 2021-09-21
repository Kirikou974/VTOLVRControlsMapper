﻿using System;
using System.Collections.Generic;
using Valve.Newtonsoft.Json;

namespace VTOLVRControlsMapper.Core
{
    public enum ControllerActionType
    {
        Button,
        Axis,
        Touchpad
    }
    public class GameAction
    {
        public ControllerActionBehavior ControllerActionBehavior { get; set; }
        public string ControllerActionName { get; set; }
        public Guid ControllerInstanceGuid { get; set; }
        public GameAction(Guid controllerInstanceGuid, string controllerActionName, ControllerActionBehavior controllerActionBehavior)
        {
            ControllerInstanceGuid = controllerInstanceGuid;
            ControllerActionBehavior = controllerActionBehavior;
            ControllerActionName = controllerActionName;
        }
    }
    public class ControlMapping
    {
        public string GameControlName { get; set; }
        public List<GameAction> KeyboardActions { get; set; }
        public List<GameAction> JoystickActions { get; set; }
        public List<Type> Types { get; set; }
        public bool HasCover { get; set; }
        public string CoverName { get; set; }
        [JsonConstructor]
        public ControlMapping(string gameControlName, List<Type> types, bool hasCover, string coverName)
        {
            GameControlName = gameControlName;
            HasCover = hasCover;
            Types = types;
            CoverName = coverName;
            //KeyboardActions = new List<GameAction>();
            //DirectInput di = new DirectInput();
            //Keyboard kb = new Keyboard(di);
            //KeyboardActions.Add(new GameAction(kb.Information.InstanceGuid, "a", ControllerActionBehavior.Toggle));
        }
        public ControlMapping(string gameControlName, List<Type> types, string coverName) : this(gameControlName, types, true, coverName) { }
        public ControlMapping(string gameControlName, List<Type> types) : this(gameControlName, types, false, string.Empty) { }
    }
}
