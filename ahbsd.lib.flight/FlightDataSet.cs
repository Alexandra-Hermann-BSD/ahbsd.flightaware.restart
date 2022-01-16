using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Common;
using System.Runtime.Serialization;
using ahbsd.lib.Exceptions;

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
        /// <remarks>
        /// Adds the tables and foreign key relations.
        /// </remarks>
        public FlightDataSet()
        {
            Tables.Add(CreatePingTable());
            Tables.Add(CreateFlightAwareTable());
            Tables.Add(CreateFaAdsBTable());
            CreateRelationFlightaware_faADB_B();
        }

        #region setting tables for the DataSet
        /// <summary>
        /// Creates the "ping" table.
        /// </summary>
        /// <returns>The created "ping" table</returns>
        private DataTable CreatePingTable()
        {
            DataTable pingTable = new DataTable("ping");
            pingTable.BeginInit();
            
            // PK Column pID
            DataColumn column = new DataColumn
            {
                ColumnName = "pID",
                DataType = typeof(DateTime),
                AllowDBNull = false,
                Caption = "ping-ID",
                DateTimeMode = DataSetDateTime.Local,
                ReadOnly = true,
                Unique = true,
                DefaultValue = DateTime.Now,
            };
            
            pingTable.Columns.Add(column);
            
            // Column destination
            column = new DataColumn
            {
                ColumnName = "destination",
                DataType = typeof(string),
                AllowDBNull = false,
                Caption = "Destination",
                ReadOnly = false,
            };

            pingTable.Columns.Add(column);
            
            // Column reached
            column = new DataColumn
            {
                ColumnName = "reached",
                DataType = typeof(bool),
                AllowDBNull = false,
                Caption = "Reached",
                ReadOnly = false,
            };
            
            pingTable.Columns.Add(column);
            
            // Make the ID column the primary key column.
            SetIdAsPk(pingTable, "pID");

            pingTable.EndInit();
            return pingTable;
        }

        /// <summary>
        /// Creates the table "flightaware".
        /// </summary>
        /// <returns>The created table "flightaware"</returns>
        private DataTable CreateFlightAwareTable()
        {
            DataTable flightAwareTable = new DataTable("flightaware");
            flightAwareTable.BeginInit();

            // Column "sID
            DataColumn column = new DataColumn
            {
                ColumnName = "sID",
                DataType = typeof(int),
                AutoIncrement = true,
                AllowDBNull = false,
                ReadOnly = true,
                Caption = "ID",
            };

            flightAwareTable.Columns.Add(column);
            
            // Column "tsCaptured"
            column = new DataColumn
            {
                ColumnName = "tsCaptured",
                DataType = typeof(DateTime),
                AllowDBNull = true,
                ReadOnly = false,
                Caption = "Captured",
            };

            flightAwareTable.Columns.Add(column);
            
            // Make the ID column the primary key column.
            SetIdAsPk(flightAwareTable, "sID");

            flightAwareTable.EndInit();
            return flightAwareTable;
        }

        /// <summary>
        /// Creates the table "faADS_B".
        /// </summary>
        /// <returns>The created table "faADS_B"</returns>
        private DataTable CreateFaAdsBTable()
        {
            DataTable faADS_B = new DataTable("faADS_B");
            DataColumn column;
            faADS_B.BeginInit();
            
            // column abID
            column = new DataColumn
            {
                ColumnName = "abID",
                AutoIncrement = true,
                AllowDBNull = false,
                DataType = typeof(int),
                ReadOnly = true,
                Caption = "ID",
            };

            faADS_B.Columns.Add(column);
            
            // column sID
            column = new DataColumn
            {
                ColumnName = "sID",
                AllowDBNull = false,
                DataType = typeof(int),
                ReadOnly = false,
                Caption = "FlightAwareID",
            };
            
            faADS_B.Columns.Add(column);
            
            // column frequency
            column = new DataColumn
            {
                ColumnName = "frequency",
                AllowDBNull = false,
                DataType = typeof(int),
                DefaultValue = 1090,
                Caption = "Frequency",
                ReadOnly = false,
            };

            faADS_B.Columns.Add(column);
            
            // column isRunning
            column = new DataColumn
            {
                ColumnName = "isRunning",
                AllowDBNull = false,
                DataType = typeof(bool),
                DefaultValue = false,
                Caption = "is running?",
                ReadOnly = false,
            };

            faADS_B.Columns.Add(column);
            
            // column pid
            column = new DataColumn
            {
                ColumnName = "pid",
                AllowDBNull = true,
                DataType = typeof(ulong),
                DefaultValue = null,
                Caption = "process ID",
                ReadOnly = false,
            };

            faADS_B.Columns.Add(column);
            
            // column comment
            column = new DataColumn
            {
                ColumnName = "comment",
                AllowDBNull = true,
                DataType = typeof(string),
                DefaultValue = null,
                Caption = "Comments",
                ReadOnly = false,
            };

            faADS_B.Columns.Add(column);
            
            SetIdAsPk(faADS_B, "sID");
            
            faADS_B.EndInit();
            return faADS_B;
        }

        /// <summary>
        /// Creates the foreign key for faADS_B and adds it.
        /// </summary>
        private void CreateRelationFlightaware_faADB_B()
        {
            DataColumn parentColumn =
                Tables["flightaware"].Columns["sID"];
            DataColumn childColumn =
                Tables["faADS_B"].Columns["sID"];
            DataRelation relation = new
                DataRelation("fk_faADS_B__flightaware", parentColumn, childColumn);
            Tables["faADS_B"].ParentRelations.Add(relation);
        }
        #endregion
        #region public static methods
        /// <summary>
        /// Sets the given column name of the given table as primary key.
        /// </summary>
        /// <param name="table">The given table</param>
        /// <param name="columnName">The given column name</param>
        /// <exception cref="Exception{DataTable}">If the given Table is <c>null</c> or has not the named column</exception>
        public static void SetIdAsPk(DataTable table, string columnName)
        {
            if (table != null && table.Columns.Contains(columnName))
            {
                DataColumn[] primaryKeyColumns = new DataColumn[1];
                primaryKeyColumns[0] = table.Columns[columnName];
                table.PrimaryKey = primaryKeyColumns;
            }
            else
            {
                Exception<DataTable> wrongTable;
                if (table == null)
                {
                    wrongTable = new Exception<DataTable>("The given Table is null!", table);
                }
                else if (table.Columns.Count == 0)
                {
                    wrongTable = new Exception<DataTable>($"{table.Columns.Count} columns can't have a column named '{columnName}'...", table);
                }
                else if (!table.Columns.Contains(columnName))
                {
                    wrongTable =
                        new Exception<DataTable>(
                            $"The given table '{table.TableName}' doesn't have a column named '{columnName}'!", table);
                }
                else
                {
                    wrongTable = new Exception<DataTable>(table);
                }
                
                throw wrongTable;
            }
        }

        /// <summary>
        /// Sets the given columns of the given table as primary key.
        /// </summary>
        /// <param name="table">The given table</param>
        /// <param name="columnNames">A list of column names, which are names of primary keys</param>
        /// <exception cref="Exception{DataTable}">
        /// If the table is empty or the column names are empty or the given column doesn't exists in the table.
        /// </exception>
        public static void SetIdAsPk(DataTable table, IList<string> columnNames)
        {
            if (table != null && columnNames != null && columnNames.Count > 0)
            {
                DataColumn[] primaryKeyColumns = new DataColumn[columnNames.Count];

                for (int i = 0; i < columnNames.Count; i++)
                {
                    DataColumn tempColumn = TryGetTempColumn(table, columnNames, i);
                    primaryKeyColumns[i] = tempColumn;
                }
                
                table.PrimaryKey = primaryKeyColumns;
            }
            else
            {
                Exception<DataTable> wrongTable;
                if (table == null)
                {
                    wrongTable = new Exception<DataTable>("The given Table is null!", table);
                }
                else if (table.Columns.Count == 0)
                {
                    wrongTable = new Exception<DataTable>($"{table.Columns.Count} columns can't have a named column...", table);
                }
                else if (columnNames?.Count == 0)
                {
                    wrongTable =
                        new Exception<DataTable>(
                            $"The given table '{table.TableName}' doesn't have a column named ''!", table);
                }
                else
                {
                    wrongTable = new Exception<DataTable>(table);
                }
                
                throw wrongTable;
            }
        }

        /// <summary>
        /// Trys to get a DataColumn from a given table, which name is at position i in the given
        /// columnNames list.
        /// </summary>
        /// <param name="table">The given table</param>
        /// <param name="columnNames">The given list of column names</param>
        /// <param name="i">The given position</param>
        /// <returns>The found <see cref="DataColumn"/></returns>
        /// <exception cref="Exception{DataTable}">If the given column name doesn't exists in the given table</exception>
        private static DataColumn TryGetTempColumn(DataTable table, IList<string> columnNames, int i)
        {
            DataColumn tempColumn;
            var tempColumnName = columnNames[i];
            try
            {
                tempColumn = table.Columns[tempColumnName];
            }
            catch (Exception e)
            {
                throw new Exception<DataTable>($"The given Table '{table.TableName}' doesn't have" +
                                                      $"the given column '{tempColumnName}'", table, e);
            }

            return tempColumn;
        }

        /// <summary>
        /// Sets the first column of a given Table as primary key.
        /// </summary>
        /// <param name="table">The given Table</param>
        /// <exception cref="Exception{DataTable}">If the given Table is <c>null</c> or has no first column</exception>
        public static void SetFirstColumnAsPk(DataTable table)
        {
            if (table != null && table.Columns.Count > 0)
            {
                DataColumn[] primaryKeyColumns = new DataColumn[1];
                primaryKeyColumns[0] = table.Columns[0];
                table.PrimaryKey = primaryKeyColumns;
            }
            else
            {
                Exception<DataTable> wrongTable;
                if (table == null)
                {
                    wrongTable = new Exception<DataTable>("The given Table is null!", table);
                }
                else if (table.Columns.Count == 0)
                {
                    wrongTable = new Exception<DataTable>($"{table.Columns.Count} columns can't have a first one...", table);
                }
                else
                {
                    wrongTable = new Exception<DataTable>(table);
                }
                
                throw wrongTable;
            }
        }
        #endregion
    }
}