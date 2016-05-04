using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace RujanService
{
	/// <summary>Предоставляет установщик сервиса.</summary>
	[RunInstaller(true)]
	public partial class RujanInstaller : System.Configuration.Install.Installer
	{
		ServiceInstaller serviceInstaller;
		ServiceProcessInstaller processInstaller;

		/// <summary>Инициализирует <see cref="RujanInstaller"/> и устанавливает <see cref="ServiceRunner"/>.</summary>
		public RujanInstaller()
		{
			InitializeComponent();
			serviceInstaller = new ServiceInstaller();
			processInstaller = new ServiceProcessInstaller();

			processInstaller.Account = ServiceAccount.LocalSystem;
			serviceInstaller.StartType = ServiceStartMode.Manual;
			serviceInstaller.ServiceName = "RujanService";
			Installers.Add(processInstaller);
			Installers.Add(serviceInstaller);
		}
	}
}
