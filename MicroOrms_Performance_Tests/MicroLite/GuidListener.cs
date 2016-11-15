using System;
using MicroLite.Listeners;
using MicroLite.Mapping;

namespace MicroOrms_Performance_Tests.MicroLite
{
    public class GuidListener : IInsertListener
    {
        public void AfterInsert(object instance, object executeScalarResult)
        {
            // nothing to do
            return; 
        }

        public void BeforeInsert(object instance)
        {
            var objectInfo = ObjectInfo.For(instance.GetType());

            if (objectInfo.TableInfo.IdentifierStrategy == IdentifierStrategy.Assigned
                && objectInfo.TableInfo.IdentifierColumn.PropertyInfo.PropertyType == typeof(Guid))
            {
                var identifier = Guid.NewGuid();

                objectInfo.SetIdentifierValue(instance, identifier);
            }
        }
    }
}