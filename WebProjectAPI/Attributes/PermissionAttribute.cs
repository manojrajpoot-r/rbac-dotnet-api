namespace WebProjectAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAttribute : Attribute
    {
        public string Name { get; }

        public PermissionAttribute(string name)
        {
            Name = name;
        }
    }
}
