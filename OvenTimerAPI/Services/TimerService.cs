using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OvenTimerAPI.Services
{
    public class TimerService : BaseService
    {        
        public TimerService(TimerContext context) : base(context)
        { }

        public void AddTimeSets(TimeSetViewModel timeSetVM) 
        {   
            _ctx.Database.ExecuteSqlCommand("DELETE FROM TimeSetDefaults");
            _ctx.Database.ExecuteSqlCommand("DELETE FROM TimeSets");
            
            _ctx.TimeSetDefaults.Add(timeSetVM.Defaults);
            _ctx.TimeSets.AddRange(timeSetVM.TimeSets);
            _ctx.SaveChanges();
        }

        public TimeSet GetTimeSet(string id) 
        {
            return _ctx.TimeSets.FirstOrDefault(t => t.Id == id);
        }

        public TimeSetViewModel GetTimeSets() 
        {
            var timeSetVM = new TimeSetViewModel() 
            {
                Defaults = _ctx.TimeSetDefaults.FirstOrDefault(),
                TimeSets = _ctx.TimeSets.ToList()
            };

            return timeSetVM;
        }       

        public void UpdateTimeSet(TimeSet timeSet) 
        {
            var t = _ctx.TimeSets.Update(timeSet);
            
            _ctx.SaveChanges();
        } 
    }
}