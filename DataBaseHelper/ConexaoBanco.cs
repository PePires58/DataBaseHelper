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
        public ConexaoBanco()
        {
            SqlTransaction = null;
            SqlConnection = new SqlConnection(ConnectionString);
        }
        #endregion

        #region Constantes
        /// <summary>
        /// String de conexão
        /// </summary>
        private const string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=Charmosa;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        #endregion

        #region Propriedades
        /// <summary>
        /// Objeto de conexão
        /// </summary>
        private SqlConnection SqlConnection { get; set; }

        /// <summary>
        /// Objeto de comando
        /// </summary>
        private SqlCommand SqlCommand { get; set; }

        /// <summary>
        /// Objeto de transação
        /// </summary>
        private SqlTransaction SqlTransaction { get; set; }
        #endregion

        #region Métodos

        /// <summary>
        /// Abrir transação
        /// </summary>
        public void BeginTransaction()
        {
            SqlConnection.Open();
            SqlTransaction = SqlConnection.BeginTransaction();
        }

        /// <summary>
        /// Realizar Commit
        /// </summary>
        public void Commit()
        {
            SqlTransaction.Commit();
            SqlConnection.Close();
        }

        /// <summary>
        /// Realizar rollback
        /// </summary>
        public void Rollback()
        {
            SqlTransaction.Rollback();
        }

        /// <summary>
        /// Execute assincronamente
        /// </summary>
        /// <param name="pComando"></param>
        public async void ExecutarAsync(string pComando)
        {
            try
            {
                if (SqlTransaction == null)
                {
                    using (SqlConnection)
                    {
                        MontarAmbienteExecucao(pComando, CommandType.Text);

                        await SqlCommand.ExecuteNonQueryAsync();
                    }
                }
                else
                {
                    MontarAmbienteExecucao(pComando, CommandType.Text);

                    await SqlCommand.ExecuteNonQueryAsync();
                }
            }
            catch
            {
                if (SqlConnection.State == ConnectionState.Open)
                {
                    SqlConnection.Close();
                }
            }
        }

        /// <summary>
        /// Executar
        /// </summary>
        /// <param name="pComando"></param>
        public void Executar(string pComando)
        {
            try
            {
                if (SqlTransaction == null)
                {
                    using (SqlConnection)
                    {
                        MontarAmbienteExecucao(pComando, CommandType.Text);

                        SqlCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    MontarAmbienteExecucao(pComando, CommandType.Text);

                    SqlCommand.ExecuteNonQuery();
                }
            }
            catch
            {
                if (SqlConnection.State == ConnectionState.Open)
                {
                    SqlConnection.Close();
                }
            }
        }

        /// <summary>
        /// Executa a procedure
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        public void ExecutarProcedure(string pNomeProcedure, object pObjeto)
        {
            try
            {
                if (SqlTransaction == null)
                {
                    using (SqlConnection)
                    {
                        MontarAmbienteExecucao(pNomeProcedure, CommandType.StoredProcedure);

                        PreencheParametros(SqlCommand, pObjeto);

                        SqlCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    MontarAmbienteExecucao(pNomeProcedure, CommandType.StoredProcedure);

                    PreencheParametros(SqlCommand, pObjeto);

                    SqlCommand.ExecuteNonQuery();
                }
            }
            catch
            {
                if (SqlConnection.State == ConnectionState.Open)
                {
                    SqlConnection.Close();
                }
            }
        }

        /// <summary>
        /// Executa a procedure
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        public async void ExecutarProcedureAsync(string pNomeProcedure, object pObjeto)
        {
            try
            {
                if (SqlTransaction == null)
                {
                    using (SqlConnection)
                    {
                        MontarAmbienteExecucao(pNomeProcedure, CommandType.StoredProcedure);

                        PreencheParametros(SqlCommand, pObjeto);

                        await SqlCommand.ExecuteNonQueryAsync();
                    }
                }
                else
                {
                    MontarAmbienteExecucao(pNomeProcedure, CommandType.StoredProcedure);

                    PreencheParametros(SqlCommand, pObjeto);

                    await SqlCommand.ExecuteNonQueryAsync();
                }
            }
            catch
            {
                if (SqlConnection.State == ConnectionState.Open)
                {
                    SqlConnection.Close();
                }
            }
        }

        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <returns></returns>
        public DataSet ConsultaPorProcedure(string pNomeProcedure) => ConsultaPorProcedure(pNomeProcedure, new { });

        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <param name="pObjeto">Objeto com os parametros</param>
        /// <returns></returns>
        public DataSet ConsultaPorProcedure(string pNomeProcedure, object pObjeto)
        {
            DataSet dataRecords = new DataSet();
            try
            {
                if (SqlTransaction == null)
                {
                    using (SqlConnection)
                    {
                        MontarAmbienteExecucao(pNomeProcedure, CommandType.StoredProcedure);
                        PreencheParametros(SqlCommand, pObjeto);

                        new SqlDataAdapter { SelectCommand = SqlCommand }.Fill(dataRecords);
                    }
                }
                else
                {
                    MontarAmbienteExecucao(pNomeProcedure, CommandType.StoredProcedure);
                    PreencheParametros(SqlCommand, pObjeto);

                    new SqlDataAdapter { SelectCommand = SqlCommand }.Fill(dataRecords);
                }
                return dataRecords;
            }
            catch
            {
                if (SqlConnection.State == ConnectionState.Open)
                {
                    SqlConnection.Close();
                }
                throw;
            }
        }

        /// <summary>
        /// Consulta um DataSet
        /// </summary>
        /// <param name="pComando"></param>
        /// <returns></returns>
        public DataSet Consulta(string pComando)
        {
            DataSet dataRecords = new DataSet();
            try
            {
                if (SqlTransaction == null)
                {
                    using (SqlConnection)
                    {
                        MontarAmbienteExecucao(pComando, CommandType.Text);

                        new SqlDataAdapter { SelectCommand = SqlCommand }.Fill(dataRecords);
                    }
                }
                else
                {
                    MontarAmbienteExecucao(pComando, CommandType.Text);

                    new SqlDataAdapter { SelectCommand = SqlCommand }.Fill(dataRecords);
                }
                return dataRecords;
            }
            catch
            {
                if (SqlConnection.State == ConnectionState.Open)
                {
                    SqlConnection.Close();
                }
                throw;
            }
        }

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <returns></returns>
        public StringBuilder MontaInsertPorAttributo(object pModel)
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
        public StringBuilder MontaInsertPorAttributo(object pModel, ref StringBuilder pStrBuilder) => MontaStringBuilderInsert(pModel, pStrBuilder);

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


        #region Métodos privados

        /// <summary>
        /// Método que monta o ambiente para execução
        /// </summary>
        /// <param name="pComando"></param>
        private void MontarAmbienteExecucao(string pComando, CommandType pCommandType)
        {
            if (SqlConnection.State == ConnectionState.Closed)
            {
                SqlConnection.Open();
            }

            SqlCommand = new SqlCommand
            {
                CommandType = pCommandType,
                CommandText = pComando,
                Connection = SqlConnection,
            };

            if (SqlTransaction != null)
            {
                SqlCommand.Transaction = SqlTransaction;
            }
        }

        /// <summary>
        /// Monta o comando e os parametros
        /// </summary>
        /// <param name="sqlCommand">Objeto de sqlCommand</param>
        /// <param name="objeto">Objeto que teram suas propriedades passadas para a procedure</param>
        private void PreencheParametros(SqlCommand pSqlCommand, object pObjeto)
        {
            SqlCommandBuilder.DeriveParameters(SqlCommand);

            PropertyInfo[] propriedades = pObjeto.GetType().GetProperties();

            for (int i = 1; i < SqlCommand.Parameters.Count; i++)
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
