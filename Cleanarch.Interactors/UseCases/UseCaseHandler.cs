using System;
using System.Threading.Tasks;

namespace Cleanarch.DomainLayer.UseCases
{
    public class UseCaseHandler<TOutput>
    {
        public delegate void OnStartCallback();
        public delegate void OnCompleteCallback<in M>(M result);
        public delegate void OnErrorCallback(Exception error);

        public OnStartCallback OnStart { get; set; }
        public OnErrorCallback OnError { get; set; }
        public OnCompleteCallback<TOutput> OnComplete { get; set; }

        public delegate M NextAction<out M>(TOutput data);

        internal TOutput Result;    
        public Task ExecutingTask { get; internal set; }

        public UseCaseHandler
            (OnCompleteCallback<TOutput> onComplete = null, OnStartCallback onStart = null, OnErrorCallback onError = null)
        {
            OnStart = onStart;
            OnError = onError;
            OnComplete = onComplete;
        }

        private UseCaseHandler(TOutput data)
        {
            Result = data;  
        }

        public UseCaseHandler<M> FromResult<M>(
            NextAction<M> action, OnCompleteCallback<M> onComplete = null, OnStartCallback onStart = null,
            OnErrorCallback onError = null)
        {
            M result = default;
            var nextHandler = new UseCaseHandler<M>();

            nextHandler.ExecutingTask = ExecutingTask?.ContinueWith(t =>
            {
                onStart?.Invoke();

                try
                {
                    result = action.Invoke(Result);

                    onComplete?.Invoke(result);
                }
                catch (Exception e)
                {
                    onError?.Invoke(e);
                }

                nextHandler.Result = result;
            });

            return nextHandler;

        }

        public UseCaseHandler<M> FromResult<M>(NextAction<M> action, UseCaseHandler<M> callbackHandler)
        {
            M result = default;
            var nextHandler = new UseCaseHandler<M>();

            nextHandler.ExecutingTask = ExecutingTask?.ContinueWith(t =>
            {
                callbackHandler.OnStart?.Invoke();

                try
                {
                    result = action.Invoke(Result);

                    callbackHandler.OnComplete?.Invoke(result);
                }
                catch (Exception e)
                {
                    callbackHandler.OnError?.Invoke(e);
                }

                nextHandler.Result = result;
            });
            
            return nextHandler;
        }
    }
}
