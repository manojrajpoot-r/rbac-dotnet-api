namespace WebProjectAPI.Attributes
{
   
  
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class PermissionAttribute : Attribute
        {
            public string Name { get; }

            public PermissionAttribute(string name)
            {
                Name = name;
            }
        }
    
}
