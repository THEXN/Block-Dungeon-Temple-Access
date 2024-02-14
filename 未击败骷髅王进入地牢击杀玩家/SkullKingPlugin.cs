using Microsoft.Xna.Framework;
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
        public override Version Version => new Version(1, 1, 2);
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
            ServerApi.Hooks.GameUpdate.Register(this, OnUpdate);
        }

        protected override void Dispose(bool disposing)
        {
            GeneralHooks.ReloadEvent += ReloadConfig;
            if (disposing)
            {
                ServerApi.Hooks.GameUpdate.Deregister(this, OnUpdate);
            }

            base.Dispose(disposing);
        }

        private bool IsDungeonGuardianAlive()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.type == NPCID.DungeonGuardian)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnUpdate(EventArgs args)
        {
            foreach (TSPlayer player in TShock.Players)
            {
                if (player == null || !player.Active)
                    continue;

                // 如果玩家已死亡，跳过该玩家
                if (player.Dead)
                    continue;
                // 检查是否在地牢环境
                if (IsPlayerInDungeon(player) && !player.HasPermission("skullking.bypass") && Config.阻止玩家进入地牢总开关)
                {
                    // 检查骷髅王是否被击败
                    if (!NPC.downedBoss3 && IsDungeonGuardianAlive())
                    {
                        // 在这里执行击杀玩家的逻辑
                        if(Config.传送未击败骷髅王进入地牢的玩家)
                        {
                            player.SendMessage("因为在没击败骷髅王的时候探索地牢，你被传送到出生点.", orangeColor);
                            player.Teleport(Main.spawnTileX * 16, Main.spawnTileY * 16);
                        }

                        if (Config.击杀未击败骷髅王进入地牢的玩家)
                        {
                            player.KillPlayer();
                        }
                        player.SendMessage("因为在没击败骷髅王的时候探索地牢，你被击杀了.", orangeColor);

                    }
                }
                // 检查是否在神庙环境
                if (IsPlayerInTemple(player) && !player.HasPermission("Plant.bypass") && Config.阻止玩家进入神庙总开关)
                {
                    // 检查世纪之花是否被击败
                    if (!NPC.downedPlantBoss)
                    {
                        if (Config.传送未击败世纪之花进入神庙的玩家)
                        {
                            player.SendMessage("禁止在没击败世纪之花的时候探索神庙，你被传送到出生点" , orangeColor);
                            player.Teleport(Main.spawnTileX * 16, Main.spawnTileY * 16);
                        }
                        if (Config.击杀未击败世纪之花进入神庙的玩家)
                        {
                            player.SendMessage("禁止在没击败世纪之花的时候探索神庙，你被击杀", orangeColor);
                            player.KillPlayer();
                        }
                    }
                }
            }
        }


        private bool IsPlayerInDungeon(TSPlayer player)
        {
            int tileX = (int)player.TileX;
            int tileY = (int)player.TileY;

            // 定义地牢墙壁的类型数组
            int[] dungeonWallTypes = { 7, 8, 9, 94, 98, 96, 95, 99, 97 };

            // 检查玩家是否在地牢墙壁内
            for (int x = tileX - 2; x <= tileX + 2; x++)
            {
                for (int y = tileY - 2; y <= tileY + 2; y++)
                {
                    if (Array.Exists(dungeonWallTypes, wallType => Main.tile[x, y].wall == wallType))
                    {
                        // 检查玩家是否在地下
                        if ((int)player.TileY > (int)(Main.worldSurface))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        private bool IsPlayerInTemple(TSPlayer player)
        {
            int tileX = (int)player.TileX;
            int tileY = (int)player.TileY;

            // 定义神庙墙壁的类型数组
            int[] templeWallTypes = { 87 };

            // 记录符合条件的神庙墙壁数量
            int templeWallCount = 0;

            // 检查玩家2x2范围内的Tile
            for (int x = tileX - 1; x <= tileX + 1; x++)
            {
                for (int y = tileY - 1; y <= tileY + 1; y++)
                {
                    // 检查当前Tile是否是指定的神庙墙壁类型
                    if (Array.Exists(templeWallTypes, wallType => Main.tile[x, y].wall == wallType))
                    {
                        // 符合条件的神庙墙壁数量加一
                        templeWallCount++;
                    }
                }
            }

            // 判断符合条件的神庙墙壁数量是否不超过4块
            return templeWallCount >= 4;
        }

    }
}


