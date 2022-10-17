using System;
using System.Globalization;

namespace Template.Aws.Lambda.Domain.ValuesObjects
{
    public static class DateTimeProvider
    {
        public static IFormatProvider Provider { get; } =
            InitializeDateTimeFormat();

        private static IFormatProvider InitializeDateTimeFormat()
        {
            var formatInfo = DateTimeFormatInfo
                .GetInstance(new CultureInfo("pt-BR"));

            return DateTimeFormatInfo.ReadOnly(formatInfo);
        }
    }
}
