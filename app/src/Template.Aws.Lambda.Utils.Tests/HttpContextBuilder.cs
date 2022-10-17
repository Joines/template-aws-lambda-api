using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.IO;

namespace Template.Aws.Lambda.Utils.Tests
{
    public class HttpContextBuilder
    {
        private HttpContextBuilder() { }

        public static HttpContext Build<TException>(
            TException exception = null)
            where TException : Exception, new()
        {
            exception ??= new TException();

            var exceptionFeature = new ExceptionHandlerFeature()
            {
                Error = exception,
                Path = "api/v1/teste"
            };

            var feature = new FeatureCollection();

            feature.Set<IExceptionHandlerPathFeature>(
                exceptionFeature);

            feature.Set<IHttpResponseFeature>(
                new HttpResponseFeature());

            //feature.Set<IHttpResponseBodyFeature>(
            //    new StreamResponseBodyFeature(Stream.Null);

            return new DefaultHttpContext(feature);
        }
    }
}
