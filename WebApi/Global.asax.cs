using System.Data.Entity;
using System.Web;
using System.Web.Http;
using WebApi.Models;

namespace WebApi
{
	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			Database.SetInitializer(new BankDbContextInitializer());
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}
	}
}