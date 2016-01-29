using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace VaccinationManager.DAL
{
    public class VaccinationConfiguration : DbConfiguration
    {
        public VaccinationConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}