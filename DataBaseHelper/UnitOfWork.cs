#region Histórico de manutenção
/*
 * Data: 22/02/2020
 * Programador: Pedro Henrique Pires
 * Descrição: Implementação inicial
 */

/*
* Data: 23/02/2020
* Programador: Pedro Henrique Pires
* Descrição: Removido herança e migrado métodos da classe de conexão banco
*/

/*
Data: 29/02/2020
Programador: Pedro Henrique Pires
Descrição: Refatoração da classe.
*/

/*
Data: 09/07/2020
Programador: Pedro Henrique Pires
Descrição: Ajuste para verificar se conexão está fechada.
*/
#endregion
using DataBaseHelper.Interfaces;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHelper
{
    /// <summary>
    /// Unidade de trabalho, utilize-a para interação com o banco
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Propriedades

        /// <summary>
        /// Commandos helper
        /// </summary>
        private ICommandHelper _CommandHelper { get; set; }

        /// <summary>
        /// Objeto de conexão
        /// </summary>
        private SqlConnection _SqlConnection { get; set; }

        /// <summary>
        /// Objeto de transação
        /// </summary>
        private SqlTransaction _SqlTransaction { get; set; }

        #endregion

        #region Propriedades readonly
        /// <summary>
        /// String de conecção
        /// </summary>
        private string _connectionString;
        #endregion

        #region Construtores

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="pConnectionString"></param>
        public UnitOfWork(string pConnectionString)
        {
            _SqlTransaction = null;
            _SqlConnection = new SqlConnection(pConnectionString);
            _connectionString = pConnectionString;
            _CommandHelper = new CommandHelper();
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Abrir transação
        /// </summary>
        void IUnitOfWork.BeginTransaction()
        {
            if (_SqlConnection.State == ConnectionState.Closed)
            {
                _SqlConnection = new SqlConnection(_connectionString);
                _SqlConnection.Open();
            }

            _SqlTransaction = _SqlConnection.BeginTransaction();
        }

        /// <summary>
        /// Realizar Commit
        /// </summary>
        void IUnitOfWork.Commit()
        {
            _SqlTransaction.Commit();
            _SqlConnection.Close();
        }

        /// <summary>
        /// Realizar rollback
        /// </summary>
        void IUnitOfWork.Rollback()
        {
            _SqlTransaction.Rollback();
        }

        /// <summary>
        /// Execute assincronamente
        /// </summary>
        /// <param name="pComando"></param>
        async void IUnitOfWork.ExecutarAsync(string pComando)
        {
            if (_SqlConnection.State == ConnectionState.Closed)
            {
                _SqlConnection = new SqlConnection(_connectionString);
            }

            await Task.Run(() => new ConexaoBanco(_SqlTransaction,_SqlConnection).ExecutarAsync(pComando));
        }

        /// <summary>
        /// Executar
        /// </summary>
        /// <param name="pComando"></param>
        void IUnitOfWork.Executar(string pComando)
        {
            if (_SqlConnection.State == ConnectionState.Closed)
            {
                _SqlConnection = new SqlConnection(_connectionString);
            }

            new ConexaoBanco(_SqlTransaction, _SqlConnection).Executar(pComando);
        }

        /// <summary>
        /// Executa a procedure
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        void IUnitOfWork.ExecutarProcedure(string pNomeProcedure, object pObjeto)
        {
            if (_SqlConnection.State == ConnectionState.Closed)
            {
                _SqlConnection = new SqlConnection(_connectionString);
            }
            new ConexaoBanco(_SqlTransaction, _SqlConnection).ExecutarProcedure(pNomeProcedure, pObjeto);
        }

        /// <summary>
        /// Executa a procedure assincronamente
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        async void IUnitOfWork.ExecutarProcedureAsync(string pNomeProcedure)
        {
            if (_SqlConnection.State == ConnectionState.Closed)
            {
                _SqlConnection = new SqlConnection(_connectionString);
            }

            await Task.Run(() => new ConexaoBanco(_SqlTransaction, _SqlConnection).ExecutarProcedureAsync(pNomeProcedure, new { }));
        }

        /// <summary>
        /// Executa a procedure assincronamente
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        async void IUnitOfWork.ExecutarProcedureAsync(string pNomeProcedure, object pObjeto)
        {
            if (_SqlConnection.State == ConnectionState.Closed)
            {
                _SqlConnection = new SqlConnection(_connectionString);
            }

            await Task.Run(() => new ConexaoBanco(_SqlTransaction, _SqlConnection).ExecutarProcedureAsync(pNomeProcedure, pObjeto));
        }

        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <returns></returns>
        DataSet IUnitOfWork.ConsultaPorProcedure(string pNomeProcedure)
        {
            if (_SqlConnection.State == ConnectionState.Closed)
            {
                _SqlConnection = new SqlConnection(_connectionString);
            }

            return new ConexaoBanco(_SqlTransaction, _SqlConnection).ConsultaPorProcedure(pNomeProcedure);
        }

        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <param name="pObjeto">Objeto com os parametros</param>
        /// <returns></returns>
        DataSet IUnitOfWork.ConsultaPorProcedure(string pNomeProcedure, object pObjeto)
        {
            if (_SqlConnection.State == ConnectionState.Closed)
            {
                _SqlConnection = new SqlConnection(_connectionString);
            }

            return new ConexaoBanco(_SqlTransaction, _SqlConnection).ConsultaPorProcedure(pNomeProcedure, pObjeto);
        }

        /// <summary>
        /// Consulta um DataSet
        /// </summary>
        /// <param name="pComando"></param>
        /// <returns></returns>
        DataSet IUnitOfWork.Consulta(string pComando)
        {
            if (_SqlConnection.State == ConnectionState.Closed)
            {
                _SqlConnection = new SqlConnection(_connectionString);
            }

            return new ConexaoBanco(_SqlTransaction, _SqlConnection).Consulta(pComando);
        }

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <returns></returns>
        StringBuilder IUnitOfWork.MontaInsertPorAttributo(object pModel) =>
            _CommandHelper.MontaInsertPorAttributo(pModel);

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <param name="pStrBuilder">String Builder</param>
        /// <returns></returns>
        StringBuilder IUnitOfWork.MontaInsertPorAttributo(object pModel, ref StringBuilder pStrBuilder) =>
            _CommandHelper.MontaInsertPorAttributo(pModel, ref pStrBuilder);

        #endregion
    }

}
