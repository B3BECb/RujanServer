using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using RujanService.Framework;

namespace RujanService
{
	/// <summary>Представляет Rujan сервер.</summary>
	public sealed class RujanHttpServer : IDisposable
	{
		#region Static

		private static string SitePath { get; } = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "sites";

		#endregion
		
		#region Data

		private CancellationTokenSource _cts;

		private ServerSettings _settings;

		private HttpListener _listener;

		private List<ISite> _sites;

		#endregion
		
		#region .ctor

		public RujanHttpServer(ServerSettings settings)
		{
			_settings = settings;
			_listener = new HttpListener();
			
			Utilities.CreateDirectory(SitePath);
		}

		#endregion

		#region Public methods

		/// <summary>Асинхронно запускает <see cref="HttpListener"/>.</summary>
		/// <returns>Задание. <seealso cref="Task"/></returns>
		public Task StartAsync()
		{
			_listener?.Prefixes.Clear();
			_sites?.Clear();

			_cts?.Cancel();
			_cts?.Dispose();
			_cts = new CancellationTokenSource();

			_listener.Prefixes.Add($"{_settings.Address}:{_settings.Port}/");

			InitializeSites(SitePath);

			_listener.Start();

			Console.WriteLine("Server started.");

			return Task.Run(() => HttpWatcher());
		}

		/// <summary>Останавливает <see cref="HttpListener"/>.</summary>
		public void Stop()
		{
			_cts.Cancel();
			_listener.Stop();
			Console.WriteLine("Server stoped.");
		}

		#endregion

		#region Private methods

		private void HttpWatcher()
		{
			while(!_cts.IsCancellationRequested)
			{
				if(_listener.Prefixes.Count == 0)
				{
					Console.WriteLine("Не зарегистрировано ни одного сайта!");
					Stop();
					break;
				}

				var context = _listener.GetContext();

				try
				{
					if(context.Request.Url.AbsolutePath == "/")
					{
						context.Response.Redirect($"{_settings.Address}:{_settings.Port}/localhost/");
						Console.WriteLine($"Redirect to {_settings.Address}:{_settings.Port}/localhost/");
						using(Stream stream = context.Response.OutputStream) { }
						continue;
					}

					if(Path.HasExtension(context.Request.Url.AbsolutePath))
					{
						GetFile(context);
					}
					else
					{						
						GetPageContent(context);
					}
				}
				catch(Exception ex)
				{
					byte[] buffer = Encoding.UTF8.GetBytes(ex.ToString());
					Response(context, buffer, HttpStatusCode.InternalServerError);

					Console.WriteLine($"Response status code:	{context.Response.StatusCode}");									
				}
			}
		}

		private void GetPageContent(HttpListenerContext context)
		{
			string path = context.Request.Url.AbsolutePath;
			if(context.Request.Url.Segments.LastOrDefault().IndexOf('/') == -1)
				path = context.Request.Url.AbsolutePath + "/";

			var targetSite = _sites.Where(site => site.Address.ToLower() == path.ToLower());
			var FoundedSitesCount = targetSite.Count();
			
			using(var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
			{
				if(FoundedSitesCount == 0)
				{
					Response(context, HttpStatusCode.NotFound);
				}
				else if(targetSite.Count() == 1)
				{
					var site = targetSite.FirstOrDefault();
											
					var requestBody = reader.ReadToEnd();

					Console.WriteLine($"{context.Request.HttpMethod}:	{context.Request.Url}");

					var siteResponse = site.GetResponse(_cts.Token, requestBody, context.Request.Url.Query);

					byte[] buffer = Encoding.UTF8.GetBytes(siteResponse.ResponseContent);
					Response(context, buffer, HttpStatusCode.OK);					
				}
				else
				{
					Response(context, HttpStatusCode.MultipleChoices);
				}
			}
		}

		private void GetFile(HttpListenerContext context)
		{
			var targetSite = context.Request.Url.Segments[1].ToLower();
			string path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(RujanHttpServer)).CodeBase) 
				+ context.Request.Url.AbsolutePath;

			if(targetSite != "favicon.ico")
			{
				path = context.Request.Url.AbsolutePath.ToLower().Replace(targetSite, "");
				path = Path.GetDirectoryName(Assembly.GetAssembly(typeof(RujanHttpServer)).CodeBase)
					+ $"/sites/{targetSite}/{targetSite}/{path}";
			}
			path = path.Remove(0, 6);

			try
			{
				using(var fileStream = File.OpenRead(path))
				{
					string mime;
					context.Response.ContentType = Utilities.MimeTypes.TryGetValue(Path.GetExtension(path), out mime)
						? mime
						: "application/octet-stream";
					context.Response.ContentLength64 = fileStream.Length;

					byte[] buffer = new byte[1024 * 16];
					int bytesCount;
					while((bytesCount = fileStream.Read(buffer, 0, buffer.Length)) > 0)
					{
						context.Response.OutputStream.Write(buffer, 0, bytesCount);
					}

					fileStream.Close();
				}
				Console.WriteLine($"File {path} sended.");
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				Response(context, HttpStatusCode.InternalServerError);
			}
		}

		private void Response(HttpListenerContext context, HttpStatusCode statusCode)
		{
			using(Stream stream = context.Response.OutputStream)
			{
				var headers = new WebHeaderCollection();
				headers.Add(HttpRequestHeader.CacheControl, "no-cache");
				headers.Add(HttpRequestHeader.AcceptCharset, "utf-8");
				headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
				headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU");

				context.Response.Headers = headers;

				context.Response.StatusCode = (int)statusCode;

				Console.WriteLine($"Response status code:	{statusCode}");
			}
		}

		private void Response(HttpListenerContext context, byte[] bytesToWrite, HttpStatusCode statusCode)
		{
			using(Stream stream = context.Response.OutputStream)
			{
				var headers = new WebHeaderCollection();
				headers.Add(HttpRequestHeader.CacheControl, "no-cache");
				headers.Add(HttpRequestHeader.AcceptCharset, "utf-8");
				headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
				headers.Add(HttpRequestHeader.AcceptLanguage, "ru-RU");

				context.Response.Headers = headers;

				context.Response.ContentLength64 = bytesToWrite.Length;
				stream.Write(bytesToWrite, 0, bytesToWrite.Length);
				context.Response.StatusCode = (int)statusCode;

				Console.WriteLine($"Response status code:	{statusCode}");
			}
		}

		private void InitializeSites(string path)
		{
			string[] sites = Directory.GetFiles(path, "*.dll");
			this._sites = new List<ISite>();

			foreach(string sitePath in sites)
			{
				Type objType = null;
				try
				{
					Assembly assembly = Assembly.LoadFrom(sitePath);
					if(assembly != null)
					{
						var fileName = Path.GetFileNameWithoutExtension(sitePath);
						var main = fileName + ".Main";
						objType = assembly.GetType(main);
					}
					if(objType != null)
					{
						var site = (ISite)Activator.CreateInstance(objType);
						_sites.Add(site);
						_listener.Prefixes.Add($"{_settings.Address}:{_settings.Port}{site.Address}");
						Console.WriteLine($"Сайт {_settings.Address}:{_settings.Port}{site.Address} зарегистрирован.");
					}
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex);
					continue;
				}
			}
		}

		#endregion

		#region IDisposable implementation

		public void Dispose()
		{
			_listener?.Abort();
			_listener?.Close();

			foreach(var site in _sites)
			{
				site.Dispose();
			}
		}

		#endregion
	}
}
