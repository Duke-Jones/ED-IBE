using System;
using System.ComponentModel;

namespace RegulatedNoise
{
	internal class Disposer : IComponent
	{
		private readonly IDisposable _disposable;
		private bool _disposed;


		public Disposer(IDisposable disposable)
		{
			if (disposable == null)
			{
				throw new ArgumentNullException("disposable");
			}
			_disposable = disposable;
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposable.Dispose();
				_disposed = true;
				RaiseDisposed();
			}
		}

		public ISite Site { get; set; }

		public event EventHandler Disposed;

		protected virtual void RaiseDisposed()
		{
			var handler = Disposed;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}
	}
}