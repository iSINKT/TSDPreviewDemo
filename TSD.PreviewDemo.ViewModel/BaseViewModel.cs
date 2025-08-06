using ReactiveUI;
using TSD.PreviewDemo.Core.Users;

namespace TSD.PreviewDemo.ViewModel
{
    public class BaseViewModel : ReactiveObject
    {
        private User _user;

        public User User
        {
            get => _user;
            set => this.RaiseAndSetIfChanged(ref _user, value);
        }
    }
}


