using ApplicationCore.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Services
{
    public class ActivatorService : IActivatorService
    {
        protected delegate object ObjectActivator(params object[] args);
        protected static Dictionary<Type, ObjectActivator> Activators { get; } = new Dictionary<Type, ObjectActivator>();

        protected ObjectActivator CreateActivator(Type type, params Type[] argTypes)
        {
            if (type == null)
                throw new ArgumentException("Incorrect class name", "className");
            // Get the public instance constructor that takes an integer parameter.
            ConstructorInfo ctor = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public, null,
                CallingConventions.HasThis, argTypes, null) 
                ?? throw new Exception("There is no any constructor with specified parameters.");
            //Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");
            Expression[] argsExp = new Expression[paramsInfo.Length];
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;
                Expression paramAccessorExp = Expression.ArrayIndex(param, index);
                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);
                argsExp[i] = paramCastExp;
            }
            NewExpression newExp = Expression.New(ctor, argsExp);
            LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator), newExp, param);
            return (ObjectActivator)lambda.Compile();
        }
        protected ObjectActivator GetActivator(Type type, params Type[] argTypes)
        {

            if (Activators.TryGetValue(type, out ObjectActivator activator))
            {
                return activator;
            }
            activator = CreateActivator(type, argTypes);
            Activators[type] = activator;
            return activator;
        }

        public T GetInstance<T>(params object[] constructorArguments)
        {
            var types = (from c in constructorArguments select c.GetType()).ToArray();
            return (T)GetActivator(typeof(T), types)(constructorArguments);
        }
    }
}
