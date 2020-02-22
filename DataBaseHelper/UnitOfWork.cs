#region Histórico de manutenção
/*
 * Data: 22/02/2020
 * Programador: Pedro Henrique Pires
 * Descrição: Implementação inicial
 */
#endregion
using DataBaseHelper.Interfaces;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseHelper
{
    /// <summary>
    /// Unidade de trabalho, utilize-a para interação com o banco
    /// </summary>
    public class UnitOfWork : ConexaoBanco, IUnitOfWork
    {
        #region Construtores

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="pConnectionString"></param>
        public UnitOfWork(string pConnectionString) : base(pConnectionString)
        {
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Abrir transação
        /// </summary>
        void IUnitOfWork.BeginTransaction()
        {
            BeginTransaction();
        }

        /// <summary>
        /// Realizar Commit
        /// </summary>
        void IUnitOfWork.Commit()
        {
            Commit();
        }

        /// <summary>
        /// Realizar rollback
        /// </summary>
        void IUnitOfWork.Rollback()
        {
            Rollback();
        }

        /// <summary>
        /// Execute assincronamente
        /// </summary>
        /// <param name="pComando"></param>
        async void IUnitOfWork.ExecutarAsync(string pComando)
        {
            await Task.Run(() => ExecutarAsync(pComando));
        }

        /// <summary>
        /// Executar
        /// </summary>
        /// <param name="pComando"></param>
        void IUnitOfWork.Executar(string pComando)
        {
            Executar(pComando);
        }

        /// <summary>
        /// Executa a procedure
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        void IUnitOfWork.ExecutarProcedure(string pNomeProcedure, object pObjeto)
        {
            ExecutarProcedure(pNomeProcedure, pObjeto);
        }

        /// <summary>
        /// Executa a procedure assincronamente
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        async void IUnitOfWork.ExecutarProcedureAsync(string pNomeProcedure, object pObjeto)
        {
            await Task.Run(() => ExecutarProcedureAsync(pNomeProcedure, pObjeto));
        }

        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <returns></returns>
        DataSet IUnitOfWork.ConsultaPorProcedure(string pNomeProcedure) => ConsultaPorProcedure(pNomeProcedure);
        
        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <param name="pObjeto">Objeto com os parametros</param>
        /// <returns></returns>
        DataSet IUnitOfWork.ConsultaPorProcedure(string pNomeProcedure, object pObjeto) => ConsultaPorProcedure(pNomeProcedure, pObjeto);

        /// <summary>
        /// Consulta um DataSet
        /// </summary>
        /// <param name="pComando"></param>
        /// <returns></returns>
        DataSet IUnitOfWork.Consulta(string pComando) => Consulta(pComando);

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <returns></returns>
        StringBuilder IUnitOfWork.MontaInsertPorAttributo(object pModel) =>
            MontaInsertPorAttributo(pModel);

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <param name="pStrBuilder">String Builder</param>
        /// <returns></returns>
        StringBuilder IUnitOfWork.MontaInsertPorAttributo(object pModel, ref StringBuilder pStrBuilder) =>
            MontaInsertPorAttributo(pModel, ref pStrBuilder);

        #endregion
    }

}
