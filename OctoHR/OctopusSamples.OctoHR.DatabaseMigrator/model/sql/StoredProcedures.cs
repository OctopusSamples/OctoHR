using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctopusSamples.OctoHR.DatabaseMigrator
{
    internal static class StoredProcedures
    {
        public static string ListClients => "[dbo].[List_Enabled_Clients]";
    }
}
