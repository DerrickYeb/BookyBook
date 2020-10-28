using BookyBook.DataAccess.Data;
using Dapper;
using DataAccess.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace DataAccess.Repository
{
    public class SP_Call : ISP_Call
    {
        private static string ConnnectionString = "";
        private readonly ApplicationDbContext _context;

        public SP_Call(ApplicationDbContext context)
        {
            _context = context;
            ConnnectionString = context.Database.GetDbConnection().ConnectionString;
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public void Execute(string procdureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnnectionString))
            {
                sqlConnection.Open();
                sqlConnection.Execute(procdureName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> List<T>(string procdureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnnectionString))
            {
                sqlConnection.Open();
                return sqlConnection.Query<T>(procdureName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnnectionString))
            {
                sqlConnection.Open();
                var result = SqlMapper.QueryMultiple(sqlConnection,procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
                var item1 = result.Read<T1>().ToList();
                var item2 = result.Read<T2>().ToList();

                if (item1 != null && item2 != null)
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(item1, item2);
                }
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
            }
        }

        public T OneRecord<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnnectionString))
            {
                sqlConnection.Open();
                var value = sqlConnection.Query(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
                return (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T));
            }
        }
            public T Single<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnnectionString))
            {
                sqlConnection.Open();
               return(T)Convert.ChangeType(sqlConnection.ExecuteScalar<T>(procedureName, param, commandType: System.Data.CommandType.StoredProcedure),typeof(T));
            }
        }
    }
}
