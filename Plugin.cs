/*
 * MicroHID Charging Plugin for SCP: Secret Laboratory
 * Copyright (C) 2025 猫德(Ma0de) - https://github.com/Ma0de/MicroHID
 * 
 * 功能：为MicroH.I.D提供自动充电功能
 * 框架：EXILED 9.7.2
 * 版本：v1.0.1
 * 
 * 许可证：GNU AGPLv3 with additional restrictions
 * 禁止商业出售及付费功能分级
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using Exiled.API.Features;
using MEC;
using InventorySystem.Items.MicroHID;
using InventorySystem.Items.MicroHID.Modules;

namespace MicroHID
{
    public class Config : Exiled.API.Interfaces.IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public float ChargeInterval { get; set; } = 5f; // 充电间隔(秒)
        public float ChargeAmount { get; set; } = 0.01f; // 每次充电量(1%)
    }

    public class Plugin : Plugin<Config>
    {
        public override string Name => "MicroHID";
        public override string Author => "By Maode";
        public override string Prefix => "MicroHID";
        public override Version Version => new Version(1, 0, 1); // 更新版本号
        public override Version RequiredExiledVersion => new Version(9, 7, 2);

        private CoroutineHandle _checkHandle;
        private HashSet<MicroHIDItem> _processedHIDs = new HashSet<MicroHIDItem>(); 

        public override void OnEnabled()
        {
            _processedHIDs.Clear();
            _checkHandle = Timing.RunCoroutine(CheckLoop());
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Timing.KillCoroutines(_checkHandle);
            _processedHIDs.Clear();
            base.OnDisabled();
        }

        private IEnumerator<float> CheckLoop()
        {
            PropertyInfo energyProp = typeof(FiringModeControllerModule)
                .GetProperty("Energy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            FieldInfo energyField = typeof(FiringModeControllerModule)
                .GetField("_energy", BindingFlags.Instance | BindingFlags.NonPublic);

            while (true)
            {
                yield return Timing.WaitForSeconds(Config.ChargeInterval);

                if (Config.Debug)
                    Log.Info("开始扫描所有 MicroHID");

                _processedHIDs.Clear();

                foreach (var hid in UnityEngine.Object.FindObjectsOfType<MicroHIDItem>())
                {
                    if (_processedHIDs.Contains(hid))
                        continue;

                    _processedHIDs.Add(hid);

                    try
                    {
                        var ccField = hid.GetType()
                            .GetField("<CycleController>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (ccField == null)
                        {
                            if (Config.Debug)
                                Log.Warn($"[MicroHID] {hid.name} 找不到 CycleController 字段，跳过");
                            continue;
                        }

                        var cycleController = ccField.GetValue(hid);
                        if (cycleController == null)
                        {
                            if (Config.Debug)
                                Log.Warn($"[MicroHID] {hid.name} CycleController 为空，跳过");
                            continue;
                        }

                        var fmcField = cycleController.GetType()
                            .GetField("_firingModeControllers", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (fmcField == null)
                        {
                            if (Config.Debug)
                                Log.Warn($"[MicroHID] {hid.name} 找不到 firingModeControllers 字段，跳过");
                            continue;
                        }

                        var fmcList = fmcField.GetValue(cycleController) as List<FiringModeControllerModule>;
                        if (fmcList == null || fmcList.Count == 0)
                        {
                            if (Config.Debug)
                                Log.Warn($"[MicroHID] {hid.name} firingModeControllers 列表为空，跳过");
                            continue;
                        }

                        var fmc = fmcList[0];
                        if (fmc == null)
                            continue;

                        float? currentEnergy = null;

                        if (energyProp != null)
                        {
                            var val = energyProp.GetValue(fmc);
                            if (val != null)
                                currentEnergy = (float)val;
                        }

                        if (currentEnergy == null && energyField != null)
                        {
                            var val = energyField.GetValue(fmc);
                            if (val != null)
                                currentEnergy = (float)val;
                        }

                        if (currentEnergy == null)
                            continue;

                        int percentBefore = (int)(currentEnergy.Value * 100);
                        string location = hid.PickupDropModel != null ? "掉落在地图" : "玩家持有";

                        if (currentEnergy.Value < 1f)
                        {
                            float newEnergy = Math.Min(1f, currentEnergy.Value + Config.ChargeAmount);
                            if (energyProp != null)
                                energyProp.SetValue(fmc, newEnergy);
                            else if (energyField != null)
                                energyField.SetValue(fmc, newEnergy);

                            if (Config.Debug)
                                Log.Info($"[MicroHID] {location} - 电量: {percentBefore}% → {(int)(newEnergy * 100)}% (+{(int)(Config.ChargeAmount * 100)}%)");
                        }
                        else if (Config.Debug)
                        {
                            Log.Info($"[MicroHID] {location} - 电量已满 ({percentBefore}%)");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"读取 MicroHID 能量时出错: {ex}");
                    }
                }
            }
        }
    }
}