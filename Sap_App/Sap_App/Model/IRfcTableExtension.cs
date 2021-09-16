using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Sap_App.Model
{
    public static class IRfcTableExtension
    {
      /// <summary>
      /// Converts SAP table to .NET DataTable table
      /// </summary>
      /// <param name="sapTable"></param>
      /// <param name="name"></param>
      /// <returns></returns>
      public static DataTable ToDataTable(this IRfcTable sapTable, string name)
      {
            DataTable adoTable = new DataTable(name);
            //..Create ADO.net table.
            for (int LiElement = 0; LiElement < sapTable.ElementCount; LiElement++)
            {
                RfcElementMetadata metadata = sapTable.GetElementMetadata(LiElement);
                adoTable.Columns.Add(metadata.Name, GetDataType(metadata.DataType));
            }
            //Transfer rows from SAP Table Ado.net Table
            foreach (IRfcStructure row in sapTable)
            {
                DataRow ldr = adoTable.NewRow();
                for (int liElement = 0; liElement < sapTable.ElementCount; liElement++)
                {
                    RfcElementMetadata metadata = sapTable.GetElementMetadata(liElement);
                    switch (metadata.DataType)
                    {
                        case RfcDataType.DATE:
                            ldr[metadata.Name] = row.GetString(metadata.Name).Substring(0,4) + row.GetString(metadata.Name).Substring(5,2) + row.GetString(metadata.Name).Substring(8,2);
                            break;
                        case RfcDataType.BCD:
                            ldr[metadata.Name] = row.GetDecimal(metadata.Name);
                            break;
                        case RfcDataType.CHAR:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.STRING:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                        case RfcDataType.INT2:
                            ldr[metadata.Name] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.INT4:
                            ldr[metadata.Name] = row.GetInt(metadata.Name);
                            break;
                        case RfcDataType.FLOAT:
                            ldr[metadata.Name] = row.GetDouble(metadata.Name);
                            break;
                        default:
                            ldr[metadata.Name] = row.GetString(metadata.Name);
                            break;
                    }
                }
                adoTable.Rows.Add(ldr);
            }
            return adoTable;
        }
        private static Type GetDataType(RfcDataType rfcDataType)
        {
            switch (rfcDataType)
            {
                case RfcDataType.DATE:
                    return typeof(string);
                    break;
                case RfcDataType.CHAR:
                    return typeof(string);
                    break;
                case RfcDataType.STRING:
                    return typeof(string);
                    break;
                case RfcDataType.BCD:
                    return typeof(decimal);
                    break;
                case RfcDataType.INT2:
                    return typeof(int);
                    break;
                case RfcDataType.INT4:
                    return typeof(int);
                    break;
                case RfcDataType.FLOAT:
                    return typeof(double);
                    break;
                default:
                    return typeof(string);
                    break;
            }
        }
    }
}
