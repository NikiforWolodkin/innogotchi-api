using BusinessLayer.Enums;
using DataLayer.Models;
using InnogotchiApi.Helpers;

namespace BusinessLayer.Helpers
{
    public static class InnogotchiHelper
    {
        public static bool IsInnogotchiDead(Innogotchi innogotchi)
        {
            if (innogotchi.DeathDate is not null)
            {
                return true;
            }

            FeedingAndQuenching innogotchiFeedingAndQuenchingTime =
                innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .First();

            DateTime lastFeedingTime = innogotchiFeedingAndQuenchingTime.FeedingTime;
            DateTime lastQuenchingTime = innogotchiFeedingAndQuenchingTime.QuenchingTime;

            int feedingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastFeedingTime);
            int quenchingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastQuenchingTime);

            if (feedingTimespan < TimeConverter.DEAD_FEEDING_THRESHOLD
                && quenchingTimespan < TimeConverter.DEAD_QUENCHING_THRESHOLD)
            {
                return false;
            }

            if (quenchingTimespan > feedingTimespan)
            {
                innogotchi.DeathDate = lastQuenchingTime
                    .AddDays(TimeConverter.DEAD_QUENCHING_THRESHOLD / TimeConverter.INNOGOTCHI_TIME_MODIFIER);
            }
            else
            {
                innogotchi.DeathDate = lastFeedingTime
                    .AddDays(TimeConverter.DEAD_FEEDING_THRESHOLD / TimeConverter.INNOGOTCHI_TIME_MODIFIER);
            }

