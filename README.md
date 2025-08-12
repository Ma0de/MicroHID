# MicroHID
![License:AGPLv3](https://img.shields.io/badge/License-AGPLv3-blue.svg)
![SCP:SL](https://img.shields.io/badge/game-SCP:%20Secret%20Laboratory-red)
![Author](https://img.shields.io/badge/author-%E7%8C%AB%E5%BE%B7-brightgreen)
为《SCP:秘密实验室》服务器添加 **MicroH.I.D** 的额外充电方式，不再依赖 SCP-914 加工

---

## 📌 功能特性
- **背包充电**：玩家手持或背包中的 MicroH.I.D 会缓慢自动充电（默认 **5秒充1%**）。
> ⚠️ **注意**：当前版本充电功能时间固定，配置系统将在后续更新中推出。
---

## ⚙️ 安装指南
0. 首先确认你的EXILED版本，该插件使用EXILED 9.7.2版本，你可以试试看低版本EXILED是否能够正常运行该插件，或者下载源代码自行使用其他版本的EXILED修改并编译，但您需要遵守**📜 许可证与使用限制**。[2025年8月不懂怎么给原版服务器安装EXILED？点我](https://lcabk.cn/archives/1748518448591)
1. 从[Release页面](https://github.com/Ma0de/MicroHID/releases)下载最新版本
2. 将插件 DLL 文件放入服务器EXILED的 `plugins` 文件夹。
3. 如果服务器已在运行，重启服务器或使用 `reload plugins` 命令加载。未运行服务器直接运行即可自动加载

---

## 🚧 计划更新
- **自定义充电速度**：通过配置文件调整充电速率。
- **手持/背包模式切换**：可选是否必须手持武器才能充电。
- **更多充电触发条件**：如靠近特定 SCP 或设施区域时加速充电。

---

## 📜 许可证与使用限制
本插件基于 **GNU AGPLv3 许可证**发布，并附加以下条款：
> 本附加条款是对AGPLv3的补充而非替代
### 您必须遵守：
1. **署名要求**  
   任何公开的分发或修改版本必须在：
   - 代码文件头部保留原始版权声明
   - 插件描述中注明：`基于猫德(Ma0de)的MicroHID插件开发`  
   - 包含指向原仓库的链接：https://github.com/Ma0de/MicroHID

2. **商业限制**  
   - 允许用于商业运营的SCP:SL服务器  
   - 禁止直接出售插件本体或修改版本（包括但不限于插件商店、付费模组平台等）  
   - 禁止通过本插件功能向玩家收取额外费用（如"快速充电VIP服务"等），每一个玩家都应该享有免费充电服务的权利


完整法律文本请参阅 [LICENSE](LICENSE) 文件。


---
