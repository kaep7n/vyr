using System;
using System.Reflection;

namespace Vyr.Isolation.Context
{
    public class IsolationControllerProxy : DispatchProxy
    {
        private object target;

        internal void SetTarget(object target)
        {
            this.target = target;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            if (targetMethod is null)
            {
                throw new ArgumentNullException(nameof(targetMethod));
            }

            return this.target.GetType().GetMethod(targetMethod.Name).Invoke(this.target, args);
        }
    }
}
