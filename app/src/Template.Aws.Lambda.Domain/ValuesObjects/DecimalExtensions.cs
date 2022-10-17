using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Template.Aws.Lambda.Domain.ValuesObjects
{
    public static class DecimalExtensions
    {
        public const string DefaultFormatNullSymbol = "-";
        private static readonly IFormatProvider provider =
            NumberProvider.Provider;

        #region Format
        public static string ToPercentage(this decimal? number) =>
            number.ToFormat("P2");

        public static string ToAbsolutePercentage(this decimal? number) =>
            number.ToAbsolute().ToPercentage();

        public static string ToCurrency(this decimal? number) =>
            ToFormat(number, "C2");

        public static string ToAbsoluteCurrency(this decimal number) =>
            ((decimal?)number).ToAbsoluteCurrency();

        public static string ToAbsoluteCurrency(this decimal? number) =>
            number.ToAbsolute().ToCurrency();

        public static string ToFormat(this decimal? number, string format) =>
            number.HasValue ?
                number.Value.ToString(format, provider) :
                DefaultFormatNullSymbol;

        public static decimal? ToRoundDefault(this decimal? number) =>
            number.HasValue ?
                number.Value.ToRoundDefault() :
                number;

        public static decimal ToRoundDefault(this decimal number) =>
            Math.Round(number, 2);
        #endregion

        #region Acessibility

        public static string ToAcessibilityCurrency(this decimal number)
        {
            const string nonNegativeAcessibilityTemplate = "{0:0} {1} e {2:00} {3}";
            const string negativeAcessibilityTemplate = "menos " + nonNegativeAcessibilityTemplate;
            const string singleIntegerToken = "real";
            const string pluralIntegerToken = "reais";
            const string singleDecimalToken = "centavo";
            const string pluralDecimalToken = "centavos";

            var template = number < decimal.Zero ?
                negativeAcessibilityTemplate :
                nonNegativeAcessibilityTemplate;

            number = Math.Abs(number);

            var decimalVal = (long)(number % 1 * 100);
            var integerVal = (long)(number - (number % 1));

            var decimalToken = decimalVal > 1 ?
                pluralDecimalToken : singleDecimalToken;

            var integerToken = integerVal > 1 ?
                pluralIntegerToken : singleIntegerToken;

            return string.Format(template, 
                integerVal, integerToken,
                decimalVal, decimalToken);
        }
        public static string ToAcessibilityPercentage(this decimal number)
        {
            const string percentageTemplate = "de {0:P2}";
            const string positiveAcessibilityTemplate = "positiva " + percentageTemplate;
            const string negativeAcessibilitryTemplate = "negativa " + percentageTemplate;

            var template = number switch
            {
                var n when n > decimal.Zero => positiveAcessibilityTemplate,
                var n when n < decimal.Zero => negativeAcessibilitryTemplate,
                _ => percentageTemplate
            };

            return string.Format(provider,
                template, number.ToAbsolute());
        }

        public static string ToAcessibilityCurrency(this decimal? number)
        {
            const string defaultEmptyToken = "indisponível";

            return number.HasValue ?
                number.Value.ToAcessibilityCurrency() :
                defaultEmptyToken;
        }

        public static string ToAcessibilityPercentage(this decimal? number)
        {
            const string defaultEmptyToken = "indisponível";

            return number.HasValue ?
                number.Value.ToAcessibilityPercentage() :
                defaultEmptyToken;
        }

        #endregion

        public static decimal? ToAbsolute(this decimal? number) =>
            number.HasValue ?
                Math.Abs(number.Value) : number;

        public static decimal ToAbsolute(this decimal number) =>
            Math.Abs(number);
    }

    public class NumberProvider
    {
        public static IFormatProvider Provider { get; } =
            InitializeNumberFormat();

        private static IFormatProvider InitializeNumberFormat()
        {
            var numberFormat = NumberFormatInfo.GetInstance(
                new CultureInfo("pt-BR"));

            numberFormat.CurrencyPositivePattern = 2;
            numberFormat.CurrencyNegativePattern = 12;

            return NumberFormatInfo.ReadOnly(numberFormat);
        }
    }
}
