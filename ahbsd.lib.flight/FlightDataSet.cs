using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using SQLitePCL;

namespace ahbsd.lib.flight
{
    /// <summary>
    /// A dataset for FlightData in SQLite 
    /// </summary>
    [Serializable]
    public class FlightDataSet : DataSet
    {
        /// <summary>
        /// Constructor for serializable.
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/></param>
        /// <param name="context"><see cref="StreamingContext"/></param>
        protected FlightDataSet(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            //
        }

        /// <summary>
        /// Simple constructor
        /// </summary>
        public FlightDataSet()
            : base()
        {
            CreatePingTable();
        }

        /// <summary>
        /// Creates the "ping" table.
        /// </summary>
        private void CreatePingTable()
        {
            DataColumn column;
            DataTable pingTable = new DataTable("ping");

            // PK Column pID
            column = new DataColumn()
            {
                ColumnName = "pID",
                DataType = typeof(DateTime),
                AllowDBNull = false,
                Caption = "ping-ID",
                DateTimeMode = DataSetDateTime.Local,
                ReadOnly = true,
                Unique = true,
            };
            
            pingTable.Columns.Add(column);
            
            // Column destination
            column = new DataColumn()
            {
                ColumnName = "destination",
                DataType = typeof(string),
                AllowDBNull = false,
                Caption = "Destination",
                ReadOnly = false,
            };

            pingTable.Columns.Add(column);
            
            // Column reached
            column = new DataColumn()
            {
                ColumnName = "reached",
                DataType = typeof(bool),
                AllowDBNull = false,
                Caption = "Reached",
                ReadOnly = false,
            };
            
            pingTable.Columns.Add(column);
            
            // Make the ID column the primary key column.
            DataColumn[] primaryKeyColumns = new DataColumn[1];
            primaryKeyColumns[0] = pingTable.Columns["pID"];
            pingTable.PrimaryKey = primaryKeyColumns;

            Tables.Add(pingTable);
        }
    }
}