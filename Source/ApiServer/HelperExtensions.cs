namespace GisApi.ApiServer
{
    using System;
    using Microsoft.AspNetCore.Hosting;

    public static class HelperExtensions
    {
        const string DEVELOPMENT = "Development";

        public static T If<T>(this T builder, bool condition, Func<T, T> action)
        {
            if (condition)
            {
                return action(builder);
            }

            return builder;
        }

        public static bool IsDevelopment(this IWebHostEnvironment env) =>
            DEVELOPMENT.Equals(env.EnvironmentName, StringComparison.InvariantCultureIgnoreCase);
    }
}