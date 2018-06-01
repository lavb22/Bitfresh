namespace Bitfresh_Core
{
    public static class Constants
    {
        //Basic limitations

        public const int MaxDays = 28;

        public const int MinFrec = 1;
        public const int MaxFrec = 48;

        public const uint MinAge = 1;
        public const int MaxAge = 27;

        //Time units

        public const int second = 1000;
        public const int minute = 60 * second;
        public const int hour = 60 * minute;
        public const int day = 24 * hour;
    }
}