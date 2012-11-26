using System;
using System.Globalization;
using SimpleSocialAuth.Core.Consumers;

namespace SimpleSocialAuth.Core.Handlers
{
    /// <summary>
    /// Twitter authentication handler.
    /// </summary>
    /// <remarks>Required that the following appSettings keys are configured: "twitterConsumerKey" and "twitterConsumerSecret"</remarks>
    public class TwitterHandler : IAuthenticationHandler
    {
        #region IAuthenticationHandler Members

        public string PrepareAuthRequest(PrepareAuthenticationContext context)
        {
            var callback =
                new Uri(Utils.GetUrlBase(context.Request) + context.RedirectPath);

            var consumer = new TwitterConsumer(context.SessionStorage);
            return consumer
                .StartSignInWithTwitter(callback)
                .Headers["Location"];
        }

        public BasicUserData ProcessAuthRequest(ProcessAuthenticationContext context)
        {
            string screenName;
            int userId;

            var consumer = new TwitterConsumer(context.SessionStorage);
            return
                consumer.TryFinishSignInWithTwitter(out screenName, out userId)
                    ? new BasicUserData
                        {
                            UserId = userId.ToString(CultureInfo.InvariantCulture),
                            UserName = screenName,
                            PictureUrl =
                                string.Format("http://api.twitter.com/1/users/profile_image/{0}.png", screenName)
                        }
                    : null;
        }

        #endregion
    }
}