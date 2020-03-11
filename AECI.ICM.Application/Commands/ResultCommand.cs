using AECI.ICM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AECI.ICM.Application.Commands
{
    public static class ResultCommand
    {
        public static class V1
        {
            public class Load
            {
                public int Id { get; set; }
                public string Branch { get; set; }
                public DateTime Date { get; set; }
                public string Month { get; set; }
                public string BMName { get; set; }
                public string FinName { get; set; }             
                public DateTime DateSigned { get; set; }
                public int GMAuthorisedStatus { get; set; }
                public int BMAuthorisedStatus { get; set; }
                public string Region { get; set; }
            }

            public class Update
            {
                public int Id { get; set; }
                public string Branch { get; set; }
                public DateTime Date { get; set; }
                public string Month { get; set; }
                public string BMName { get; set; }
                public string FinName { get; set; }
                public DateTime DateSigned { get; set; }
                public int GMAuthorisedStatus { get; set; }
                public int BMAuthorisedStatus { get; set; }
                public string Region { get; set; }
            }
        }
        public static List<V1.Load> MapFrom(List<ResultEntity> unmapped)
        {
            var mapped = new List<V1.Load>();
            unmapped.ForEach(p =>
            {
                var r = new V1.Load();
                r.BMAuthorisedStatus = p.BMAuthorisedStatus;
                r.BMName = p.BMName;
                r.Branch = p.Branch;
                r.Date = p.Date;
                r.DateSigned = p.DateSigned;
                r.FinName = p.FinName;
                r.GMAuthorisedStatus = p.GMAuthorisedStatus;
                r.Id = p.Id;
                r.Month = p.Month;
                r.Region = p.Region;

                mapped.Add(r);
            });

            return mapped.ToList();
        }

        public static ResultEntity MapTo(V1.Update unmapped)
        {
            var r = new ResultEntity();
            r.BMAuthorisedStatus = unmapped.BMAuthorisedStatus;
            r.BMName = unmapped.BMName;
            r.Branch = unmapped.Branch;
            r.Date = unmapped.Date;
            r.DateSigned = unmapped.DateSigned;
            r.FinName = unmapped.FinName;
            r.GMAuthorisedStatus = unmapped.GMAuthorisedStatus;
            r.Id = unmapped.Id;
            r.Month = unmapped.Month;
            r.Region = unmapped.Region;

            return r;
        }
    }
}
