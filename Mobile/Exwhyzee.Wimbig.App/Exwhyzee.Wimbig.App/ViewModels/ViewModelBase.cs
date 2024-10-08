﻿using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exwhyzee.Wimbig.App.ViewModels
{
    public class ViewModelBase : ReactiveObject, IRoutableViewModel, ISupportsActivation
    {
        public string UrlPathSegment
        {
            get;
            protected set;
        }
       

        public IScreen HostScreen
        {
            get;
            protected set;
        }

        public ViewModelActivator Activator
        {
            get { return viewModelActivator; }
        }

        protected readonly ViewModelActivator viewModelActivator = new ViewModelActivator();

        public ViewModelBase(IScreen hostScreen = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        }
    }
}
