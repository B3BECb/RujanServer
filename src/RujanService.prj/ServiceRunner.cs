using System;
using System.ServiceProcess;

namespace RujanService
{
	/// <summary>Предоставляет загрузчик сервиса.</summary>
	public partial class ServiceRunner : ServiceBase
	{
		#region Data

		RujanHttpServer _server;

		ServerSettings _settings;

		#endregion

		#region .ctor

		/// <summary>Инициализирует и создаёт <see cref="ServiceRunner"/>.</summary>
		public ServiceRunner()
		{
			InitializeComponent();
			this.CanStop = true;
			this.CanPauseAndContinue = true;
			this.AutoLog = true;

			_settings = new ServerSettings();
			_settings.Load();
		}

		#endregion

		#region Override

		protected override void OnStart(string[] args)
		{
			_server = new RujanHttpServer(_settings);
			_server.StartAsync();
		}

		protected override void OnStop()
		{
			_server.Stop();
		}

		#endregion
	}
}
