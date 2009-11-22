using System;
using log4net.Config;

namespace NHibernate.Tool.hbm2net
{
	/// <summary>
	/// Summary description for Bootstrap.
	/// </summary>
	public class Bootstrap
	{
		[STAThread]
		public static void Main(String[] args)
		{
            XmlConfigurator.Configure();
			CodeGenerator.Generate(args);
		}
	}
}