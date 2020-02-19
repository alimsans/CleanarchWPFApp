using System;
using System.Linq;
using Cleanarch.DomainLayer.Models;
using Cleanarch.DomainLayer.UseCases;
// ReSharper disable PossibleNullReferenceException
// ReSharper disable PossibleMultipleEnumeration

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var useCaseAdd = new AddTaskUseCase();
            var useCaseRemove = new RemoveTaskUseCase();

            useCaseAdd.Payload = new TaskModel("!!!!!!!!!!!", DateTimeOffset.Now);
            useCaseAdd.Execute(((res) =>
            {
                Console.WriteLine("1st stage.");
                Console.WriteLine(res.Count());
            }))
            .FromResult((res) =>
            {
                Console.WriteLine("2nd stage.");
                Console.WriteLine(res.Count());

                useCaseRemove.Payload = res.FirstOrDefault();
                useCaseRemove.Execute();

                return res;
            })
            .FromResult((res) =>
            {
                throw new Exception();
                return 0;
            }, onError: (e) => { Console.WriteLine(e.Message); });


            Console.ReadLine();
        }

    }
}
//    public class ActionChain<T>
//    {
//        private T _result;
//        private CustomAction _action;

//        public delegate T CustomAction(T data);

//        private ActionChain<T> _parentNode;

//        public ActionChain(T data)
//        {
//            _result = data;
//        }

//        private ActionChain()
//        {
//        }


//        public ActionChain<M> FromResult<M>(CustomAction action)
//        {
//            var newNode = new ActionChain<M>()
//            {
//                _parentNode = this,
//                _action = action
//            };

//            return newNode;
//        }

//        public void Execute()
//        {
//            var current = this;
//            do
//            {
//                current._action?.Invoke(_result);
//                current = current._parentNode;
//            }
//            while (current != null);
//        }
//    }


public class ActionChain<T>
{
    private T _result;

    public delegate M NextAction<out M>(T data);

    public ActionChain(T value)
    {
        _result = value;
    }

    public ActionChain<M> FromResult<M>(NextAction<M> action)
    {
        var result = action.Invoke(_result);

        return new ActionChain<M>(result);
    }
}


//}
