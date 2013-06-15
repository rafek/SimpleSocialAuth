using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleSocialAuth.Core.Handlers;

namespace SimpleSocialAuth.Core
{
	/// <summary>
	/// Use to produce one of the supported handlers.
	/// </summary>
	/// <remarks></remarks>
	public class AuthHandlerFactory
	{
		private static readonly Dictionary<string, Func<IAuthenticationHandler>> _handlers =
			new Dictionary<string, Func<IAuthenticationHandler>>();

		static AuthHandlerFactory()
		{
			MapHandlers();
		}

		private static void MapHandlers()
		{
			var handlers = Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(x => typeof (IAuthenticationHandler).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface);
			foreach (var handler in handlers)
			{
				var key = handler.Name.ToLower().Replace("handler", "");
				var handlerType = handler;
				_handlers.Add(key, () => (IAuthenticationHandler) Activator.CreateInstance(handlerType));
			}
		}

		/// <summary>
		/// Map your own authentication handler.
		/// </summary>
		/// <param name="key">Lower case name such as "facebook"</param>
		/// <param name="factoryMethod">Func used to created the handler.</param>
		public static void AddHandler(string key, Func<IAuthenticationHandler> factoryMethod)
		{
			if (key == null) throw new ArgumentNullException("key");
			if (factoryMethod == null) throw new ArgumentNullException("factoryMethod");
			_handlers[key] = factoryMethod;
		}

		/// <summary>
		/// Create a new handler
		/// </summary>
		/// <param name="handler">Lower case name such as "facebook"</param>
		/// <returns>Handler</returns>
		public static IAuthenticationHandler Create(string handler)
		{
			Func<IAuthenticationHandler> factory;
			if (!_handlers.TryGetValue(handler.ToLowerInvariant(), out factory))
			{
				string message = "Unknown authentication handler; valid options = ";
				foreach (var kvp in _handlers)
				{
					message += kvp.Key + ", ";
				}
				throw new ArgumentOutOfRangeException("handler", handler, message);
			}

			return factory();
		}
	}
}