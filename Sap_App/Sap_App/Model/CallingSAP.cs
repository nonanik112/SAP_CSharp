using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sap_App.Model
{
    internal static void CallingSAP()
    {
        ECCDestinationConfig cfg = null;
        RfcDestination dest = null;
        try
        {
            cfg = new ECCDestinationConfig();
            RfcDestinationManager.RegisterDestinationConfiguration(cfg);
            dest = RfcDestinationManager.GetDestination("mySAPdestination");

            RfcRepository repo = dest.Repository;
            IRfcFunction fnpush = repo.CreateFunction("XFUNCTION");

            // Send Data With RFC Structure
            IRfcStructure data = fnpush.GetStructure("IM_STRUCTURE");
            data.SetValue("ITEM1", "VALUE1");
            data.SetValue("ITEM2", "VALUE2");
            data.SetValue("ITEM3", "VALUE3");
            data.SetValue("ITEM4", "VALUE4");

            fnpush.SetValue("IM_STRUCTURE", data);
           
            // Send Data With RFC Table
            IRfcTable dataTbl = fnpush.GetTable("IM_TABLE");
            foreach (var item in ListItem)
            {
                dataTbl.Append();
                dataTbl.SetValue("ITEM1", item.VALUE1);
                dataTbl.SetValue("ITEM2", item.VALUE2);
                dataTbl.SetValue("ITEM3", item.VALUE3);
                dataTbl.SetValue("ITEM4", item.VALUE4);
            }
            fnpush.Invoke(dest);

            var exObject = fnpush.GetObject("EX_OBJECT");
            IRfcStructure exStructure = fnpush.GetStructure("EX_STRUCTURE");

            RfcSessionManager.EndContext(dest);
            RfcDestinationManager.UnregisterDestinationConfiguration(cfg);
        }
        catch (Exception ex)
        {

           RfcSessionManager.EndContext(dest);
            RfcDestinationManager.UnregisterDestinationConfiguration(cfg);
            ThreadStaticAttribute.Sleep(1000);
        }
    }
}
