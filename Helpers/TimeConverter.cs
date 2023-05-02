namespace innogotchi_api.Helpers
{
    public static class TimeConverter
    {
        public const int INNOGOTCHI_TIME_MODIFIER = 52;
        public const int DEAD_FEEDING_THRESHOLD = INNOGOTCHI_TIME_MODIFIER * 4;
        public const int HUNGER_FEEDING_THRESHOLD = INNOGOTCHI_TIME_MODIFIER * 3;
        public const int NORMAL_FEEDING_THRESHOLD = INNOGOTCHI_TIME_MODIFIER * 2;
        public const int FULL_FEEDING_THRESHOLD = INNOGOTCHI_TIME_MODIFIER;
        public const int DEAD_QUENCHING_THRESHOLD = INNOGOTCHI_TIME_MODIFIER * 4 / 2;
        public const int HUNGER_QUENCHING_THRESHOLD = INNOGOTCHI_TIME_MODIFIER * 3 / 2;
        public const int NORMAL_QUENCHING_THRESHOLD = INNOGOTCHI_TIME_MODIFIER * 2 / 2;
        public const int FULL_QUENCHING_THRESHOLD = INNOGOTCHI_TIME_MODIFIER / 2;
        private const int MINUTES = 60;
        private const int HOURS = 24;

        /// <summary>
        /// Converts a timespan to innogotchi days using innogotchi time modifier.
        /// </summary>
        public static int ConvertToInnogotchiTime(TimeSpan time)
        {
            return (int)(time.TotalMinutes * INNOGOTCHI_TIME_MODIFIER / MINUTES / HOURS);
        }
    }
}
