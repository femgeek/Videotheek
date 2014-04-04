using System.Configuration;
using System.Data.Common;
using System.Data.SQLite;

namespace VideoCommon
{
	public class VideoDbManager
	{
		private static ConnectionStringSettings conVideoSettings = ConfigurationManager.ConnectionStrings["Video"];
		private static DbProviderFactory factory = DbProviderFactories.GetFactory(conVideoSettings.ProviderName);
		public DbConnection GetConnection()
		{
			var conVideo = factory.CreateConnection();
			conVideo.ConnectionString = conVideoSettings.ConnectionString;
			return conVideo;
		}
	}
}
