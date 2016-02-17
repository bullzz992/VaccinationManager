using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModelContainer;



namespace DataAccessLayer
{
    public class VaccincationPriceDal
    {
        public bool AddVaccinationPrice(string inputVaccinationDefId, decimal inputAmount)
        {
            try
            {
                
                using (var db = new VaccinationManagerEntities())
                {
                    var obj = db.VaccincationPrices.FirstOrDefault(p => p.VaccinationDefId == inputVaccinationDefId);

                    if (obj == null)
                    {
                        obj = new VaccincationPrice
                        {
                            VaccinationDefId = inputVaccinationDefId,
                            PriceAmount = inputAmount
                        };

                        db.VaccincationPrices.Add(obj);
                        
                    }
                    else
                    {
                        obj.PriceAmount = inputAmount;
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public decimal FindPriceByFaccinationId(string inputVaccinationDefId)
        {
            try
            {
                using (var db = new VaccinationManagerEntities())
                {
                    var obj = db.VaccincationPrices.FirstOrDefault(p => p.VaccinationDefId == inputVaccinationDefId);
                    if (obj == null)
                    {
                        return -1;
                    }
                    else
                    {
                        return obj.PriceAmount;
                    }
                }
            }
            catch (Exception)
            {

                return -1;
            }
        }
    }
}
