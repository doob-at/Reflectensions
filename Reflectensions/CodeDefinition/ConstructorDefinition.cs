namespace doob.Reflectensions.CodeDefinition.Definitions
{
    public class ConstructorSignature
    {
        public string Name { get; set; }

        public TypeSignature? DeclaringType { get; set; }

        public TypeSignature[] GenericArguments { get; set; }

        public ParameterSignature[] Parameters { get; set; }


        public ConstructorSignature(ConstructorInfo constructorInfo)
        {
            Name = constructorInfo.Name;

            if(constructorInfo.DeclaringType != null)
                DeclaringType = TypeSignature.Create(constructorInfo.DeclaringType);

            var parameters = constructorInfo.GetParameters();
            Parameters = parameters.Select(ParameterSignature.Create).ToArray();

            if (constructorInfo.IsGenericMethod)
            {
                var args = constructorInfo.GetGenericArguments().Select(TypeSignature.Create).ToArray();
                GenericArguments = args;
            }
            else
            {
                GenericArguments = Array.Empty<TypeSignature>();
            }
        }

        public override string ToString()
        {
            return $"{Name}({String.Join(", ", Parameters.ToList())})";
        }

        
    }
}
