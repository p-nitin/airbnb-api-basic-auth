using airbnb.api.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using JsonProperty = Newtonsoft.Json.Serialization.JsonProperty;

namespace airbnb.api.Extensions
{
    public class FieldsFilterContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (property.DeclaringType == typeof(BaseEntity) || property.DeclaringType.BaseType == typeof(BaseEntity))
            {
                if (property.PropertyName == "SerializableProperties")
                {
                    property.ShouldSerialize = instance => { return false; };
                }
                else
                {
                    property.ShouldSerialize = instance =>
                    {
                        if(instance.GetType() == typeof(Listing))
                        {
                            var p = (Listing)instance;
                            return p.SerializableProperties.Contains(property.PropertyName);
                        }
                        return false;
                    };
                }
            }
            return property;
        }
    }
}
