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
        public bool AddVaccinationPrice(string inputVaccinationDefId, decimal inputAmount, bool isInitialize = false)
        {
            try
            {
                
                using (var db = new VaccinationManagerEntities())
                {
                    var obj = db.VaccinationPrices.FirstOrDefault(p => p.VaccinationDefId == inputVaccinationDefId);

                    if (obj == null)
                    {
                        obj = new VaccinationPrice
                        {
                            VaccinationDefId = inputVaccinationDefId,
                            PriceAmount = inputAmount
                        };

                        db.VaccinationPrices.Add(obj);
                        
                    }
                    else
                    {
                        if (!isInitialize)
                        {
                            obj.PriceAmount = inputAmount;
                        }
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception exception)
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
                    var obj = db.VaccinationPrices.FirstOrDefault(p => p.VaccinationDefId == inputVaccinationDefId);
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

        public List<VaccinationPrice> GetList()
        {
            using (var db = new VaccinationManagerEntities())
            {
                return db.VaccinationPrices.ToList();
            }
        }
    }
}
