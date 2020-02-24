using AECI.ICM.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace AECI.ICM.Data.Context
{
    public class MapConfigDbCtx
    {
        #region Fields

        private SqlConnection _conn;
     
        #endregion

        #region Constructor

        public MapConfigDbCtx(string connString)
        {
            _conn = new SqlConnection(connString);
        }

        #endregion

        #region Methods

        public List<Branch> GetAll()
        {
            var branches = new List<Branch>();

            if (_conn.State != ConnectionState.Open)
                _conn.Open();

            var sql = "SELECT * FROM tblPlant where IsActive = 'true' and CompanyCode='MA'";
            var dt = new DataTable();

            try
            {
                using (SqlDataAdapter da = new SqlDataAdapter(sql, _conn))
                {
                    da.Fill(dt);

                    if(!dt.HasErrors && dt.Rows.Count > 0)
                    {
                        for(int i = 0; i < dt.Rows.Count -1; i++)
                        {
                            var branch = new Branch
                            {
                                Id = dt.Rows[i]["Id"] == DBNull.Value ? -1: Convert.ToInt32(dt.Rows[i]["Id"]),
                                CompanyCode = dt.Rows[i]["CompanyCode"] == DBNull.Value ? "" : dt.Rows[i]["CompanyCode"].ToString().Trim(),
                                Plant = dt.Rows[i]["Plant"] == DBNull.Value ? -1:Convert.ToInt32(dt.Rows[i]["Plant"]),
                                Code = dt.Rows[i]["Code"] == DBNull.Value ? "" : dt.Rows[i]["Code"].ToString().Trim(),
                                Region = dt.Rows[i]["Region"] == DBNull.Value ? "" : dt.Rows[i]["Region"].ToString().Trim(),
                                Name = dt.Rows[i]["Name"] == DBNull.Value ? "" : dt.Rows[i]["Name"].ToString().Trim(),
                                ServerUrl = dt.Rows[i]["ServerUrl"] == DBNull.Value ? "" :  dt.Rows[i]["ServerUrl"].ToString().Trim()
                            };
                            branches.Add(branch);
                        }
                    }
                }

                return branches;
            }
            catch (Exception)
            {
                throw;
            }
        }
       
        #endregion
    }
}