            return true;
        }

        public static DateTime CalculateInnogotchiDeathDate(Innogotchi innogotchi)
        {
            FeedingAndQuenching innogotchiFeedingAndQuenchingTime =
                innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .First();

            DateTime lastFeedingTime = innogotchiFeedingAndQuenchingTime.FeedingTime;
            DateTime lastQuenchingTime = innogotchiFeedingAndQuenchingTime.QuenchingTime;

            int feedingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastFeedingTime);
            int quenchingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastQuenchingTime);

            if (quenchingTimespan > feedingTimespan)
            {
                return lastQuenchingTime
                    .AddDays(TimeConverter.DEAD_QUENCHING_THRESHOLD / TimeConverter.INNOGOTCHI_TIME_MODIFIER);
            }
            else
            {
                return lastFeedingTime
                    .AddDays(TimeConverter.DEAD_FEEDING_THRESHOLD / TimeConverter.INNOGOTCHI_TIME_MODIFIER);
            }
        }

        public static InnogotchiHungerLevel GetInnogotchiHungerLevel(Innogotchi innogotchi)
        {
            FeedingAndQuenching innogotchiFeedingAndQuenchingTime =
                innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .First();

            DateTime lastFeedingTime = innogotchiFeedingAndQuenchingTime.FeedingTime;

            int feedingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastFeedingTime);

            switch (feedingTimespan)
            {
                case < TimeConverter.NORMAL_FEEDING_THRESHOLD:
                    return InnogotchiHungerLevel.Full;
                case < TimeConverter.HUNGER_FEEDING_THRESHOLD:
                    return InnogotchiHungerLevel.Normal;
                case < TimeConverter.DEAD_FEEDING_THRESHOLD:
                    return InnogotchiHungerLevel.Hungry;
                default:
                    return InnogotchiHungerLevel.Dead;
            }
        }

        public static InnogotchiThirstLevel GetInnogotchiThirstLevel(Innogotchi innogotchi)
        {
            FeedingAndQuenching innogotchiFeedingAndQuenchingTime =
                innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .First();

            DateTime lastQuenchingTime = innogotchiFeedingAndQuenchingTime.QuenchingTime;

            int quenchingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastQuenchingTime);

            switch (quenchingTimespan)
            {
                case < TimeConverter.NORMAL_QUENCHING_THRESHOLD:
                    return InnogotchiThirstLevel.Full;
                case < TimeConverter.HUNGER_QUENCHING_THRESHOLD:
                    return InnogotchiThirstLevel.Normal;
                case < TimeConverter.DEAD_QUENCHING_THRESHOLD:
                    return InnogotchiThirstLevel.Thirsty;
                default:
                    return InnogotchiThirstLevel.Dead;
            }
        }

        public static int GetInnogotchiAge(Innogotchi innogotchi)
        {
            if (innogotchi.DeathDate is not null)
            {
                return TimeConverter.ConvertToInnogotchiTime((DateTime)innogotchi.DeathDate - innogotchi.CreationDate);
            }

            return TimeConverter.ConvertToInnogotchiTime(DateTime.Now - innogotchi.CreationDate);
        }

        public static int GetInnogotchiHappinessDayCount(Innogotchi innogotchi)
        {
            IList<FeedingAndQuenching> feedingsAndQuenchings = innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .ToList();

            DateTime lastFeedingTime = feedingsAndQuenchings.First().FeedingTime;
            DateTime lastQuenchingTime = feedingsAndQuenchings.First().QuenchingTime;

            int feedingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastFeedingTime);
            int quenchingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastQuenchingTime);

            int happyDays = 0;

            if (feedingTimespan >= TimeConverter.NORMAL_FEEDING_THRESHOLD)
            {
                feedingTimespan = feedingTimespan - TimeConverter.NORMAL_FEEDING_THRESHOLD;
            }
            else
            {
                feedingTimespan = 0;
            }
            if (quenchingTimespan >= TimeConverter.NORMAL_QUENCHING_THRESHOLD)
            {
                quenchingTimespan = quenchingTimespan - TimeConverter.NORMAL_QUENCHING_THRESHOLD;
            }
            else
            {
                quenchingTimespan = 0;
            }

            happyDays += (GetInnogotchiAge(innogotchi)
                - (feedingTimespan > quenchingTimespan ? feedingTimespan : quenchingTimespan))
                - (int)feedingsAndQuenchings.Select(f => f.UnhappyDays).Sum();

            return happyDays;
        }

        public static int GetInnogotchiAverageFeedingPeriod(Innogotchi innogotchi)
        {
            IList<FeedingAndQuenching> feedingsAndQuenchings = innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .ToList();

            if (innogotchi.FeedingAndQuenchings.Count > 1)
            {
                feedingsAndQuenchings = feedingsAndQuenchings
                    .Take(innogotchi.FeedingAndQuenchings.Count - 1)
                    .ToList();
            }

            DateTime lastFeedingTime = feedingsAndQuenchings.First().FeedingTime;

            int feedingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastFeedingTime);

            IList<int> feedingPeriods = feedingsAndQuenchings.Select(f => (int)f.FeedingPeriod).ToList();

            feedingPeriods.Add(feedingTimespan);

            return (int)feedingPeriods.Average();
        }

        public static int GetInnogotchiAverageQuenchingPeriod(Innogotchi innogotchi)
        {
            IList<FeedingAndQuenching> feedingsAndQuenchings = innogotchi.FeedingAndQuenchings
                .OrderByDescending(f => f.Id)
                .ToList();

            if (innogotchi.FeedingAndQuenchings.Count > 1)
            {
                feedingsAndQuenchings = feedingsAndQuenchings
                    .Take(innogotchi.FeedingAndQuenchings.Count - 1)
                    .ToList();
            }

            DateTime lastQuenchingTime = feedingsAndQuenchings.First().QuenchingTime;

            int quenchingTimespan = TimeConverter.ConvertToInnogotchiTime(DateTime.Now - lastQuenchingTime);

            IList<int> quenchingPeriods = feedingsAndQuenchings.Select(f => (int)f.FeedingPeriod).ToList();

            quenchingPeriods.Add(quenchingTimespan);

            return (int)quenchingPeriods.Average();
        }
    }
}
