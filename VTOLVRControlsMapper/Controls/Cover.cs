﻿using System;
using System.Collections;
using VTOLVRControlsMapper.Core;

namespace VTOLVRControlsMapper.Controls
{
    [ControlClass(UnityTypes = new Type[] { typeof(VRInteractable), typeof(VRSwitchCover), typeof(VRLever) })]
    public class Cover : ControlToggleBase<VRSwitchCover>
    {
        public Lever Lever
        {
            get;
            protected set;
        }
        public Cover(string coverName) : base(coverName) {
            Lever = new Lever(ControlName);
            //This is a fix because covers are considered as opened by default when the game loads
            UnityControl.OnSetState(1);
        }
        public override IEnumerator Toggle()
        {
            yield return Lever.Toggle();
        }
        public override void StartControlInteraction()
        {
            Lever.StartControlInteraction();
        }
        public override void StopControlInteraction()
        {
            Lever.StopControlInteraction();
        }
    }
}
