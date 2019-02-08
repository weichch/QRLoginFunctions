using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QRLoginFunctions
{
    internal class Session
    {
        public Session()
        {

        }

        public KeyInfo KeyInfo { get; set; }

        public Task MoveNextAsync()
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var tcs = new TaskCompletionSource<object>();
            cts.Token.Register(
                () => { tcs.TrySetCanceled(cts.Token); },
                false);
            return tcs.Task;
        }
    }
}
