using System;
using System.Collections.Generic;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;

namespace SimpleSocialAuth.MVC3.Consumers
{
  internal class InMemoryTokenManager : IConsumerTokenManager
  {
    private readonly Dictionary<string, string> _tokensAndSecrets = new Dictionary<string, string>();

    public InMemoryTokenManager(string consumerKey, string consumerSecret)
    {
      if (string.IsNullOrEmpty(consumerKey))
      {
        throw new ArgumentNullException("consumerKey");
      }

      if (string.IsNullOrEmpty(consumerSecret))
      {
        throw new ArgumentNullException("consumerSecret");
      }

      ConsumerKey = consumerKey;
      ConsumerSecret = consumerSecret;
    }

    public string ConsumerKey { get; private set; }

    public string ConsumerSecret { get; private set; }

    #region ITokenManager Members

    public string GetTokenSecret(string token)
    {
      return _tokensAndSecrets[token];
    }

    public void StoreNewRequestToken(UnauthorizedTokenRequest request, ITokenSecretContainingMessage response)
    {
      _tokensAndSecrets[response.Token] = response.TokenSecret;
    }

    public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey, string requestToken, string accessToken, string accessTokenSecret)
    {
      _tokensAndSecrets.Remove(requestToken);
      _tokensAndSecrets[accessToken] = accessTokenSecret;
    }

    public TokenType GetTokenType(string token)
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
