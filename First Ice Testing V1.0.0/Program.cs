using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
    //FROM HERE
        IMyPistonBase Piston;
        IMyMotorStator Rotor;
        IMyTextPanel LCD;
        IMyCargoContainer Cargo;
        IMyShipDrill Drill;
        List<IMyTerminalBlock> Drills = new List<IMyTerminalBlock>();
        IMyInteriorLight OnLight;


        public Program()
        {
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
            Piston = GridTerminalSystem.GetBlockWithName("Ice Piston") as IMyPistonBase;
            Rotor = GridTerminalSystem.GetBlockWithName("Ice Rotor") as IMyMotorStator;
            LCD = GridTerminalSystem.GetBlockWithName("LCD Panel") as IMyTextPanel;
            Cargo = GridTerminalSystem.GetBlockWithName("Ice Cargo") as IMyCargoContainer;
            OnLight = GridTerminalSystem.GetBlockWithName("Ice Light") as IMyInteriorLight;
            
            IMyBlockGroup DrillGroup = GridTerminalSystem.GetBlockGroupWithName("Ice Drills");
            DrillGroup.GetBlocks(Drills);
        }
        bool FirstBoot = true;
        
        public void Main(string argument, UpdateType updateSource)
        {
            if (OnLight.Enabled == false | Cargo.GetInventory(0).IsFull == true)
            {
                Rotor.TargetVelocityRPM = 0;
                for (int i = 0; i < Drills.Count(); i++)
                {
                    Drill = (IMyShipDrill)Drills[i];
                    Drill.ApplyAction("OnOff_Off");
                    LCD.WriteText("Full and turning off");
                }
                OnLight.Enabled = false;
                FirstBoot = true;
            }
            else
            {
                if (FirstBoot == true)
                {
                    Rotor.TargetVelocityRPM = 2;
                    for (int i = 0; i < Drills.Count(); i++)
                    {
                        Drill = (IMyShipDrill)Drills[i];
                        Drill.ApplyAction("OnOff_On");
                    }
                    FirstBoot = false;
                }
                Rotor.UpperLimitDeg = 360;
                LCD.WriteText(Rotor.Angle.ToString());
                if ((float)Rotor.Angle == (float)(2 * Math.PI))
                {
                    Piston.MaxLimit += (float)0.1;
                    Rotor.UpperLimitDeg = 361;
                    LCD.WriteText("0.1 Deeper");
                }
            }

            
        }
    
    //TO HERE
    }
}
