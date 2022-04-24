﻿using Lambda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Global;

namespace ConfigObjective
{
    public partial class ToolControl
    {
        Global.Mode.Config.MoveStep MoveStep = WindowData.GetInstance().Stage.MoveStep;
        Global.Mode.Config.Stage Stage = WindowData.GetInstance().Stage;    
        private void Stage_Initialized()
        {
            if (Stage == null)
                Stage = new Global.Mode.Config.Stage();
            ToggleButtonXYF.IsChecked = Stage.MoveStep.XStep == 200;
            ToggleButtonZF.IsChecked = Stage.MoveStep.ZStep == 200;
        }

        private void Stage_Reset_Click(object sender, RoutedEventArgs e)
        {
            LambdaControl.Trigger("STAGE_SETTING_RESET", this, new Dictionary<string, object> { });
        }

        private void ToggleButtonXYF_Checked(object sender, RoutedEventArgs e)
        {
            MoveStep.XStep = 200;
            MoveStep.YStep = 200;
        }

        private void ToggleButtonXYF_Unchecked(object sender, RoutedEventArgs e)
        {
            MoveStep.XStep = 1000;
            MoveStep.YStep = 1000;
        }

        private void ToggleButtonZF_Checked(object sender, RoutedEventArgs e)
        {
            MoveStep.ZStep = 200;

        }

        private void ToggleButtonZF_Unchecked(object sender, RoutedEventArgs e)
        {
            MoveStep.ZStep = 1000;
        }

        private void ButtonLeft_Click(object sender, RoutedEventArgs e)
        {
            LambdaControl.Trigger("STAGE_MOVE_LEFT", this, new Dictionary<string, object> { { "step", MoveStep.XStep }, { "direction", 0 } });
        }

        private void ButtonRight_Click(object sender, RoutedEventArgs e)
        {
            LambdaControl.Trigger("STAGE_MOVE_RIGHT", this, new Dictionary<string, object> { { "step", MoveStep.XStep }, { "direction", 1 } });
        }

        private void ButtonFront_Click(object sender, RoutedEventArgs e)
        {
            LambdaControl.Trigger("STAGE_MOVE_FRONT", this, new Dictionary<string, object> { { "step", MoveStep.YStep }, { "direction", 2 } });
        }

        private void ButtonRear_Click(object sender, RoutedEventArgs e)
        {
            LambdaControl.Trigger("STAGE_MOVE_REAR", this, new Dictionary<string, object> { { "step", MoveStep.YStep }, { "direction", 3 } });
        }

        private void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            LambdaControl.Trigger("STAGE_MOVE_DOWN", this, new Dictionary<string, object> { { "step", MoveStep.ZStep }, { "direction", 4 } });
        }

        private void ButtonDown_Click(object sender, RoutedEventArgs e)
        {
            LambdaControl.Trigger("STAGE_MOVE_DOWN", this, new Dictionary<string, object> { { "step", MoveStep.ZStep }, { "direction", 5 } });
        }

        private void ButtonRe_Click(object sender, RoutedEventArgs e)
        {
            LambdaControl.Trigger("STAGE_MOVE_CENTRE", this, new Dictionary<string, object> { { "step", 0 }, { "direction", 6 } });
        }

        private void ButtonAutoFocus_Click(object sender, RoutedEventArgs e)
        {
            LambdaControl.Trigger("STAGE_AUTO_FOCUS", this, new Dictionary<string, object> { { "mode", (bool)ToggleButtonZF.IsChecked ? 0 : 1 } });
        }


    }
}