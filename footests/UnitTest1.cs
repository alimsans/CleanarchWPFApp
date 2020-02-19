using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using 
using Xunit;
using System.Threading;

// ReSharper disable IdentifierTypo

namespace Footests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var addUseCase = new AddTaskUseCase()
            {
                Payload = new TaskModel("sss", DateTimeOffset.Now)
            };

            var fooCollection = new List<TaskModel>();

            //addUseCase.ExecuteInChain(result => Assert.True(true))
            //    .Then(addUseCase.Execute(result => Assert.True(true)));

            addUseCase.ExecuteInChain(null).ContinueWith((onComplete) => addUseCase.ExecuteInChain(null));
        }
    }
}
