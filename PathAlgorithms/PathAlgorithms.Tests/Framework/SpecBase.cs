using System;

namespace ACS.TechTest.Tests.Framework
{
    public class SpecBase<T> : SpecBase
    {
        protected T Target { get; set; }
    }

    public class SpecBase
    {
        public SpecBase()
        {
// Deliberately calling virtual method as part of testing framework
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            Given();
            try
            {
                When();
            }
            catch (Exception e)
            {
                Exception = e;
            }
// ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        protected virtual void Given()
        {
            throw new NotImplementedException();
        }

        protected virtual void When() 
        {
            throw new NotImplementedException();
        }

        protected Exception Exception { get; set; }
    }
}