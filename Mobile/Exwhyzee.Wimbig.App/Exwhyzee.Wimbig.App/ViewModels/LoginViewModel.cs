using Exwhyzee.Wimbig.App.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Text;
using System.Text.RegularExpressions;

namespace Exwhyzee.Wimbig.App.ViewModels
{
    public class LoginViewModel: ViewModelBase
    {
        IUserManager userManager;

        private string _username;
        public string UserName
        {
            get => _username;

            //Notify when property user name changes
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        /// <summary>
        /// This is an Oaph Observable property helper, 
        /// Which is used to determine whether a subsequent action
        /// Could be performed or not depending on its value
        /// This condition is calculated every time its value changes.
        /// </summary>
        ObservableAsPropertyHelper<bool> _validLogin;
        public bool ValidLogin
        {
            get { return _validLogin?.Value ?? false; }
        }

        public ReactiveCommand<Unit,DateTime> LoginCommand { get; private set; }

       
        public LoginViewModel(IUserManager userManager, IScreen hostScreen = null) : base(hostScreen)
        {
            this.userManager = userManager;

            this.WhenAnyValue(x => x.UserName, x => x.Password,
                (email, password) =>
                (
                    ///Validate the password
                    !string.IsNullOrEmpty(password) && password.Length > 5
                )
                &&
                (
                    ///Validate teh email.
                    !string.IsNullOrEmpty(email)
                            &&
                     Regex.Matches(email, "^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$").Count == 1
                ))
                .ToProperty(this, v => v.ValidLogin, out _validLogin);

            //LoginCommand = ReactiveCommand.CreateFromTask(async () =>
            //{

            //    var lg = await userManager.Login(_username, _password);
            //    if (lg)
            //    {
            //        HostScreen.Router
            //                    .Navigate
            //                    .Execute(new dynamic)
            //                    .Subscribe();
            //    }
            //}, this.WhenAnyValue(x => x.ValidLogin, x => x.ValidLogin, (validLogin, valid) => ValidLogin && valid));

        }


    }
}
