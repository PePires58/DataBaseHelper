#region Histórico de Manutenção
/*
 Data: 22/02/2020
 Programador: Pedro Henrique Pires
 Descrição: Implementação Inicial da classe.
 */

/*
Data: 29/02/2020
Programador: Pedro Henrique Pires
Descrição: Inclusão de método de executar procedure.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DataBaseHelper.Interfaces
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Abrir transação
        /// </summary>
        void BeginTransaction();
        
        /// <summary>
        /// Realizar Commit
        /// </summary>
        void Commit();

        /// <summary>
        /// Realizar rollback
        /// </summary>
        void Rollback();


        /// <summary>
        /// Execute assincronamente
        /// </summary>
        /// <param name="pComando"></param>
        void ExecutarAsync(string pComando);


        /// <summary>
        /// Executar
        /// </summary>
        /// <param name="pComando"></param>
        void Executar(string pComando);

        /// <summary>
        /// Executa a procedure
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        void ExecutarProcedure(string pNomeProcedure, object pObjeto);


        /// <summary>
        /// Executa a procedure assincronamente
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        void ExecutarProcedureAsync(string pNomeProcedure);

        /// <summary>
        /// Executa a procedure assincronamente
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procedure</param>
        /// <param name="objeto">Objeto</param>
        void ExecutarProcedureAsync(string pNomeProcedure, object pObjeto);


        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <returns></returns>
        DataSet ConsultaPorProcedure(string pNomeProcedure);


        /// <summary>
        /// Consulta um dataset
        /// </summary>
        /// <param name="pNomeProcedure">Nome da procecure</param>
        /// <param name="pObjeto">Objeto com os parametros</param>
        /// <returns></returns>
        DataSet ConsultaPorProcedure(string pNomeProcedure, object pObjeto);


        /// <summary>
        /// Consulta um DataSet
        /// </summary>
        /// <param name="pComando"></param>
        /// <returns></returns>
        DataSet Consulta(string pComando);

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <returns></returns>
        StringBuilder MontaInsertPorAttributo(object pModel);

        /// <summary>
        /// Monta a instrução de Inserção no banco a partir do objeto com os atributos
        /// </summary>
        /// <param name="pModel">Modelo</param>
        /// <param name="pStrBuilder">String Builder</param>
        /// <returns></returns>
        StringBuilder MontaInsertPorAttributo(object pModel, ref StringBuilder pStrBuilder);
    }
}
