using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTest
{
    //Ссылки на ресурс ПЕРЕДАЮЩИЙСЯ в Bundle
    class Ids
    {
       // static public string patient = "02255d1f-548c-4b04-9ac2-7c97d3efad1a";
        static public string practitioner = "131d7d5d-0f21-451d-86ec-27fa3e069e1a";
        static public string diagnosticOrder = "2c98670c-3494-4c63-bb29-71acd486da3d";
        static public string specimen = "f8cd600f-f5b5-4b18-9662-18212c1935f9";
        static public string encounter = "f0ceca14-6847-4ea4-b128-7c86820da428";
        
        static public string condition = "64d57862-f2c2-41ef-a5cf-27f2d53569eb";
        static public string condition_min = "65d57862-f2c2-41ef-a5cf-27f2d53569eb";

        static public string observation = "651f0cdc-2e7f-4e3a-99b1-da68d2b196c6";
        static public string observation_res = "661f0cdc-2e7f-4e3a-99b1-da68d2b196c6";

        static public string coverage = "04c84a8b-8de7-400a-b9d0-53e6ce37a9bb";
        static public string diagnosticReport = "4f6a30fb-cd3c-4ab6-8757-532101f72065";
        static public string order = "cd45a667-bde0-490f-b602-8d780acf4aa2";
        //static public string supportInfo = "56350c6f-7333-4002-a622-96968b85381e";
        //static public string indicat = "71cf33b8-2eae-432d-88d5-747ef8147d0b";
    }

    //Ссылки на уже СУЩЕСТВУЮЩИЙ ресурс 
    class References
    {
        static public string organization_Patient = "Link/4a94e705-ee3e-46fc-bba0-0298e0fd5bd2";
        
        static public string organization = "Organization/4a94e705-ee3e-46fc-bba0-0298e0fd5bd2";
        static public string organization_two = "Organization/ ";
        static public string patient = "Patient/02255d1f-548c-4b04-9ac2-7c97d3efad1a";
        static public string practitioner = "Practitioner/eb292a81-bc3a-4309-b365-9761fdee0443";
        static public string encounter = "Encounter/1b377871-c721-40c6-8189-7d96369180b0";
        static public string order = "Order/001dccff-f603-4262-8d5d-e66e73e499e9";
        static public string diagnosticOrder = "DiagnosticOrder/05c93184-997d-405e-84bc-340996a59e57";
    }
}
