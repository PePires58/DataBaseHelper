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
        /// Classe que conexão de banco
        /// </summary>
        private readonly ConexaoBanco _ConexaoBanco;

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

        #region Construtores

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="pConnectionString"></param>
        public UnitOfWork(string pConnectionString)
        {
            _ConexaoBanco = new ConexaoBanco(pConnectionString);
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Abrir transação
        /// </summary>
        void IUnitOfWork.BeginTransaction()
        {
            _SqlConnection.Open();
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
            await Task.Run(() => _ConexaoBanco.ExecutarAsync(pComando));
        }

        /// <summary>
        /// Executar
        /// </summary>
        /// <param name="pComando"></param>
        void IUnitOfWork.Executar(string pComando)
        {
            _ConexaoBanco.Executar(pComando);
        }

        /// <summary>
        /// Executa a procedure
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        void IUnitOfWork.ExecutarProcedure(string pNomeProcedure, object pObjeto)
        {
            _ConexaoBanco.ExecutarProcedure(pNomeProcedure, pObjeto);
        }

        /// <summary>
        /// Executa a procedure assincronamente
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        async void IUnitOfWork.ExecutarProcedureAsync(string pNomeProcedure, object pObjeto)
        {
            await Task.Run(() => _ConexaoBanco.ExecutarProcedureAsync(pNomeProcedure, pObjeto));
        }

        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <returns></returns>
        DataSet IUnitOfWork.ConsultaPorProcedure(string pNomeProcedure) => _ConexaoBanco.ConsultaPorProcedure(pNomeProcedure);
        
        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <param name="pObjeto">Objeto com os parametros</param>
        /// <returns></returns>
        DataSet IUnitOfWork.ConsultaPorProcedure(string pNomeProcedure, object pObjeto) => _ConexaoBanco.ConsultaPorProcedure(pNomeProcedure, pObjeto);

        /// <summary>
        /// Consulta um DataSet
        /// </summary>
        /// <param name="pComando"></param>
        /// <returns></returns>
        DataSet IUnitOfWork.Consulta(string pComando) => _ConexaoBanco.Consulta(pComando);

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <returns></returns>
        StringBuilder IUnitOfWork.MontaInsertPorAttributo(object pModel) =>
            _ConexaoBanco.MontaInsertPorAttributo(pModel);

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <param name="pStrBuilder">String Builder</param>
        /// <returns></returns>
        StringBuilder IUnitOfWork.MontaInsertPorAttributo(object pModel, ref StringBuilder pStrBuilder) =>
            _ConexaoBanco.MontaInsertPorAttributo(pModel, ref pStrBuilder);

        #endregion
    }

}
