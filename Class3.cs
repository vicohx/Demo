using CandidateContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace CandidateImplement
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Candidate : ICandidate
    {
        private readonly log4net.ILog logger;

        public Candidate()
        {
            logger = log4net.LogManager.GetLogger("FileAppender");
        }

        public bool Active(int idEmp, int idEval)
        {
            using (EntitiesDB entities = new EntitiesDB())
            {
                Tbls emp = entities.Tbls.Where((w) => w.idEmp == idEmp).FirstOrDefault();

                if (emp.cipher != null)
                {
                    if (entities.Tbls2.Where(w => w.idEval == idEval && w.PerReg == emp.cipher).Count() > 0)
                        return true;
                }

                return false;
            }
        }
    }
}
