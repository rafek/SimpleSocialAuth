using System;

namespace SimpleSocialAuth.Core
{
    public interface IHttpRequest
    {
        Uri Url { get; }
    }
}