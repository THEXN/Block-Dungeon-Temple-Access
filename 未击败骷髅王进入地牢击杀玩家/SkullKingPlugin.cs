﻿using Microsoft.Xna.Framework;
using System;
using System.Configuration;
using Terraria;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;
using TShockAPI.Hooks;

namespace SkullKingPlugin
{
    [ApiVersion(2, 1)]
    public class SkullKingPlugin : TerrariaPlugin
    {
        public override string Author => "肝帝熙恩";
        public override string Name => "阻止进入地牢或神庙";
        public override string Description => "阻止玩家在击败骷髅王/世纪之花前进入地牢/神庙";
        public override Version Version => new Version(1, 1, 3);
        public static Configuration Config;
        Color orangeColor = new Color(255, 165, 0);

        public SkullKingPlugin(Main game) : base(game)
        {
            LoadConfig();
        }
        private static void LoadConfig()
        {
            Config = Configuration.Read(Configuration.FilePath);
            Config.Write(Configuration.FilePath);

        }
        private static void ReloadConfig(ReloadEventArgs args)
        {
            LoadConfig();
            args.Player?.SendSuccessMessage("[{0}] 重新加载配置完毕。", typeof(SkullKingPlugin).Name);
        }
        public override void Initialize()
        {
            GeneralHooks.ReloadEvent += ReloadConfig;
            GetDataHandlers.PlayerUpdate.Register(OnPlayerUpdatePhysics);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                GetDataHandlers.PlayerUpdate.UnRegister(OnPlayerUpdatePhysics);
            }
            base.Dispose(disposing);
        }
        private void OnPlayerUpdatePhysics(object? sender, GetDataHandlers.PlayerUpdateEventArgs e)
        {
            CheckPlayerLocation(e.Player);
        }

        private void CheckPlayerLocation(TSPlayer player)
        {
            if (player == null || !player.Active || player.Dead)
                return;

            // 检查是否在地牢环境
            if (IsPlayerInDungeon(player) && !player.HasPermission("skullking.bypass") && Config.PreventPlayersEnterDungeon)
            {
                // 检查骷髅王是否被击败
                if (!NPC.downedBoss3)
                {
                    player.SendMessage("因为在没击败骷髅王的时候探索地牢，你被传送到出生点.", orangeColor);
                    if (Config.TeleportPlayersEnterDungeonForUnkilledSkullKing)
                    {
                        player.Teleport(Main.spawnTileX * 16, Main.spawnTileY * 16);
                        player.TPlayer.ZoneDungeon = false;
                    }
                    if (Config.KillPlayersEnterDungeonForUnkilledSkullKing)
                    {
                        player.KillPlayer();
                        player.SendMessage("因为在没击败骷髅王的时候探索地牢，你被击杀了.", orangeColor);
                        player.TPlayer.ZoneDungeon = false;

                    }
                }
            }
            // 检查是否在神庙环境
            if (IsPlayerInTemple(player) && !player.HasPermission("Plant.bypass") && Config.PreventPlayersEnterTemple)
            {
                // 检查世纪之花是否被击败
                if (!NPC.downedPlantBoss)
                {
                    player.SendMessage("禁止在没击败世纪之花的时候探索神庙，你被传送到出生点", orangeColor);
                    if (Config.TeleportPlayersEnterTempleForUnkilledPlantBoss)
                    {
                        player.Teleport(Main.spawnTileX * 16, Main.spawnTileY * 16);
                        player.TPlayer.ZoneLihzhardTemple = false;
                    }
                    if (Config.KillPlayersEnterTempleForUnkilledPlantBoss)
                    {
                        player.SendMessage("禁止在没击败世纪之花的时候探索神庙，你被击杀", orangeColor);
                        player.KillPlayer();
                        player.TPlayer.ZoneLihzhardTemple = false;
                    }
                }
            }
        }

        private bool IsPlayerInDungeon(TSPlayer player)
        {
            return player.TPlayer.ZoneDungeon;
        }

        private bool IsPlayerInTemple(TSPlayer player)
        {
            // 假设您有与神庙相关的Zone属性，例如 ZoneLihzhardTemple，此处替换为实际属性名
            return player.TPlayer.ZoneLihzhardTemple;
        }
    }
}


