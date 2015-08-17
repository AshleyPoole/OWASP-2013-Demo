using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Web;

namespace OWASP_2013_Demo.Authentication
{
	public interface ISessionData
	{
		object Get(string key);

		void Set(string key, object value);

		void Clear();
	}

	public class InMemorySessionData : ISessionData
	{
		private const string COOKIE_NAME = "supersecure-session";
		private static ConcurrentDictionary<string, ConcurrentDictionary<string,object>> sessionStore = new ConcurrentDictionary<string, ConcurrentDictionary<string, object>>();

		private string currentSessionId;

		public InMemorySessionData(HttpContextBase context)
		{
			var sessionId = context.Request[COOKIE_NAME];

			if (sessionId == null || !sessionStore.ContainsKey(sessionId))
			{
				// The session is missing or invalid so we'll make a new session for the user
				sessionId = NewSessionId();

				// store new anonymous session
				sessionStore[sessionId] = new ConcurrentDictionary<string, object>();
			}

			// keep their session going for a couple more hours
			var sessionCookie = new HttpCookie(COOKIE_NAME, sessionId);
			sessionCookie.Expires = DateTime.Now.AddHours(2);
			context.Response.SetCookie(sessionCookie);

			currentSessionId = sessionId;
		}

		public object Get(string key)
		{
			var sessionData = sessionStore[currentSessionId];
			object value;
			sessionData.TryGetValue(key, out value);
			return value;
		}

		public void Set(string key, object value)
		{
			var sessionData = sessionStore[currentSessionId];
			sessionData[key] = value;
		}

		public void Clear()
		{
			sessionStore[currentSessionId] = new ConcurrentDictionary<string, object>();
		}

		private static string NewSessionId()
		{
			// generate a large random Session Id and ensures that it is unused.
			var rng = new RNGCryptoServiceProvider();
			var sessionIdBytes = new byte[96];
			string sessionId;

			do
			{
				rng.GetBytes(sessionIdBytes);
				sessionId = Convert.ToBase64String(sessionIdBytes);
			}
			while (sessionStore.ContainsKey(sessionId));

			return sessionId;
		}
	}
}
