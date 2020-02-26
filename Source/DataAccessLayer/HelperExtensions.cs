namespace GisApi.DataAccessLayer
{
    using System;

    public static class HelperExtensions
    {
        public static T If<T>(this T builder, bool condition, Func<T, T> action)
        {
            if (condition)
            {
                return action(builder);
            }

            return builder;
        }
    }
}