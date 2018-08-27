using Xamarin.Forms;
using Acquaint.XForms;
using Acquaint.Abstractions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
[assembly: Dependency( typeof( CapabilityService ) )]

namespace Acquaint.XForms
{
  public class CapabilityService : ICapabilityService
  {
    readonly IEnvironmentService _EnvironmentService;

    public CapabilityService()
    {
      _EnvironmentService = SimpleIoc.Default.GetInstance<IEnvironmentService>(); // ServiceLocator.Current.GetInstance<IEnvironmentService>();
    }

    #region ICapabilityService implementation

    public bool CanMakeCalls => _EnvironmentService.IsRealDevice || (Device.RuntimePlatform != Device.iOS);

    public bool CanSendMessages => _EnvironmentService.IsRealDevice || (Device.RuntimePlatform != Device.iOS);

    public bool CanSendEmail => _EnvironmentService.IsRealDevice || (Device.RuntimePlatform != Device.iOS);

    #endregion
  }
}

