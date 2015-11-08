using System.ComponentModel.Composition;
using Resolver;
using TimeDifference.BusinessClasses;
using IComponent = System.ComponentModel.IComponent;

namespace TimeDifference.Business
{
    [Export(typeof(IComponent))]
    public class DependencyResolver : Resolver.IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IUserServices, UserServices>();
            registerComponent.RegisterType<ITokenServices, TokenServices>();
            
        }
    }
}
