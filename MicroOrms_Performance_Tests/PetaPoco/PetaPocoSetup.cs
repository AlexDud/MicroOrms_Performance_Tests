using PetaPoco;

namespace MicroOrms_Performance_Tests.PetaPoco
{
    public static class PetaPocoSetup
    {
        private static readonly IDatabase Db;

        static PetaPocoSetup()
        {
            Db = new Database("TestConnection");
            PocoData.FlushCaches();
        }

        public static IDatabase GetDb()
        {
            return Db;
        }
    }
}
