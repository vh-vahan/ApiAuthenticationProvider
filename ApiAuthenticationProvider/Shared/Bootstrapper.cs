using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;


namespace Shared.DependencyInjection
{
    public class Bootstrapper<T> where T : Bootstrapper<T>
    {
        #region Singlton
        private static readonly Lazy<T> bootstrapper = new Lazy<T>(() => CreateInstanceOfT(), true);
        public static T Instance { get { return bootstrapper.Value; } } //SingletonFactory.instance; 
        private static T CreateInstanceOfT()
        {
            //return Activator.CreateInstance(typeof(T), true) as T;
            var type = typeof(T);
            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (constructors.Length == 1)
            {
                var ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
                if ((ctor != null) && (ctor.IsPrivate || ctor.IsFamily))
                {
                    var instance = ctor.Invoke(new object[] { }) as T;

                    if (instance == null)
                    {
                        throw new TypeInitializationException(type.FullName, new NullReferenceException());
                    }

                    (instance as T).Initialise();

                    return instance;
                }
            }
            throw new TypeInitializationException(type.FullName, new TypeAccessException("Type must contain a single (non-public) constructor if derived from Bootstrapper<T>."));
        }

        static Bootstrapper()
        {

        }
        protected Bootstrapper()
        {

        }
        #endregion



        #region Unity
        private readonly Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() => CreateInstanceOfUnityContainer(), true);
        private static IUnityContainer CreateInstanceOfUnityContainer()
        {
            return Activator.CreateInstance(typeof(UnityContainer), true) as IUnityContainer;
        }
        public IUnityContainer Container { get { return container.Value; } }


        public IUnityContainer CreateChildContainer()
        {
            IUnityContainer childContainer = Container.CreateChildContainer();
            RegisterTypesForChildContainer(childContainer);
            return childContainer;
        }


        public IUnityContainer Initialise()
        {
            return BuildUnityContainer();
        }
        protected virtual IUnityContainer BuildUnityContainer()
        {
            return Container;
        }
        public virtual void RegisterTypes(IUnityContainer container)
        {

        }
        public virtual void RegisterTypesForChildContainer(IUnityContainer container)
        {

        }
        #endregion



        #region Resolvers
        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            try
            {
                return Container.Resolve(t, name, resolverOverrides);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public object Resolve(Type t, params ResolverOverride[] resolverOverrides)
        {
            try
            {
                return Container.Resolve(t, resolverOverrides);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public TP Resolve<TP>(string name, params ResolverOverride[] resolverOverrides)
        {
            try
            {
                return Container.Resolve<TP>(name, resolverOverrides);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public TP Resolve<TP>(params ResolverOverride[] resolverOverrides)
        {
            try
            {
                return Container.Resolve<TP>(resolverOverrides);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            try
            {
                return Container.ResolveAll(t, resolverOverrides);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IEnumerable<TP> ResolveAll<TP>(params ResolverOverride[] resolverOverrides)
        {
            try
            {
                return Container.ResolveAll<TP>(resolverOverrides);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion




    }
}
