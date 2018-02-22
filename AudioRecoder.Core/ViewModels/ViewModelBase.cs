using System;
using System.Reactive.Disposables;
using Prism.Mvvm;

namespace AudioRecoder.Core.ViewModels
{
    public abstract class ViewModelBase : BindableBase, IDisposable
    {
        protected CompositeDisposable Disposable { get; private set; }

        public ViewModelBase()
        {
            this.Disposable = new CompositeDisposable();
        }

        public void Dispose()
        {
            this.Disposable.Dispose();
        }
    }
}
