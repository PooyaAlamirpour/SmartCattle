using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCattle.DomainClass
{
    
    //public enum FertilityState
    //{
    //      AbortedOn ,
    //      DoNotBreedScince ,
    //      DryOffSince ,
    //      EmbryoImplantedOn ,
    //      ETFlushedOn ,
    //      FreshSince ,
    //      InHeatOn ,
    //      InseminationOn ,
    //      openScince,
    //      PregnancyCheckInDoubtOn ,
    //      PregnancyCheckNegativeOn ,
    //      PregnantSince
    //}

    public enum CattleEvent_Category
    {
        Select,
        Heat_Event,
        HealthState,
        Fertility_Status,
        Vet,
        Assign_Herd,
        Assign_Group,
        Assign_Freestall
    }

    public enum Heat_Event_Subcategory
    {
        Select,
        Suspicious,
        Heat,
        Normal
    }

    public enum HealthState_Subcategory
    {
        Select,
        Suspicious,
        Health,
        Sick
    }

    public enum Fertility_Status_Subcategory
    {
        Select,
        Open,
        Insemination,
        Pregnant
    }

    public enum Vet_Subcategory
    {
        Select,
        Diagnosis,
        Examination,
        Treatment,
        Insemination,
        Abortion,
        Calving,
        Pregnancy
    }

    public enum Sex_Subcategory
    {
        Sex,
        Male,
        Female
    }

    public enum HeatState
    {
          Potential,
          Suspicious,
          InHeat
    }

    public enum BehaviorState
    {
        sitting,
        standing,
        walking,
        eating,
        rumination,
        drinking,
        nonActive
    }


    public enum HealthState
    {
          Suspicious ,
          Sick ,
          VerySick ,
          NoMovement 
    }
     
    public enum EventSate
    {
          Calving ,
          Open ,
          Heat ,
          HormoneTreatment ,
          Insemination ,
          ETFlushed ,
          PregnancyCheck ,
          DryOff ,
          DoNotBreed 
    }
    
    public enum FertilityStates
    {
        Calf,
        BClf,
        Heifer,
        Fresh,
        Early,
        Abort,
        Ready,
        Open,
        Insem,
        Preg,
        Preg2,
        Dry,
        Lead,
        Other,
        TBCul,
        Cull,
        Treated,
        DryPreg,
        DryNonPreg,
        InsemPreg,
        InsemNonPreg,
        ReadyNonPreg,
        ReadyPreg,
        NotReadyPreg,
        NotReadyNotPreg

    }
    
     
}
