using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace doob.Reflectensions.CodeDefinition.Definitions
{
    public class ConstructorDefinition
    {
        public string Name { get; } = "constructor";

        public TypeDefinition? DeclaringType { get; set; }
        
        public List<TypeDefinition> GenericArguments { get; set; } = new();

        public List<ParameterDefinition> Parameters { get; set; } = new();


        public override string ToString()
        {
            
            return $"{Name}({string.Join(", ", Parameters)})";
        }

        public static ConstructorDefinition FromConstructorInfo(ConstructorInfo constructorInfo)
        {
            var constructorDefinition = new ConstructorDefinition();

            if(constructorInfo.DeclaringType != null)
                constructorDefinition.DeclaringType = TypeDefinition.Create(constructorInfo.DeclaringType);

            var parameters = constructorInfo.GetParameters();
            constructorDefinition.Parameters = parameters.Select(ParameterDefinition.Create).ToList();

            if (constructorInfo.IsGenericMethod)
            {

                var args = constructorInfo.GetGenericArguments().Select(TypeDefinition.Create).ToList();
                
                constructorDefinition.GenericArguments = args;

            }

            return constructorDefinition;
        }
    }
}
