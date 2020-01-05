 
   
public class StateDie:IState
    {
        public void Enter(EntityBase entity, params object[] args)
        {
            entity.CurrentAniState = AniState.Die;
            CommonTool.Log("en Die"+entity.EntityType);

            
        }

        public void Process(EntityBase entity, params object[] args)
        {
            
                entity.SetAciton(Constans.ActionDie);
                TimerSvc.Instance.AddTimeTask(tid => { entity.SetActive(false); },
                    Constans.DieAniLength);
            CommonTool.Log("pr Die");

        }

        public void Exit(EntityBase entity, params object[] args)
        {
            CommonTool.Log("ex Die");

        }
    }
