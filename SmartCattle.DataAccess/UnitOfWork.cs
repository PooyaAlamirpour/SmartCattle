//using SmartCattle.DomainClass;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SmartCattle.DataAccess
//{
//    public class UnitOfWork : IDisposable,IUnitOfWork
//    {
//        private readonly IDbContext context;
//        public UnitOfWork(IDbContext context)
//        {
//           this.context=context;
//        }       

//         private GenericRepository<Cattle> cattleRepository;
//         private GenericRepository<CattleActivity> activityRepository;
//         private GenericRepository<CattleActivityState> activityStateRepository;
//         private GenericRepository<CattleCalving> cattleCalvingRepository;
//         private GenericRepository<CattleBreeding> cattleBreedingRepository;
//         private GenericRepository<CattleEatingRumination> eatingRuminationRepository;
//         private GenericRepository<CattleEvent> cattleEventRepository;
//         private GenericRepository<CattleFrtilityState> fertilityStateRepository;
//         private GenericRepository<CattleGroup> cattleGroupRepository;
//         private GenericRepository<CattleHerd> cattleHerdRepository;
//         private GenericRepository<FreeStall> freeStallRepository;
//         private GenericRepository<CattleHealthState> healthStateRepository;
//         private GenericRepository<CattleHeatState> heatStateRepository;
//         private GenericRepository<CattleMilking> milkingRepository;
//         private GenericRepository<CattlePosition> positionRepository; 
//         private GenericRepository<Farm> farmRepository;
//         private GenericRepository<FarmZone> farmZoneRepository;
//         private GenericRepository<HerdEvent> groupEventRepository;
//         private GenericRepository<Sensor> sensorRepository;
//         private GenericRepository<CattleTempreture> tempretureRepository;
//         private GenericRepository<UserSetting> userSettingRepository;
//         private GenericRepository<ApplicationSetting> applicationSettingRepository;
//         private GenericRepository<ACL> aCLRepository;
//         private GenericRepository<CattleTimeBudget> cattleTimeBudgetRepository;
//         private GenericRepository<CattleTimeBudgetItem> cattleTimeBudgetItemRepository;
//         private GenericRepository<CattleTHI> cattleTHIRepository;



//        public GenericRepository<Cattle> CattleRepository
//        {
//            get
//            {
//                if(this.cattleRepository ==null)
//                {
//                    return new GenericRepository<Cattle>(context);
//                }
//                return cattleRepository;
//            }
//        }
//        public GenericRepository<CattleActivity> ActivityRepository
//        {
//            get
//            {
//                if (this.activityRepository == null)
//                {
//                    return new GenericRepository<CattleActivity>(context);
//                }
//                return activityRepository;
//            }
//        }
//        public GenericRepository<CattleActivityState> ActivityStateRepository
//        {
//            get
//            {
//                if (this.activityStateRepository == null)
//                {
//                    return new GenericRepository<CattleActivityState>(context);
//                }
//                return activityStateRepository;
//            }
//        }
//        public GenericRepository<CattleCalving> CattleCalvingRepository
//        {
//            get
//            {
//                if (this.cattleCalvingRepository == null)
//                {
//                    return new GenericRepository<CattleCalving>(context);
//                }
//                return cattleCalvingRepository;
//            }
//        }
//        public GenericRepository<CattleBreeding> CattleBreedingRepository
//        {
//            get
//            {
//                if (this.cattleBreedingRepository == null)
//                {
//                    return new GenericRepository<CattleBreeding>(context);
//                }
//                return cattleBreedingRepository;
//            }
//        }
//        public GenericRepository<CattleEatingRumination> EatingRuminationRepository
//        {
//            get
//            {
//                if (this.eatingRuminationRepository == null)
//                {
//                    return new GenericRepository<CattleEatingRumination>(context);
//                }
//                return eatingRuminationRepository;
//            }
//        }
//        public GenericRepository<CattleEvent> CattleEventRepository
//        {
//            get
//            {
//                if (this.cattleEventRepository == null)
//                {
//                    return new GenericRepository<CattleEvent>(context);
//                }
//                return cattleEventRepository;
//            }
//        }
//        public GenericRepository<CattleFrtilityState> FertilityStateRepository
//        {
//            get
//            {
//                if (this.fertilityStateRepository == null)
//                {
//                    return new GenericRepository<CattleFrtilityState>(context);
//                }
//                return fertilityStateRepository;
//            }
//        }
//        public GenericRepository<CattleGroup> CattleGroupRepository
//        {
//            get
//            {
//                if (this.cattleGroupRepository == null)
//                {
//                    return new GenericRepository<CattleGroup>(context);
//                }
//                return cattleGroupRepository;
//            }
//        }
//        public GenericRepository<CattleHerd> CattleHerdRepository
//        {
//            get
//            {
//                if (this.cattleHerdRepository == null)
//                {
//                    return new GenericRepository<CattleHerd>(context);
//                }
//                return cattleHerdRepository;
//            }
//        }
//        public GenericRepository<CattleHealthState> HealthStateRepository
//        {
//            get
//            {
//                if (this.healthStateRepository == null)
//                {
//                    return new GenericRepository<CattleHealthState>(context);
//                }
//                return healthStateRepository;
//            }
//        }
//        public GenericRepository<CattleHeatState> HeatStateRepository
//        {
//            get
//            {
//                if (this.heatStateRepository == null)
//                {
//                    return new GenericRepository<CattleHeatState>(context);
//                }
//                return heatStateRepository;
//            }
//        }
//        public GenericRepository<CattleMilking> MilkingRepository
//        {
//            get
//            {
//                if (this.milkingRepository == null)
//                {
//                    return new GenericRepository<CattleMilking>(context);
//                }
//                return milkingRepository;
//            }

