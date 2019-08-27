using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private ResSvc resSvc;
    private TimerSvc timerSvc;

    public void InitManager()
    {
        timerSvc = TimerSvc.Instance;
        resSvc = ResSvc.Instance;
        CommonTool.Log("Init SkillManager Done");
    }

    public void SkillAttack(EntityBase entity, int skillID)
    {
        AttackDamage(entity, skillID);
        AttackEffect(entity, skillID);
    }

    public void SkillAction(EntityBase caster, SkillCfg skillCfg, SkillActionCfg sac, int index)
    {
        var monsters = caster.BattleManager.GetEntityMonsters();
        int damage = skillCfg.SkillDamageLst[index];
        monsters.ForEach(target =>
        {
            if (IsInRange(caster.GetPos(), target.GetPos(), sac.Radius) &&
                IsInAngle(caster.GetTrans(), target.GetPos(), sac.Angel))
            {
                CalcDamage(caster, target, damage, skillCfg.DmgType);
            }
        });
    }

    System.Random rd = new System.Random();

    private void CalcDamage(EntityBase caster, EntityBase target, int damage, DamageType dt)
    {
        int damageSum = damage;
        if (dt == DamageType.AD)
        {
            {
                //闪避
                int dodgeNum = ZCTools.RDInt(1, 100, rd);
                if (dodgeNum <= target.BattleProps.Dodge)
                {
                    //     GameRoot.AddTips(target.Controller.name + "闪避了你的攻击"); //todo转移到聊天窗口
                    CommonTool.Log("闪避Rate:" + dodgeNum + "/" + target.BattleProps.Dodge);
                    target.SetDodge();
                    return;
                }
            }
            damageSum += caster.BattleProps.PA;
            {
                //暴击
                int criticalNum = ZCTools.RDInt(1, 100, rd);
                if (criticalNum <= caster.BattleProps.Critical)
                {
                    float criticalIncRate =(1 + ZCTools.RDInt(1, 100, rd) / 100.0f);
                    CommonTool.Log("暴击ratr"+criticalIncRate);
                    damageSum =  (int)(criticalIncRate * damageSum);
                    target.SetCritical(damageSum);
                }
            }
            {
                //穿甲
                int pd = (int) ((1 - caster.BattleProps.Pierce / 100.0f)*target.BattleProps.PD);
                damageSum -= pd;
                CommonTool.Log("对"+target.Name+"造成了"+damageSum+"点物理伤害");
            }
        }
        else if (dt == DamageType.AP)
        {
            damageSum += caster.BattleProps.SA;
            damageSum -= target.BattleProps.SD;
        }
        if (damageSum <= 0)
        {
            return;
        }
        if (target.HP <= damageSum)
        {
            target.HP = 0;
            target.BattleManager.RemoveMonster(target.Name);
        }
        else
        {
            target.HP -= damageSum;
            target.Hit();
        }
        target.SetHurt(damageSum);
    }

    private bool IsInRange(Vector3 from, Vector3 to, float range)
    {
        float dis = Vector3.Distance(from, to);
        return dis <= range;
    }

    private bool IsInAngle(Transform trans, Vector3 to, float angle)
    {
        if (angle == 360) return true;
        Vector3 start = trans.forward;
        Vector3 dir = (to - trans.position);
        float ang = Vector3.Angle(start, dir);
        return ang <= angle / 2;
    }

    private void AttackDamage(EntityBase entity, int skillID)
    {
        SkillCfg skillCfg = resSvc.GetSkillCfg(skillID);
        List<int> actionIDs = skillCfg.SkillActionLst;
        int sum = 0;
        for (int i = 0; i < actionIDs.Count; i++)
        {
            SkillActionCfg sac = resSvc.GetSkillActionCfg(actionIDs[i]);
            sum += sac.DelayTime;
            if (sum > 0)
            {
                var i1 = i;
                timerSvc.AddTimeTask(tid => { SkillAction(entity, skillCfg, sac, i1); }, sum);
            }
            else
            {
                SkillAction(entity, skillCfg, sac, i);
            }
        }
    }

    private void AttackEffect(EntityBase entity, int skillID)
    {
        SkillCfg skillCfg = resSvc.GetSkillCfg(skillID);
        
        
        if (entity.GetDirInput()==Vector2.zero)
        {
            Vector2 dir = entity.CalcTargetDir();
            if (dir != Vector2.zero)
            {
                entity.SetAtkRotation(dir);
            }
        }
        else
        {
            entity.SetAtkRotation(entity.GetDirInput(),true);
        }

        entity.SetAciton(skillCfg.AniAction);
        entity.SetFX(skillCfg.FX, skillCfg.Duration);
        skillCfg.SkillMoveLst.ForEach(sid => { CalcSkillMove(entity, sid); });
        entity.canControl = false;
        entity.SetDir(Vector2.zero);
        timerSvc.AddTimeTask(tid => entity.Idle(), skillCfg.Duration);
    }

    private void CalcSkillMove(EntityBase entity, int sid)
    {
        int sum = 0;

        SKillMoveCfg sKillMoveCfg = resSvc.GetSkillMoveCfg(sid);
        float speed = sKillMoveCfg.MoveDis / (sKillMoveCfg.MoveTime / 1000f);
        sum += sKillMoveCfg.DelayTime;
        if (sum > 0)
        {
            timerSvc.AddTimeTask(tid => { entity.SetSkillMove(true, speed); }, sum);
        }
        else
        {
            entity.SetSkillMove(true, speed);
        }
        sum += sKillMoveCfg.MoveTime;
        timerSvc.AddTimeTask(tid => { entity.SetSkillMove(false); }, sum);
    }
}