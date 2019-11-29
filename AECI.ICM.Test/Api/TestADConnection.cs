using AECI.ICM.Api.Controllers;
using AECI.ICM.Api.ViewModels;
using Xunit;

namespace AECI.ICM.Test.Api
{
    public class TestADConnection
    {
        [Fact]
        public void TestADSearchFunc()
        {
            var loginCtrl = new LoginController();
            loginCtrl.Authenticate( "mrma86423");
        }
    }
}
