using Microservice.Framework.Domain;
using Microservice.Framework.Persistence.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMornitoringTool.API.Core.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static IDomainContainer ConfigurationFramework(this IDomainContainer domainContainer, IEntityFrameworkConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            configuration.Apply(domainContainer.ServiceCollection);
            return domainContainer;
        }

        public static IDomainContainer AddContextProvider<TDbContext,IContextProvider>( this IDomainContainer domainContainer)
            where IContextProvider:class,IDbContextProvider<TDbContext>
            where TDbContext : DbContext
        {
            domainContainer.ServiceCollection.AddTransient<IDbContextProvider<TDbContext>, IContextProvider>();

            return domainContainer;
        }
    }
}
