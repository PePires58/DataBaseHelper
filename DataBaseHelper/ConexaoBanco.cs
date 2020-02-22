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
#endregion


using DataBaseHelper.Atributos;
using DataBaseHelper.Models;
using ModulosHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHelper
{
    public class ConexaoBanco
    {
        #region Construtores
        /// <summary>
        /// Construtor
        /// </summary>
        internal ConexaoBanco(string pConnectionString)
        {
            _SqlTransaction = null;
            _ConnectionString = pConnectionString;
            _SqlConnection = new SqlConnection(_ConnectionString);
        }
        #endregion

        #region Propriedades

        /// <summary>
        /// String de conexão
        /// </summary>
        private readonly string _ConnectionString;

        /// <summary>
        /// Objeto de conexão
        /// </summary>
        private SqlConnection _SqlConnection { get; set; }

        /// <summary>
        /// Objeto de comando
        /// </summary>
        private SqlCommand _SqlCommand { get; set; }

        /// <summary>
        /// Objeto de transação
        /// </summary>
        private SqlTransaction _SqlTransaction { get; set; }
        #endregion

        #region Métodos

        /// <summary>
        /// Abrir transação
        /// </summary>
        protected void BeginTransaction()
        {
            _SqlConnection.Open();
            _SqlTransaction = _SqlConnection.BeginTransaction();
        }

        /// <summary>
        /// Realizar Commit
        /// </summary>
        protected void Commit()
        {
            _SqlTransaction.Commit();
            _SqlConnection.Close();
        }

        /// <summary>
        /// Realizar rollback
        /// </summary>
        protected void Rollback()
        {
            _SqlTransaction.Rollback();
        }

        /// <summary>
        /// Execute assincronamente
        /// </summary>
        /// <param name="pComando"></param>
        protected async void ExecutarAsync(string pComando)
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
            }
        }

        /// <summary>
        /// Executar
        /// </summary>
        /// <param name="pComando"></param>
        protected void Executar(string pComando)
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
            }
        }

        /// <summary>
        /// Executa a procedure
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        protected void ExecutarProcedure(string pNomeProcedure, object pObjeto)
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
            }
        }

        /// <summary>
        /// Executa a procedure
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        protected async void ExecutarProcedureAsync(string pNomeProcedure, object pObjeto)
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
            }
        }

        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <returns></returns>
        protected DataSet ConsultaPorProcedure(string pNomeProcedure) => ConsultaPorProcedure(pNomeProcedure, new { });

        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <param name="pObjeto">Objeto com os parametros</param>
        /// <returns></returns>
        protected DataSet ConsultaPorProcedure(string pNomeProcedure, object pObjeto)
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
        protected DataSet Consulta(string pComando)
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

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <returns></returns>
        protected StringBuilder MontaInsertPorAttributo(object pModel)
        {
            StringBuilder strBuilder = new StringBuilder();
            return MontaInsertPorAttributo(pModel, ref strBuilder);
        }

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <param name="pStrBuilder">String Builder</param>
        /// <returns></returns>
        protected StringBuilder MontaInsertPorAttributo(object pModel, ref StringBuilder pStrBuilder) => MontaStringBuilderInsert(pModel, pStrBuilder);

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

        /// <summary>
        /// Monta o string builder para insert
        /// </summary>
        /// <param name="pModel"></param>
        /// <param name="pStrBuilder"></param>
        /// <returns></returns>
        private static StringBuilder MontaStringBuilderInsert(object pModel, StringBuilder pStrBuilder)
        {
            PropertyInfo[] propriedades = pModel.GetType().GetProperties();

            var colunaModel = new List<ColunaModel>();

            if (pModel.GetType().GetCustomAttribute(typeof(TabelaAttribute)) is TabelaAttribute pTabela)
            {

                foreach (PropertyInfo propertyInfo in propriedades)
                    if (propertyInfo.GetCustomAttribute(typeof(ColunaAttribute)) is ColunaAttribute pColuna)
                        colunaModel.Add(new ColunaModel()
                        {
                            NomeColuna = pColuna.NomeColuna,
                            TipoDadoBanco = pColuna.TipoDado,
                            ValorCampo = propertyInfo.GetValue(pModel)
                        });

                pStrBuilder.Append($"INSERT INTO {(pTabela.Temporaria ? "#" : "")}{pTabela.NomeTabela}(");

                for (int i = 0; i < colunaModel.Count; i++)
                    pStrBuilder.AppendLine($"{colunaModel[i].NomeColuna} {(i == colunaModel.Count - 1 ? ")" : ",")}");

                pStrBuilder.AppendLine("VALUES(");

                for (int i = 0; i < colunaModel.Count; i++)
                    switch (colunaModel[i].TipoDadoBanco)
                    {
                        case Enumerados.TipoDadosBanco.Char:
                        case Enumerados.TipoDadosBanco.Varchar:
                            pStrBuilder.AppendLine($"'{colunaModel[i].ValorCampo.ToString()}'{(i == colunaModel.Count - 1 ? ")" : ",")}");
                            break;
                        case Enumerados.TipoDadosBanco.Tinyint:
                        case Enumerados.TipoDadosBanco.Integer:
                        case Enumerados.TipoDadosBanco.BigInt:
                            pStrBuilder.AppendLine($"{Convert.ToInt64(colunaModel[i].ValorCampo)}{(i == colunaModel.Count - 1 ? ")" : ",")}");
                            break;
                        case Enumerados.TipoDadosBanco.Enum:
                            pStrBuilder.AppendLine($"'{((Enum)colunaModel[i].ValorCampo).GetDefaultValue()}'{(i == colunaModel.Count - 1 ? ")" : ",")}");
                            break;
                        case Enumerados.TipoDadosBanco.Float:
                            pStrBuilder.AppendLine($"{Convert.ToDouble(colunaModel[i].ValorCampo)}{(i == colunaModel.Count - 1 ? ")" : ",")}");
                            break;
                        case Enumerados.TipoDadosBanco.Byte:
                            pStrBuilder.AppendLine($"{(Convert.ToBoolean(colunaModel[i].ValorCampo) ? 1 : 0)}{(i == colunaModel.Count - 1 ? ")" : ",")}");
                            break;
                    }
            }
            return pStrBuilder;
        }

        #endregion

    }
}
