using System;

namespace BookService.WebApi.Infrastructure.Extensions
{
    public class DTOAttribute : Attribute
    {
        public Type Type { get; private set; }
        public DTOAttribute(Type type)
        {
            Type = type;
        }
    }
}