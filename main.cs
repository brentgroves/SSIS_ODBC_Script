#region Help:  Introduction to the Script Component
/* The Script Component allows you to perform virtually any operation that can be accomplished in
 * a .Net application within the context of an Integration Services data flow.
 *
 * Expand the other regions which have "Help" prefixes for examples of specific ways to use
 * Integration Services features within this script component. */
#endregion
// https://stackoverflow.com/questions/2799405/odbccommand-on-stored-procedure-parameter-not-supplied-error-on-output-param

#region Namespaces
using System;
using System.Data;
using System.Data.Odbc;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
#endregion

/// <summary>
/// This is the class to which to add your code.  Do not change the name, attributes, or parent
/// of this class.
/// </summary>
[Microsoft.SqlServer.Dts.Pipeline.SSISScriptComponentEntryPointAttribute]
public class ScriptMain : UserComponent
{
    #region Help:  Using Integration Services variables and parameters
    /* To use a variable in this script, first ensure that the variable has been added to
     * either the list contained in the ReadOnlyVariables property or the list contained in
     * the ReadWriteVariables property of this script component, according to whether or not your
     * code needs to write into the variable.  To do so, save this script, close this instance of
     * Visual Studio, and update the ReadOnlyVariables and ReadWriteVariables properties in the
     * Script Transformation Editor window.
     * To use a parameter in this script, follow the same steps. Parameters are always read-only.
     *
     * Example of reading from a variable or parameter:
     *  DateTime startTime = Variables.MyStartTime;
     *
     * Example of writing to a variable:
     *  Variables.myStringVariable = "new value";
     */
    #endregion

    #region Help:  Using Integration Services Connnection Managers
    /* Some types of connection managers can be used in this script component.  See the help topic
     * "Working with Connection Managers Programatically" for details.
     *
     * To use a connection manager in this script, first ensure that the connection manager has
     * been added to either the list of connection managers on the Connection Managers page of the
     * script component editor.  To add the connection manager, save this script, close this instance of
     * Visual Studio, and add the Connection Manager to the list.
     *
     * If the component needs to hold a connection open while processing rows, override the
     * AcquireConnections and ReleaseConnections methods.
     * 
     * Example of using an ADO.Net connection manager to acquire a SqlConnection:
     *  object rawConnection = Connections.SalesDB.AcquireConnection(transaction);
     *  SqlConnection salesDBConn = (SqlConnection)rawConnection;
     *
     * Example of using a File connection manager to acquire a file path:
     *  object rawConnection = Connections.Prices_zip.AcquireConnection(transaction);
     *  string filePath = (string)rawConnection;
     *
     * Example of releasing a connection manager:
     *  Connections.SalesDB.ReleaseConnection(rawConnection);
     */
    #endregion

    #region Help:  Firing Integration Services Events
    /* This script component can fire events.
     *
     * Example of firing an error event:
     *  ComponentMetaData.FireError(10, "Process Values", "Bad value", "", 0, out cancel);
     *
     * Example of firing an information event:
     *  ComponentMetaData.FireInformation(10, "Process Values", "Processing has started", "", 0, fireAgain);
     *
     * Example of firing a warning event:
     *  ComponentMetaData.FireWarning(10, "Process Values", "No rows were received", "", 0);
     */
    #endregion
    OdbcConnection odbcConn;
    OdbcDataReader odbcDataReader;

    /// <summary>
    /// This method is called once, before rows begin to be processed in the data flow.
    ///
    /// You can remove this method if you don't need to do anything here.
    /// </summary>
    public override void PreExecute()
    {
        base.PreExecute();
        odbcConn = new OdbcConnection("DSN=Plex;UID=mg.odbcalbion;PWD=Mob3xalbion;");
        odbcConn.Open();
        
        OdbcCommand cmd = new OdbcCommand("{call sproc300758_11728751_1898254(?,?,?)}", odbcConn);
//        OdbcCommand cmd = new OdbcCommand("{call sproc300758_11728751_1904315(?,?,?)}", odbcConn);
        //        OdbcCommand cmd = new OdbcCommand("{call sproc300758_11728751_1904306(?)}", odbcConn);
        //        OdbcCommand cmd = new OdbcCommand("{call sproc300758_11728751_1904306(?)}", odbcConn);
        cmd.Parameters.Add("@PCN", OdbcType.Int);
        cmd.Parameters["@PCN"].Value = 300758;
        cmd.Parameters.Add("@By_Due_Date", OdbcType.NVarChar);
        cmd.Parameters["@By_Due_Date"].Value = "20210701";
        cmd.Parameters.Add("@Building_Key", OdbcType.Int);
        cmd.Parameters["@Building_Key"].Value = 5644;
        //        cmd.Parameters.Add("@PCN", OdbcType.NVarChar);
        //        cmd.Parameters["@PCN"].Value = "300758";

        //        OdbcCommand cmd = new OdbcCommand("{call sproc300758_11728751_1903209}", odbcConn);
        //        OdbcCommand cmd = new OdbcCommand("select top 10 Name from part_v_part", odbcConn);

        // Execute the DataReader and access the data.
        odbcDataReader = cmd.ExecuteReader();

    }

    /// <summary>
    /// This method is called after all the rows have passed through this component.
    ///
    /// You can delete this method if you don't need to do anything here.
    /// </summary>
    public override void PostExecute()
    {
        base.PostExecute();
        odbcDataReader.Close();
        odbcConn.Close();

    }

    public override void CreateNewOutputRows()
    {
        /*
          Add rows by calling the AddRow method on the member variable named "<Output Name>Buffer".
          For example, call MyOutputBuffer.AddRow() if your output was named "MyOutput".
        */
        //   OutputBuffer.AddRow();
        //   OutputBuffer.Name = "Test";
        // OutputBuffer.Name = "Test";
        /*
         odbcDataReader.Read();
         OutputBuffer.AddRow();
         OutputBuffer.pcn = odbcDataReader.GetInt32(0);
         OutputBuffer.partkey = odbcDataReader.GetInt32(1);
         OutputBuffer.qtydue = odbcDataReader.GetInt32(2);
         OutputBuffer.qtyshipped = odbcDataReader.GetInt32(3);
         OutputBuffer.qtywip = odbcDataReader.GetInt32(4);
         OutputBuffer.qtyready = odbcDataReader.GetInt32(5);
         OutputBuffer.qtyloaded = odbcDataReader.GetInt32(6);
         OutputBuffer.qtyreadyorloaded = odbcDataReader.GetInt32(7);
         OutputBuffer.lessthanduedate = odbcDataReader.GetDateTime(8);
        */
        //        OutputBuffer.AddRow();
        //        OutputBuffer.pcn = odbcDataReader.GetInt32(0);
        while (odbcDataReader.Read())
                {
                        OutputBuffer.AddRow();
                        OutputBuffer.pcn = odbcDataReader.GetInt32(0);
                        OutputBuffer.buildingkey = odbcDataReader.GetInt32(1);
                        OutputBuffer.partkey = odbcDataReader.GetInt32(2);
                        OutputBuffer.qtydue = odbcDataReader.GetInt32(3);
                        OutputBuffer.qtyshipped = odbcDataReader.GetInt32(4);
                        OutputBuffer.qtywip = odbcDataReader.GetInt32(5);
                        OutputBuffer.qtyready = odbcDataReader.GetInt32(6);
                        OutputBuffer.qtyloaded = odbcDataReader.GetInt32(7);
                        OutputBuffer.qtyreadyorloaded = odbcDataReader.GetInt32(8);
                        //OutputBuffer.lessthanduedate = odbcDataReader.GetDateTime(8);
                }
      
    }

}
