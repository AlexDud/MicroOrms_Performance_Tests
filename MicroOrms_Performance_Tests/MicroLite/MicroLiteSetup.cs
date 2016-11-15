using MicroLite;
using MicroLite.Configuration;
using MicroLite.Listeners;
using MicroLite.Mapping;

namespace MicroOrms_Performance_Tests.MicroLite
{
    public static class MicroLiteSetup
    {
        private static readonly ISessionFactory SessionFactory;

        static MicroLiteSetup()
        {
            Listener.InsertListener.Add(new GuidListener());

            Configure.Extensions()
                .WithConventionBasedMapping(
                    new ConventionMappingSettings
                    {
                        ResolveIdentifierStrategy = type => IdentifierStrategy.Assigned
                    });

            SessionFactory = Configure
                 .Fluently()
                 .ForMsSql2012Connection("TestConnection")
                 .CreateSessionFactory();
        }

        public static ISessionFactory GetSessionFactory()
        {
            return SessionFactory;
        }
    }
}