//        }
//        public GenericRepository<CattlePosition> PositionRepository
//        {
//            get
//            {
//                if (this.positionRepository == null)
//                {
//                    return new GenericRepository<CattlePosition>(context);
//                }
//                return positionRepository;
//            }

//        } 
//        public GenericRepository<Farm> FarmRepository
//        {
//            get
//            {
//                if (this.farmRepository == null)
//                {
//                    return new GenericRepository<Farm>(context);
//                }
//                return farmRepository;
//            }

//        }
//        public GenericRepository<FarmZone> FarmZoneRepository
//        {
//            get
//            {
//                if (this.farmZoneRepository == null)
//                {
//                    return new GenericRepository<FarmZone>(context);
//                }
//                return farmZoneRepository;
//            }

//        }
//        public GenericRepository<HerdEvent> GroupEventRepository
//        {
//            get
//            {
//                if (this.groupEventRepository == null)
//                {
//                    return new GenericRepository<HerdEvent>(context);
//                }
//                return groupEventRepository;
//            }

//        }
//        public GenericRepository<FreeStall> FreeStallRepository
//        {
//            get
//            {
//                if (this.freeStallRepository == null)
//                {
//                    return new GenericRepository<FreeStall>(context);
//                }
//                return freeStallRepository;
//            }

//        }
//        public GenericRepository<Sensor> SensorRepository
//        {
//            get
//            {
//                if (this.sensorRepository == null)
//                {
//                    return new GenericRepository<Sensor>(context);
//                }
//                return sensorRepository;
//            }

//        }
//        public GenericRepository<CattleTempreture> TempretureRepository
//        {
//            get
//            {
//                if (this.tempretureRepository == null)
//                {
//                    return new GenericRepository<CattleTempreture>(context);
//                }
//                return tempretureRepository;
//            }

//        }
//        public GenericRepository<CattleTimeBudget> CattleTimeBudgetRepository
//        {
//            get
//            {
//                if (this.cattleTimeBudgetRepository == null)
//                {
//                    return new GenericRepository<CattleTimeBudget>(context);
//                }
//                return cattleTimeBudgetRepository;
//            }

//        }
//        public GenericRepository<CattleTimeBudgetItem> CattleTimeBudgetItemRepository
//        {
//            get
//            {
//                if (this.cattleTimeBudgetItemRepository == null)
//                {
//                    return new GenericRepository<CattleTimeBudgetItem>(context);
//                }
//                return cattleTimeBudgetItemRepository;
//            }

//        }
//        public GenericRepository<ApplicationSetting> ApplicationSettingRepository
//        {
//            get
//            {
//                if (this.applicationSettingRepository == null)
//                {
//                    return new GenericRepository<ApplicationSetting>(context);
//                }
//                return applicationSettingRepository;
//            }

//        }
//        public GenericRepository<UserSetting> UserSettingRepository
//        {
//            get
//            {
//                if (this.userSettingRepository == null)
//                {
//                    return new GenericRepository<UserSetting>(context);
//                }
//                return userSettingRepository;
//            }

//        }
//        public GenericRepository<ACL> ACLRepository
//        {
//            get
//            {
//                if (this.aCLRepository == null)
//                {
//                    return new GenericRepository<ACL>(context);
//                }
//                return aCLRepository;
//            }
//        }
//        public GenericRepository<CattleTHI> CattleTHIRepository
//        {
//            get
//            {
//                if (this.cattleTHIRepository == null)
//                {
//                    return new GenericRepository<CattleTHI>(context);
//                }
//                return cattleTHIRepository;
//            }
//        }

//        public int Save()
//        {
//           return context.saveChanges();
//        }

//        private bool disposed = false;

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!this.disposed)
//            {
//                if (disposing)
//                {
//                    context.Dispose();
//                }
//            }
//            this.disposed = true;
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }
//    }
//}
