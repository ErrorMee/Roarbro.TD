[返回引导](./ProjectGuide.md)

# 综述
- 游戏的核心是战斗
- 战斗的核心是技能
- 技能的核心是伤害计算
- 伤害的计算是各属性值计算
- 各属性值的计算遵循统一规则

以下反向解析

# 统一规则
### 单属性成长规则
$$ AttrValue = BaseValue + Level * UpValue $$


### 单属性组成规则
$$ AttrValue = BaseValue * (1 + PercentValue) + AddValue $$

# 属性介绍
- ATK: 攻击力
- DEF: 防御力

# 伤害计算

$$ Damage = ATK - DEF $$
$$ ATK = BaseATK * (1 + PercentATK) + AddATK $$
$$ BaseATK = Equip1ATK + Equip2ATK +... $$
$$ EquipATK = EquipBaseATK + EquipLevel * EquipUpATK $$

# 技能
### 枪
Name | ATK | Cooldown 
---|---|---
霰弹枪 | 低 | 慢  | 
冲锋枪  | 很低 | 很快 | 
狙击枪 | 很高 | 很慢  | 
突击步枪 | 高 | 快  | 