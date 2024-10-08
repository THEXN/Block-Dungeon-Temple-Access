# 本仓库已不再更新，已经整合到：https://github.com/UnrealMultiple/TShockPlugin，后续更新均会发布至此仓库
# This repository is no longer updated and has been integrated into: https://github.com/UnrealMultiple/TShockPlugin. All future updates will be published there.
 # SkullKingPlugin：阻止玩家进入地牢或神庙

## 简介

**SkullKingPlugin** 是一个TShock插件，旨在防止玩家在击败骷髅王或世纪之花之前进入地牢或神庙，以维护游戏的平衡和挑战性。

## 功能

- **地牢和神庙限制**：插件会阻止玩家在未击败骷髅王或世纪之花的情况下进入地牢或神庙。
- **传送与击杀**：对于违规玩家，插件可以选择将其传送回出生点或直接击杀。
- **权限控制**：管理员可以通过设置特定权限来允许某些玩家绕过这些限制。

## 权限介绍

SkullKingPlugin 提供了两个主要的权限设置：

1. `skullking.bypass`：拥有此权限的玩家可以无视地牢进入限制，即使在未击败骷髅王的情况下也能自由进入地牢。
2. `Plant.bypass`：拥有此权限的玩家可以无视神庙进入限制，即使在未击败世纪之花的情况下也能自由进入神庙。

## 支持与反馈
- 如果您在使用过程中遇到问题或有任何建议，欢迎在官方论坛或社区中提出issues或pr。
- github仓库：https://github.com/THEXN/Block-Dungeon-Temple-Access
