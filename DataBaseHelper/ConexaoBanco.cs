#region Histórico de Manutenção
/*
 Data: 15/02/2020
 Programador: Pedro Henrique Pires
 Descrição: Implementação Inicial da classe.
 */

/*
 * Data: 21/02/2020
 * Programador: Pedro Henrique Pires
 * Descrição: Implementação de métodos de consulta e por procedure
 */

/*
* Data: 22/02/2020
* Programador: Pedro Henrique Pires
* Descrição: Removendo connection string e recebendo por parametro
*/

/*
 * Data: 23/02/2020
 * Programador: Pedro Henrique Pires
 * Descrição: Migração de métodos para UnitOfWork
 */

/*
Data: 29/02/2020
Programador: Pedro Henrique Pires
Descrição: Refatoração da classe.
*/
#endregion


using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace DataBaseHelper
{
    /// <summary>
    /// Classe de conexão com banco
    /// </summary>
    internal class ConexaoBanco
    {
        #region Construtores
        /// <summary>
        /// Construtor
        /// </summary>
        internal ConexaoBanco(SqlTransaction pSqlTransaciton, SqlConnection pSqlConnection)
        {
            _SqlTransaction = pSqlTransaciton;
            _SqlConnection = pSqlConnection;
        }
        #endregion

        #region Propriedades

        /// <summary>
        /// Objeto de conexão
        /// </summary>
        private readonly SqlConnection _SqlConnection;

        /// <summary>
        /// Objeto de comando
        /// </summary>
        private SqlCommand _SqlCommand { get; set; }

        /// <summary>
        /// Objeto de transação
        /// </summary>
        private readonly SqlTransaction _SqlTransaction;
        #endregion

        #region Métodos

        /// <summary>
        /// Execute assincronamente
        /// </summary>
        /// <param name="pComando"></param>
        internal async void ExecutarAsync(string pComando)
        {
            try
            {
                if (_SqlTransaction == null)
                {
                    using (_SqlConnection)
                    {
                        MontarAmbienteExecucao(pComando, CommandType.Text);

                        await _SqlCommand.ExecuteNonQueryAsync();
                    }
                }
                else
                {
                    MontarAmbienteExecucao(pComando, CommandType.Text);

                    await _SqlCommand.ExecuteNonQueryAsync();
                }
            }
            catch
            {
                if (_SqlConnection.State == ConnectionState.Open)
                {
                    _SqlConnection.Close();
                }
                throw;
            }
        }

        /// <summary>
        /// Executar
        /// </summary>
        /// <param name="pComando"></param>
        internal void Executar(string pComando)
        {
            try
            {
                if (_SqlTransaction == null)
                {
                    using (_SqlConnection)
                    {
                        MontarAmbienteExecucao(pComando, CommandType.Text);

                        _SqlCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    MontarAmbienteExecucao(pComando, CommandType.Text);

                    _SqlCommand.ExecuteNonQuery();
                }
            }
            catch
            {
                if (_SqlConnection.State == ConnectionState.Open)
                {
                    _SqlConnection.Close();
                }
                throw;
            }
        }

        /// <summary>
        /// Executa a procedure
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        internal void ExecutarProcedure(string pNomeProcedure, object pObjeto)
        {
            try
            {
                if (_SqlTransaction == null)
                {
                    using (_SqlConnection)
                    {
                        MontarAmbienteExecucao(pNomeProcedure, CommandType.StoredProcedure);

                        PreencheParametros(_SqlCommand, pObjeto);

                        _SqlCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    MontarAmbienteExecucao(pNomeProcedure, CommandType.StoredProcedure);

                    PreencheParametros(_SqlCommand, pObjeto);

                    _SqlCommand.ExecuteNonQuery();
                }
            }
            catch
            {
                if (_SqlConnection.State == ConnectionState.Open)
                {
                    _SqlConnection.Close();
                }
                throw;
            }
        }

        /// <summary>
        /// Executa a procedure
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        internal async void ExecutarProcedureAsync(string pNomeProcedure, object pObjeto)
        {
            try
            {
                if (_SqlTransaction == null)
                {
                    using (_SqlConnection)
                    {
                        MontarAmbienteExecucao(pNomeProcedure, CommandType.StoredProcedure);

                        PreencheParametros(_SqlCommand, pObjeto);

                        await _SqlCommand.ExecuteNonQueryAsync();
                    }
                }
                else
                {
                    MontarAmbienteExecucao(pNomeProcedure, CommandType.StoredProcedure);

                    PreencheParametros(_SqlCommand, pObjeto);

                    await _SqlCommand.ExecuteNonQueryAsync();
                }
            }
            catch
            {
                if (_SqlConnection.State == ConnectionState.Open)
                {
                    _SqlConnection.Close();
                }
                throw;
            }
        }

        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <returns></returns>
        internal DataSet ConsultaPorProcedure(string pNomeProcedure) => ConsultaPorProcedure(pNomeProcedure, new { });

        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <param name="pObjeto">Objeto com os parametros</param>
        /// <returns></returns>
        internal DataSet ConsultaPorProcedure(string pNomeProcedure, object pObjeto)
        {
            DataSet dataRecords = new DataSet();
            try
            {
                if (_SqlTransaction == null)
                {
                    using (_SqlConnection)
                    {
                        MontarAmbienteExecucao(pNomeProcedure, CommandType.StoredProcedure);
                        PreencheParametros(_SqlCommand, pObjeto);

                        new SqlDataAdapter { SelectCommand = _SqlCommand }.Fill(dataRecords);
                    }
                }
                else
                {
                    MontarAmbienteExecucao(pNomeProcedure, CommandType.StoredProcedure);
                    PreencheParametros(_SqlCommand, pObjeto);

                    new SqlDataAdapter { SelectCommand = _SqlCommand }.Fill(dataRecords);
                }
                return dataRecords;
            }
            catch
            {
                if (_SqlConnection.State == ConnectionState.Open)
                {
                    _SqlConnection.Close();
                }
                throw;
            }
        }

        /// <summary>
        /// Consulta um DataSet
        /// </summary>
        /// <param name="pComando"></param>
        /// <returns></returns>
        internal DataSet Consulta(string pComando)
        {
            DataSet dataRecords = new DataSet();
            try
            {
                if (_SqlTransaction == null)
                {
                    using (_SqlConnection)
                    {
                        MontarAmbienteExecucao(pComando, CommandType.Text);

                        new SqlDataAdapter { SelectCommand = _SqlCommand }.Fill(dataRecords);
                    }
                }
                else
                {
                    MontarAmbienteExecucao(pComando, CommandType.Text);

                    new SqlDataAdapter { SelectCommand = _SqlCommand }.Fill(dataRecords);
                }
                return dataRecords;
            }
            catch
            {
                if (_SqlConnection.State == ConnectionState.Open)
                {
                    _SqlConnection.Close();
                }
                throw;
            }
        }

       
        #endregion

        #region Métodos privados

        /// <summary>
        /// Método que monta o ambiente para execução
        /// </summary>
        /// <param name="pComando"></param>
        private void MontarAmbienteExecucao(string pComando, CommandType pCommandType)
        {
            if (_SqlConnection.State == ConnectionState.Closed)
            {
                _SqlConnection.Open();
            }

            _SqlCommand = new SqlCommand
            {
                CommandType = pCommandType,
                CommandText = pComando,
                Connection = _SqlConnection,
            };

            if (_SqlTransaction != null)
            {
                _SqlCommand.Transaction = _SqlTransaction;
            }
        }

        /// <summary>
        /// Monta o comando e os parametros
        /// </summary>
        /// <param name="sqlCommand">Objeto de sqlCommand</param>
        /// <param name="objeto">Objeto que teram suas propriedades passadas para a procedure</param>
        private void PreencheParametros(SqlCommand pSqlCommand, object pObjeto)
        {
            SqlCommandBuilder.DeriveParameters(_SqlCommand);

            PropertyInfo[] propriedades = pObjeto.GetType().GetProperties();

            for (int i = 1; i < _SqlCommand.Parameters.Count; i++)
            {
                object parametro = null;

                var propriedade = propriedades[i - 1];

                switch (propriedades[i - 1].PropertyType.Name)
                {
                    case "Boolean":
                        parametro = Convert.ToBoolean(propriedade.GetValue(pObjeto)) ? 1 : 0;
                        break;
                    case "Byte":
                        parametro = Convert.ToByte(propriedade.GetValue(pObjeto));
                        break;
                    case "DateTime":
                        parametro = Convert.ToDateTime(propriedade.GetValue(pObjeto));
                        break;
                    case "Decimal":
                        parametro = Convert.ToDecimal(propriedade.GetValue(pObjeto));
                        break;
                    case "Double":
                        parametro = Convert.ToDouble(propriedade.GetValue(pObjeto));
                        break;
                    case "Int16":
                    case "Int32":
                    case "Int64":
                        parametro = Convert.ToInt64(propriedade.GetValue(pObjeto));
                        break;
                    case "Char":
                    case "String":
                        parametro = propriedade.GetValue(pObjeto).ToString();
                        break;
                }

                pSqlCommand.Parameters[i].Value = parametro;
            }
        }

        #endregion

    }
}
