using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var obs = new Observable<string>("ssss");

            
            obs.FromCallable((str) =>
            {
                return "1";

            }).FromCallable((str) =>
            {
                return "2";

            }).FromCallable((str) =>
            {
                return "3";

            });
        }
    }

    public class Observable<T>
    {
        private T _result;

        public delegate T NextAction(T data);

        public Observable(T value)
        {
            _result = value;
        }

        public Observable<T> FromCallable(NextAction action)
        {
            var result = action.Invoke(_result);

            return new Observable<T>(result);
        }
    }
}
