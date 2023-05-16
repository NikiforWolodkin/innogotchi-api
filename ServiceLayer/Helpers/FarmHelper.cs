using DataLayer.Models;

namespace BusinessLayer.Helpers
{
    public static class FarmHelper
    {
        public static int GetFarmAlivePetsCount(Farm farm)
        {
            return farm.Innogotchis
                .Where(inno => inno.DeathDate == null)
                .Count();
        }

        public static int GetFarmDeadPetsCount(Farm farm)
        {
            return farm.Innogotchis
                .Where(inno => inno.DeathDate != null)
                .Count();
        }

        public static int GetFarmAverageHappyDaysAmount(Farm farm)
        {
            if (farm.Innogotchis.Count() == 0)
            {
                return 0;
            }

            return (int)farm.Innogotchis
                .Select(InnogotchiHelper.GetInnogotchiHappinessDayCount)
                .Average();
        }

        public static int GetFarmAverageFeedingPeriod(Farm farm)
        {
            if (farm.Innogotchis.Count() == 0)
            {
                return 0;
            }

            return (int)farm.Innogotchis
                .Select(InnogotchiHelper.GetInnogotchiAverageFeedingPeriod)
                .Average();
        }

        public static int GetFarmAverageQuenchingPeriod(Farm farm)
        {
            if (farm.Innogotchis.Count() == 0)
            {
                return 0;
            }

            return (int)farm.Innogotchis
                .Select(InnogotchiHelper.GetInnogotchiAverageQuenchingPeriod)
                .Average();
        }
    }
}
